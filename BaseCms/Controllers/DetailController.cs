using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using BaseCms.CRUDRepository;
using BaseCms.CRUDRepository.Core;
using BaseCms.CRUDRepository.DetailView;
using BaseCms.CRUDRepository.Serialization;
using BaseCms.CRUDRepository.Serialization.Interfaces;
using BaseCms.Controllers.Base;
using BaseCms.DependencyResolution;
using BaseCms.Helpers;
using BaseCms.Logging;
using BaseCms.MetadataValueProviders.Interfaces;
using BaseCms.Model;
using BaseCms.StateRepository.Defaults;
using BaseCms.Views.ActionButtons;
using BaseCms.Views.Detail;
using BaseCms.Views.Detail.Enums;
using BaseCms.Views.Detail.Interfaces;
using BaseCms.Views.List.ListViewMetadata;
using BaseCms.Views.Xml;
using static System.String;

namespace BaseCms.Controllers
{
    public class DetailController : CmsControllerBase
    {
        private readonly DefaultState _defaultState;
        private readonly ISerializer _serializer;

        public DetailController(QueryResolver queryResolver, DefaultState defaultState, ISerializer serializer)
            : base(queryResolver)
        {
            _defaultState = defaultState;
            _serializer = serializer;
        }

        private DataWithIdentifier<object, ObjectCollectionWithId> GetDataWithIdentifier(string collectionName, string identifier, string detailViewGuid)
        {
            var itemIdentifier = new ObjectCollectionWithId { Id = identifier, CollectionName = collectionName };
            object obj =
                !IsNullOrEmpty(identifier)
                    ? QueryResolver.Execute(new GetByIdQueryBase(identifier, detailViewGuid), collectionName)
                    : QueryResolver.Execute(new NewObjectQueryBase(), collectionName);
            var type = QueryResolver.Execute(new GetTypeQueryBase(), collectionName);
            return new DataWithIdentifier<object, ObjectCollectionWithId>(type, obj, itemIdentifier);
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Authorize]
        public ActionResult Edit(string collectionName, string identifier)
        {
            var structure = GetDataWithIdentifier(collectionName, identifier, null);

            var provider = new DefaultDetailMetadataProvider();
            var type = QueryResolver.Execute(new GetTypeQueryBase(), collectionName);
            var metadata = provider.GetMetadata(type);

            var viewName = string.IsNullOrEmpty(metadata.CustomEditView)
                               ? "Edit"
                               : metadata.CustomEditView;

            var state = QueryResolver.ContainsQuery(typeof(GetStateQueryBase), collectionName)
                                  ? QueryResolver.Execute(new GetStateQueryBase(identifier), collectionName)
                                  : _defaultState;

            var buttons = state.GetButtons(collectionName, identifier, metadata).ToArray();

            if (!buttons.Any(b => b.SaveButton))
            {
                var l = metadata.Items.ToList();
                l.ForEach(i => i.IsEditable = false);
                metadata.Items = l;
            }

            return View(viewName,
                        new DetailViewModel
                        {
                            Structure = structure,
                            DefaultDetailMetadata = metadata,
                            Buttons = buttons
                        });
        }

