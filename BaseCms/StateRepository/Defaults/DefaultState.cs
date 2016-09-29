using System.Collections.Generic;
using System.Linq;
using BaseCms.CRUDRepository.Core.Intefaces;
using BaseCms.DependencyResolution;
using BaseCms.Security;
using BaseCms.StateRepository.Base;
using BaseCms.Views.ActionButtons;
using BaseCms.Views.Detail.Interfaces;

namespace BaseCms.StateRepository.Defaults
{
    public class DefaultState : StateBase
    {
        #region ActionButtons

        protected static readonly ActionButton SaveButton = new ActionButton
        {
            Caption = "Сохранить",
            Class = "btn-success",
            CMSScopeMethod = "save()",
            Icon = "icon-ok",
            SaveButton = true
        };
        protected static readonly ActionButton CancelButton = new ActionButton
        {
            Caption = "Отмена",
            CMSScopeMethod = "closeTab()",
            Icon = "icon-undo"
        };
        protected static readonly ActionButton DeleteButton = new ActionButton
        {
            Caption = "Удалить",
            Class = "btn-danger",
            CMSScopeMethod = "remove()",
            Icon = "icon-remove"
        };

        #endregion

        public override bool CheckPermission(IQuery query, string collectionName, string objectId)
        {
            var type = query.GetType();
            return IoC.SecurityProvider.CheckPermission(type, collectionName);
        }

        public override IEnumerable<ActionButton> GetButtons(string collectionName, string identifier, IDetailMetadata metadata = null)
        {
            var buttons = new List<ActionButton>();
            var permissions = IoC.SecurityProvider.GetCollectionPermissions(collectionName).ToArray();
            var insertPerm = permissions.Any(cp => cp.GetType() == typeof(InsertObjectPermission));
            var updatePerm = permissions.Any(cp => cp.GetType() == typeof(UpdateObjectPermission));

            bool addSaveAndCancelButtons;

            if (!string.IsNullOrEmpty(identifier))
            {
                int id;
                if (!int.TryParse(identifier, out id)) id = 1;

                if (id > 0)
                {
                    if (permissions.Any(cp => cp.GetType() == typeof(DeleteObjectPermission)))
                        buttons.Add(DeleteButton);
                    addSaveAndCancelButtons = updatePerm;
                }
                else addSaveAndCancelButtons = insertPerm;
            }
            else addSaveAndCancelButtons = insertPerm;

            if (addSaveAndCancelButtons)
            {
                buttons.Insert(0, SaveButton);
                buttons.Add(CancelButton);
            }

            return buttons;
        }
    }
}
