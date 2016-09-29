using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using BaseCms.CRUDRepository.Core;
using BaseCms.CRUDRepository.Core.Intefaces;
using BaseCms.Common.Binders;
using BaseCms.Configuration;
using BaseCms.DependencyResolution;
using BaseCms.Init;
using BaseCms.ViewsEngine;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(BaseCmsConfig), "Register")]
namespace BaseCms.Init
{
    public class BaseCmsConfig
    {
        public static void Register()
        {
            ((Route)RouteTable.Routes["Default"]).Defaults["controller"] = "Root";
            //DependencyRegistrar.EnsureDependenciesRegistered();
            var container = IoC.Container;
            DependencyResolver.SetResolver(new StructureMapDependencyResolver(container));
            //GlobalConfiguration.Configuration.DependencyResolver = new StructureMapDependencyResolver(container);

            RegisterAllQueries();

            ModelBinders.Binders.Add(typeof(JDataTableSettings), new JDataTableModelBinder());

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new BaseCmsViewEngine());
        }

        private static void RegisterAllQueries()
        {
            var queryResolver = IoC.Container.GetInstance<QueryResolver>();
            var type = typeof(IQueryInitializer);


            var myCustomSection = (QuerySetterAssemblyElementSection)ConfigurationManager.GetSection("customSection");
            var types = new List<Type>();
            foreach (QuerySetterAssemblyElement element in myCustomSection.Elements)
            {
                if (!string.IsNullOrEmpty(element.Name))
                {
                    //string name = element.Name;
                    string assembly = element.Assembly;
                    //bool shouldrun = element.ShouldRun;
                    types.AddRange(Assembly.Load(assembly).GetTypes()
                        .Where(p => type.IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract));

                }
            }
            //var types =
            //    Assembly.GetAssembly(typeof(NewtonCMS5Config))
            //            .GetTypes()
            //            .Where(p => type.IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract);

            //var types = AppDomain.CurrentDomain.GetAssemblies().ToList()
            //                     .SelectMany(s => s.GetTypes())
            //                     .Where(p => type.IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract);
            var curtypes = new List<IQueryInitializer>();
            foreach (var a in types)
            {

                if (a.Name != "MongoQueryInitializerBase`1")
                {
                    var d = (IQueryInitializer)Activator.CreateInstance(a);
                    curtypes.Add(d);
                }

            }
            //types.Select(a => (IQueryInitializer)Activator.CreateInstance(a))
            //     .ToList();
            curtypes.ForEach(f => f.Init(queryResolver));
        }
    }
}