        [System.Web.Http.HttpPost]
        [ValidateInput(false)]
        [Authorize]
        public JsonResult Save(string collectionName, string identifier, string upperIdentifier, string detailViewGuid, string newStateId)
        {
            try
            {
                var objectType = QueryResolver.Execute(new GetTypeQueryBase(), collectionName);
                var provider = new DefaultDetailMetadataProvider();
                var metadata = provider.GetMetadata(objectType);

                // Если upperIdentifier существует, значит сохранение происходит во временное хранилище (сессию), иначе в БД
                var globalSave = IsNullOrEmpty(upperIdentifier);

                // Получаем объект для заполнения его свойств
                var dataWithId = GetDataWithIdentifier(collectionName, identifier, globalSave ? null : detailViewGuid);

                var obj = dataWithId.Data;

                // Получаем текущее состояние объекта
                var currentState = (QueryResolver.ContainsQuery(typeof(GetStateQueryBase), collectionName))
                                             ? QueryResolver.Execute(new GetStateQueryBase(identifier), collectionName)
                                             : _defaultState;

                object oldObject = null;
                if ((globalSave) && (!IsNullOrEmpty(identifier)))
                {
                    oldObject = _serializer.CopyObject(obj as XmlSerializableArgumentBase);
                }

                //todo:Тут должна быть валидация

                //SetValues(obj, HttpContext.Request.Unvalidated.Form,
                //          metadata.Items);
                //.Where(
                //p => p.LinkMetadata == null || (p.LinkMetadata.Multiplier == LinkMultiplier.ToOne && !p.LinkMetadata.DetailViewSave)));
                SetValues(obj, HttpContext.Request.Unvalidated.Form,
                          metadata.Items.Where(
                              p => p.LinkMetadata == null || (p.LinkMetadata.Multiplier == LinkMultiplier.ToOne && !p.LinkMetadata.DetailViewSave)));
                string newId;
                if (IsNullOrEmpty(identifier))
                {
                    var ex = globalSave ? new InsertObject(obj, null, detailViewGuid) : new InsertObject(obj, detailViewGuid);
                    newId = QueryResolver.Execute(ex, collectionName, currentState);
                    dataWithId.Identifier.Id = newId ?? metadata.Items.First(p => p.PropertyName == "Id" || p.PropertyName == "EntityId").GetValue(obj).ToString();
                }
                else
                {
                    newId = identifier;
                    QueryResolver.Execute(
                        globalSave ? new UpdateObject(obj, oldObject, detailViewGuid) : new UpdateObject(obj, detailViewGuid), collectionName, currentState);
                }

                // Сохраняем данные из временного хранилища в БД
                foreach (
                    var entity in
                        metadata.Items.Where(p => p.LinkMetadata != null && p.LinkMetadata.DetailViewSave)
                                .Select(m => m.LinkMetadata.CollectionName))
                {
                    if (string.IsNullOrEmpty(identifier) && string.IsNullOrEmpty(newId))
                    {
                        throw new Exception(
                            $"Запрос InsertObject коллекции {collectionName} не возвращает Id созданного объекта (возможное решение: inp.Input.Output = obj.Id.ToString(); в конце Register<InsertObject>(inp => {{   }}))");
                    }
                    QueryResolver.Execute(new SaveDetailViewObjects(detailViewGuid, String.IsNullOrEmpty(identifier) ? newId : null), entity);
                }

                SetValues(obj, HttpContext.Request.Unvalidated.Form,
                          metadata.Items.Where(
                              p => p.LinkMetadata != null && p.LinkMetadata.Multiplier == LinkMultiplier.ToMany));

                // Сохранение коллекций, ссылающихся на сохраняемый объект и редактируемых на карточке этого объекта.
                SaveCollections(collectionName, newId, identifier, detailViewGuid);

                if (!IsNullOrEmpty(newStateId))
                    currentState = QueryResolver.Execute(new ChangeObjectStateId(dataWithId.Identifier.Id, newStateId),
                        collectionName, currentState);

                return GetEditForm(dataWithId, metadata, currentState.GetButtons(collectionName, dataWithId.Identifier.Id, metadata));
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null) ex = ex.InnerException;
                return Json(new { success = false, error = ex.Message });
            }
        }

        private void SetValues(object model, NameValueCollection form, IEnumerable<DetailMetadataItem> metadata)
        {

            foreach (var key in form.AllKeys)
            {
                var keyName = key.Split('.').Last();

                if (keyName.StartsWith("xml_"))
                {
                    var array = key.Split('_');
                    var objPropName = array[1];
                    var xmlPropName = array[2];

                    var objProp = model.GetType().GetProperty(objPropName);

                    var dict = objProp.GetValue(model) as Dictionary<string, string> ??
                               new Dictionary<string, string>();
                    if (dict.ContainsKey(xmlPropName)) dict[xmlPropName] = form[key];
                    else dict.Add(xmlPropName, form[key]);

                    objProp.SetValue(model, dict);
                }
            }

            foreach (var row in metadata)
            {
                var formItemName = row.PropertyName;

                foreach (var formKey in form.AllKeys)
                {
                    if (formKey.Split('.').Last() == formItemName)
                    {
                        try
                        {
                            row.SetValue(model, form[formKey], QueryResolver);
                        }
                        catch (Exception)
                        {

                            row.SetValue(model, form[formKey], QueryResolver);
                        }

                    }
                }
            }
        }


