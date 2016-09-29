using System;
using System.Configuration;
using System.Threading;
using StructureMap;
using BaseCms.CRUDRepository.Core;
using BaseCms.CRUDRepository.Serialization;
using BaseCms.CRUDRepository.Serialization.Interfaces;
using BaseCms.Common;
using BaseCms.Common.Image;
using BaseCms.Common.Image.Interfaces;
using BaseCms.Common.Typograph;
using BaseCms.Common.Typograph.Interfaces;
using BaseCms.Events;
using BaseCms.Manager.Interfaces;
using BaseCms.Model.Interfaces;
using BaseCms.Security;
using BaseCms.StateRepository.Defaults;
using BaseCms.Views.Detail;
using BaseCms.Views.Detail.Interfaces;
using BaseCms.Views.List;
using BaseCms.Views.List.Interfaces;

namespace BaseCms.DependencyResolution
{
    public static class IoC
    {

        private static readonly Lazy<Container> _containerBuilder =
            new Lazy<Container>(defaultContainer, LazyThreadSafetyMode.ExecutionAndPublication);

        public static IContainer Container => _containerBuilder.Value;

        private static Container defaultContainer()
        {   

             
            return new Container(x =>
            {
                _manager = GetManager();

                var eventSource = new EventSource();
                x.Scan(scan =>
                {
                    //scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                });

                x.For<IMetadataProvider<IListMetadata>>().Use(new DefaultListMetadataProvider());
                x.For<IMetadataProvider<IDetailMetadata>>().Use(new DefaultDetailMetadataProvider());
                x.For<SecurityRepository>().Use<XmlFileSecurityRepository>();
                x.For<SecurityProvider>().Singleton().Use<SecurityProvider>();
                x.For<QueryResolver>().Singleton().Use<QueryResolver>();
                //x.For<ILogger>().Singleton().Use(new Logger());
                x.For<ISerializer>().Singleton().Use(new XmlSerializer());
                x.For<DefaultState>().Singleton().Use(new DefaultState());
                x.For<ITypograph>().Singleton().Use(new Typograph());
                x.For<IImageCropper>().Singleton().Use(new DefaultImageCropper());
                x.For<EventSource>().Singleton().Use(eventSource);

                x.For<IBackofficeManager>().Singleton().Use(_manager);                 
                _manager.InitializeIoCContainer(x);
                _manager.SubscribeToEvents(eventSource);
            });
        }


        //public static IContainer Container;
        //public static IContainer Initialize()
        //{
        //    _manager = GetManager();

        //    var eventSource = new EventSource();

        //    ObjectFactory.Initialize(x =>
        //    {
        //        x.Scan(scan =>
        //        {
        //            //scan.TheCallingAssembly();
        //            scan.WithDefaultConventions();
        //        });

        //        x.For<IMetadataProvider<IListMetadata>>().Use(new DefaultListMetadataProvider());
        //        x.For<IMetadataProvider<IDetailMetadata>>().Use(new DefaultDetailMetadataProvider());
        //        x.For<SecurityRepository>().Use<XmlFileSecurityRepository>();
        //        x.For<SecurityProvider>().Singleton().Use<SecurityProvider>();
        //        x.For<QueryResolver>().Singleton().Use<QueryResolver>();
        //        //x.For<ILogger>().Singleton().Use(new Logger());
        //        x.For<ISerializer>().Singleton().Use(new XmlSerializer());
        //        x.For<DefaultState>().Singleton().Use(new DefaultState());
        //        x.For<ITypograph>().Singleton().Use(new Typograph());
        //        x.For<IImageCropper>().Singleton().Use(new DefaultImageCropper());
        //        x.For<EventSource>().Singleton().Use(eventSource);

        //        x.For<IBackofficeManager>().Singleton().Use(_manager);
        //        _manager.InitializeIoCContainer(x);
        //    });

        //    Container = ObjectFactory.Container;

        //    _manager.SubscribeToEvents(eventSource);

        //    return Container;
        //}

        public static SecurityProvider SecurityProvider => Container.GetInstance<SecurityProvider>();

        private static IBackofficeManager _manager;

        private static IBackofficeManager GetManager()
        {
            var backofficeManagerNamespace = ConfigurationManager.AppSettings["BackofficeManager"];

            if (string.IsNullOrEmpty(backofficeManagerNamespace)) throw new Exception("В конфигурации не задан BackofficeManager");

            return InstanceCreator.CreateInstance<IBackofficeManager>(backofficeManagerNamespace);
        }
    }
}