        private void SaveCollections(string foreignCollectionName, string foreignIdentifier, string initialForeignIdentifier, string upperDetailViewGuid)
        {
            var form = HttpContext.Request.Unvalidated.Form;

            // Парсим названия коллекций, количество элементов и список полей (поля формы с индексами в названиях - "[ ]").
            var collections =
                (form.AllKeys.Where(key => key.Contains("["))
                    .Select(key => new {key, parts = key.Split('.')})
                    .Select(@t => new
                    {
                        CollectionName = @t.parts[@t.parts.Length - 2],
                        PropertyName = Regex.Match(@t.parts.Last(), "(.+)\\[").Groups[1].Value,
                        PropertyFullName = Regex.Match(@t.key, "(.+)\\[").Groups[1].Value,
                        Index = int.Parse(Regex.Match(@t.parts.Last(), "\\[(.+)\\]").Groups[1].Value)
                    })).Where(p => p.Index >= 0).GroupBy(p => new {p.CollectionName})
                    .Select(item => new
                    {
                        item.Key.CollectionName,
                        MaxIndex = item.Max(q => q.Index),
                        PropertyNames = item.Select(q => new {q.PropertyName, q.PropertyFullName}).Distinct().ToList(),
                    });

            foreach (var collection in collections)
            {
                var objectType = QueryResolver.Execute(new GetTypeQueryBase(), collection.CollectionName);
                var provider = new DefaultDetailMetadataProvider();
                var collectionMetadata = provider.GetMetadata(objectType);

                for (int i = 0; i <= collection.MaxIndex; i++)
                {
                    var keyId =
                        $"{collection.PropertyNames.First(p => p.PropertyFullName.ToLowerInvariant().EndsWith(".id")).PropertyFullName}[{i}]";
                    string identifier = form[keyId];

                    object obj = IsNullOrEmpty(identifier)
                                     ? QueryResolver.Execute(new NewObjectQueryBase(), collection.CollectionName)
                                     : QueryResolver.Execute(new GetByIdQueryBase(identifier), collection.CollectionName);

                    // Удаление.
                    var deletedKey =
                        collection.PropertyNames.First(p => p.PropertyName == "EditableCollectionDeletedItem")
                                  .PropertyFullName;
                    if (!IsNullOrEmpty(form[$"{deletedKey}[{i}]"]))
                    {
                        if (!IsNullOrEmpty(identifier))
                        {
                            QueryResolver.Execute(new DeleteObject(identifier), collection.CollectionName);
                        }
                        continue;
                    }

                    // Сохранение.
                    foreach (
                        var formKey in
                            collection.PropertyNames.Where(
                                p =>
                                p.PropertyName.ToLowerInvariant() != "id" &&
                                p.PropertyName != "EditableCollectionDeletedItem"))
                    {
                        var key = $"{formKey.PropertyFullName}[{i}]";
                        var row = collectionMetadata.Items.First(p => p.PropertyName == formKey.PropertyName);
                        row.SetValue(obj, form[key]);
                    }

                    // Сохранение значения внешнего ключа.
                    var foreignKeyMetadata = collectionMetadata.Items.FirstOrDefault(
                        p =>
                        p.LinkMetadata != null &&
                        p.LinkMetadata.CollectionName == foreignCollectionName);

                    foreignKeyMetadata?.SetValue(obj, foreignIdentifier);

                    if (IsNullOrEmpty(identifier))
                    {
                        identifier = QueryResolver.Execute(new InsertObject(obj), collection.CollectionName);
                    }
                    else
                    {
                        QueryResolver.Execute(new UpdateObject(obj, null), collection.CollectionName);
                    }

                    // Сохраняем данные из временного хранилища в БД
                    foreach (
                        var entity in
                            collectionMetadata.Items.Where(p => p.LinkMetadata != null && p.LinkMetadata.DetailViewSave)
                                              .Select(m => m.LinkMetadata.CollectionName))
                    {
                        if (string.IsNullOrEmpty(identifier))
                        {
                            throw new Exception(
                                $"Запрос InsertObject коллекции {collection.CollectionName} не возвращает Id созданного объекта (возможное решение: inp.Input.Output = obj.Id.ToString(); в конце Register<InsertObject>(inp => {{   }}))");
                        }
                        QueryResolver.Execute(
                            new SaveDetailViewObjects(upperDetailViewGuid + "_" + collection.CollectionName + "[" + i.ToString(CultureInfo.InvariantCulture) + "]",
                                                      identifier), entity);
                    }
                }
            }
        }

        public JsonResult GetEditForm(DataWithIdentifier<object, ObjectCollectionWithId> dataWithIdentifier, IDetailMetadata metadata, IEnumerable<ActionButton> buttons)
        {
            ViewData.Model = new DetailViewModel
            {
                Structure = dataWithIdentifier,
                DefaultDetailMetadata = metadata,
                Buttons = buttons
            };
            using (var sw = new StringWriter())
            {
                var viewName = IsNullOrEmpty(metadata.CustomEditView)
                                   ? "Edit"
                                   : metadata.CustomEditView;
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                var htmlContent = sw.GetStringBuilder().ToString();
                return Json(new { html = htmlContent, id = dataWithIdentifier.Identifier.Id, success = true });
            }
        }

        [System.Web.Http.HttpPost]
        public ActionResult Remove(string collectionName, string items, string upperIdentifier, string detailViewGuid)
        {
            // Если параметр upperIdentifier пуст, значит идёт глобальное удаление, пробуем очистить данные в сессии
            if (IsNullOrEmpty(upperIdentifier)) ClearDetailViewData(collectionName, detailViewGuid);

            var ids = items.Split(',');

            var checkStateForEveryObject = QueryResolver.ContainsQuery(typeof(GetStateQueryBase), collectionName);

            foreach (var id in ids)
            {
                try
                {
                    // Получаем текущее состояние объекта
                    var currentState = checkStateForEveryObject
                                           ? QueryResolver.Execute(new GetStateQueryBase(id), collectionName)
                                           : _defaultState;

                    QueryResolver.Execute(new DeleteObject(id, detailViewGuid, upperIdentifier), collectionName, currentState);
                }
                catch (Exception ex)
                {
                    while (ex.InnerException != null) ex = ex.InnerException;
                    return Json(new { success = false, error = ex.Message });
                }
            }
            return Json(new { success = true });
        }

        [System.Web.Http.HttpPost]
        public ActionResult CreateToManyLink(string collectionName, string upperIdentifier, string linkedIdentifier, string additionalData, string detailViewGuid)
        {
            try
            {
                QueryResolver.Execute(new CreateToManyLink(upperIdentifier, linkedIdentifier, additionalData, detailViewGuid), collectionName);
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null) ex = ex.InnerException;
                return Json(new { success = false, error = ex.Message });
            }

            return Json(new { success = true });
        }

        [System.Web.Http.HttpPost]
        public JsonResult ChangeState(string collectionName, string id, string stateId, string detailViewGuid, string detailViewEntityId)
        {
            // Если параметр detailViewEntityId пуст, значит идёт глобальное изменение состояния без сохранения, можем очищать данные во временном хранилище
            //if (String.IsNullOrEmpty(detailViewEntityId)) ClearDetailViewData(collectionName, detailViewGuid);
            try
            {
                // Получаем текущее состояние объекта
                var currentState = QueryResolver.Execute(new GetStateQueryBase(id), collectionName);

                currentState = QueryResolver.Execute(new ChangeObjectStateId(id, stateId), collectionName, currentState);

                var objectType = QueryResolver.Execute(new GetTypeQueryBase(), collectionName);
                var provider = new DefaultDetailMetadataProvider();
                var metadata = provider.GetMetadata(objectType);

                // Получаем объект для заполнения его свойств
                var dataWithId = GetDataWithIdentifier(collectionName, id, detailViewGuid);

                return GetEditForm(dataWithId, metadata, currentState.GetButtons(collectionName, id, metadata));
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null) ex = ex.InnerException;
                return Json(new { success = false, error = ex.Message });
            }
        }

        public ActionResult EditableCollection(string collectionName, string upperIdentifier, string detailViewGuid, EditableCollectionMetadata editableCollectionMetadata)
        {
            var collectionType = QueryResolver.Execute(new GetTypeQueryBase(), collectionName);
            // Задаём сортировку по умолчанию: сортировка по первичному ключу по возрастанию
            string initSortExpression = new DefaultDetailMetadataProvider().GetMetadata(collectionType).Items.First(i => i.IsKey).PropertyName + " asc";
            var collection = QueryResolver.Execute(new GetListQueryBase(0, Int32.MaxValue, initSortExpression, new[] { upperIdentifier }.ToList()), collectionName);

            return View(editableCollectionMetadata.ViewName, new EditableCollectionViewModel { Collection = collection, Type = collectionType, UpperIdentifier = upperIdentifier, DetailViewGuid = detailViewGuid });
        }

        public ActionResult ListView(bool isEditable, string collectionName, string upperCollectionName, string upperIdentifier, string detailViewGuid, string filterContext, ListViewMetadata listViewMetadata, PopupMetadata popupMetadata, string collectionIndex)
        {
            var type = QueryResolver.Execute(new GetTypeQueryBase(), collectionName);
            var structureWithCollectionName = new DataWithIdentifier<object, string>(type, null, collectionName);

            var filterContextUrlPart = Empty;
            if (!IsNullOrEmpty(filterContext)) filterContextUrlPart = "filterContext=" + filterContext + "&";
            var listMetadataUrlPart = Empty;
            if (listViewMetadata.ListMetadataProviderType != null) listMetadataUrlPart =
                $"listMetadataProvider={listViewMetadata.ListMetadataProviderType.FullName}&";
            var detailViewGuidUrlPart = Empty;
            if (!IsNullOrEmpty(detailViewGuid)) detailViewGuidUrlPart = "detailViewGuid=" + detailViewGuid + "&";

            ViewBag.CollectionIndex = collectionIndex;

            return
                View(listViewMetadata.ViewName, new ListViewModel
                {
                    IsEditable = isEditable,
                    Url = Format("/List/Read/?{2}{3}{4}collectionName={0}&filters={1}", collectionName, upperIdentifier, filterContextUrlPart, listMetadataUrlPart, detailViewGuidUrlPart),
                    Structure = structureWithCollectionName,
                    UpperIdentifier = upperIdentifier,
                    UpperCollectionName = upperCollectionName,
                    DetailViewGuid = detailViewGuid,
                    ListViewSettings = listViewMetadata,
                    PopupSettings = popupMetadata
                });
        }

        /// <summary>
        /// Возвращает редактор словаря (ключ типа string, значение типа string)
        /// </summary>
        /// <param name="collectionName">Имя коллекции, у которой есть поле типа словарь (string, string)</param>
        /// <param name="identifier">Идентификатор коллекции, у которой есть поле типа словарь (string, string)</param>
        /// <param name="dictStrStrPropertyName">Имя поля типа словарь (string, string)</param>
        /// <param name="xmlMetadataCollectionName">Имя коллекции, которая содержит метаданные для элементов словаря</param>
        /// <param name="xmlMetadataIdentifier">Идентификатор коллекции, которая содержит метаданные для элементов словаря</param>
        /// <param name="xmlMetadataPropertyName">Имя поля, содержащего метаданные для элементов словаря</param>
        /// <param name="xmlMetadataValueProviderType">Имя типа источника данных, который будет давать типы по параметру <param name="xmlMetadataIdentifier"/>, дополняющие метаданные для элементов словаря. Данный тип обязательно должен быть прописан в IoC</param>
        /// <param name="viewName">Имя view</param>
        /// <returns>Html, представляющий собой редактор словаря</returns>
        public ActionResult XmlEditor(string collectionName, string identifier, string dictStrStrPropertyName, string xmlMetadataCollectionName, string xmlMetadataIdentifier, string xmlMetadataPropertyName, string xmlMetadataValueProviderType, string viewName)
        {
            // Получаем метаданные от коллекции xmlMetadtaCollectionName
            var xmlMetadataType = QueryResolver.Execute(new GetTypeQueryBase(), xmlMetadataCollectionName);

            string metadata;
            if (string.IsNullOrEmpty(xmlMetadataIdentifier))
                metadata = null;
            else
            {
                var xmlMetadata = QueryResolver.Execute(new GetByIdQueryBase(xmlMetadataIdentifier),
                    xmlMetadataCollectionName);

                metadata = xmlMetadataType.GetProperty(xmlMetadataPropertyName).GetValue(xmlMetadata) as string;
            }


            // Получаем данные (в виде словаря) от коллекции collectionName
            var objWithDictStrStrType = QueryResolver.Execute(new GetTypeQueryBase(), collectionName);

            var objWithDictStrStr = QueryResolver.Execute(new GetByIdQueryBase(identifier), collectionName);

            if (objWithDictStrStr == null)
                return new EmptyResult();
            var data = objWithDictStrStrType.GetProperty(dictStrStrPropertyName).GetValue(objWithDictStrStr);

            var structureWithCollectionName = new DataWithIdentifier<object, string>(typeof(Dictionary<string, string>), data, dictStrStrPropertyName);

            Type overrideMetadataType = null;
            if (!string.IsNullOrEmpty(xmlMetadataValueProviderType))
            {
                var valueProviderType = Type.GetType(xmlMetadataValueProviderType);
                var provider = IoC.Container.GetInstance(valueProviderType) as IPageSettingsValueProvider;
                if (provider != null) overrideMetadataType = provider.GetValue(xmlMetadataIdentifier);
            }

            if (string.IsNullOrEmpty(viewName)) viewName = "XmlEditor";

            return View(viewName, new XmlEditorViewModel
            {
                Structure = structureWithCollectionName,
                XmlDetailMetadata = new XmlDetailMetadataProvider().GetMetadata(overrideMetadataType, DataContractSerializerHelper.Deserialize<Dictionary<string, string>>(metadata), dictStrStrPropertyName)
            });
        }

        [System.Web.Http.HttpPost]
        public JsonResult InlineEdit(string collectionName, string identifier, string propertyName, string value, string detailViewGuid)
        {
            var objectType = QueryResolver.Execute(new GetTypeQueryBase(), collectionName);
            var provider = new DefaultDetailMetadataProvider();
            var collectionMetadata = provider.GetMetadata(objectType);

            var obj = GetDataWithIdentifier(collectionName, identifier, detailViewGuid).Data;

            var oldValue = collectionMetadata.Items.First(p => p.PropertyName == propertyName).GetValue(obj);
            collectionMetadata.Items.First(p => p.PropertyName == propertyName).SetValue(obj, value);

            try
            {
                // Получаем текущее состояние объекта
                var currentState = (QueryResolver.ContainsQuery(typeof(GetStateQueryBase), collectionName))
                                             ? QueryResolver.Execute(new GetStateQueryBase(identifier), collectionName)
                                             : _defaultState;
                int areaMax;
                int.TryParse(identifier, out areaMax);

                if (areaMax > 0)
                    QueryResolver.Execute(new UpdateObject(obj, detailViewGuid), collectionName, currentState);
                return Json(new { success = true, result = value });
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                return Json(new { success = false, result = oldValue, error = ex.Message });
            }
        }

        [System.Web.Http.HttpPost]
        public ActionResult PopupEdit(string viewName, string collectionName, string identifier, string upperCollectionName, string upperIdentifier, string detailViewGuid, string customDetailMetadataProvider, string title, string height, string width, string noScroll, string top, string left, int screenWidth)
        {
            var dataWithId = GetDataWithIdentifier(collectionName, identifier, detailViewGuid);

            if (QueryResolver.ContainsQuery(typeof(BeforeEditQueryBase), collectionName))
            {
                QueryResolver.Execute(new BeforeEditQueryBase(dataWithId.Data), collectionName);
            }

            bool scroll;
            bool.TryParse(noScroll, out scroll);

            return View(viewName, new PopupEditViewModel
            {
                DataWithIdentifier = GetDataWithIdentifier(collectionName, identifier, detailViewGuid),
                DetailViewGuid = detailViewGuid,
                UpperCollectionName = upperCollectionName,
                UpperIdentifier = upperIdentifier,
                CustomDetailMetadataProvider = customDetailMetadataProvider,
                Title = title,
                Height = height,
                Width = width,
                Top = top,
                Left = left,
                NoScroll = scroll,
                ScreenWidth = screenWidth
            });
        }

        [System.Web.Http.HttpPost]
        public ActionResult MoveItem(string collectionName, string identifier, string upperIdentifier, string detailViewGuid, bool up)
        {
            try
            {
                QueryResolver.Execute(new MoveUpDown(identifier, upperIdentifier, detailViewGuid, up), collectionName);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                return Json(new { success = false, error = ex.Message });
            }
        }

        public ActionResult History(string collectionName, string identifier, string jDataTableDom = null)
        {
            var structureWithCollectionName = new DataWithIdentifier<object, string>(typeof(Audit), null, "Audit");

            return
                View(new ListViewModel
                {
                    Url = $"/List/Read/?collectionName={"Audit"}&filters={collectionName + "," + identifier}",
                    Structure = structureWithCollectionName,
                    ListViewSettings = new ListViewMetadata
                    {
                        JDataTableDom = "rt"
                    }
                });
        }

        public ActionResult ClearDetailViewData(string collectionName, string detailViewGuid)
        {
            var objectType = QueryResolver.Execute(new GetTypeQueryBase(), collectionName);
            var provider = new DefaultDetailMetadataProvider();
            var metadata = provider.GetMetadata(objectType);

            foreach (
                    var entity in
                        metadata.Items.Where(p => p.LinkMetadata != null && p.LinkMetadata.DetailViewSave)
                                .Select(m => m.LinkMetadata.CollectionName))
            {
                QueryResolver.Execute(new ClearDetailViewObjects(detailViewGuid), entity);
            }

            return new EmptyResult();
        }
    }
}
