using BackofficeDemo.Model;
using BackofficeDemo.Repository.Custom;
using BackofficeDemo.Repository.Generic;
using StructureMap;
using StructureMap.Pipeline;

namespace BackofficeDemo.Repository
{
    public class DalManager
    {
        public void InitializeIoCContainer(ConfigurationExpression x)
        {
            x.For<IRepository<Partner>>().LifecycleIs(new UniquePerRequestLifecycle()).Singleton().Use(new Repository<Partner>());
            x.For<IRepository<PartnerType>>().LifecycleIs(new UniquePerRequestLifecycle()).Singleton().Use(new Repository<PartnerType>());
            x.For<IRepository<FoodCategory>>().LifecycleIs(new UniquePerRequestLifecycle()).Singleton().Use(new Repository<FoodCategory>());
            x.For<IRepository<PartnerCategory>>().LifecycleIs(new UniquePerRequestLifecycle()).Singleton().Use(new Repository<PartnerCategory>());
            x.For<IRepository<Product>>().LifecycleIs(new UniquePerRequestLifecycle()).Singleton().Use(new Repository<Product>());
            x.For<IRepository<PartnerUser>>().LifecycleIs(new UniquePerRequestLifecycle()).Singleton().Use(new Repository<PartnerUser>());
            x.For<IRepository<PartnerUserRole>>().LifecycleIs(new UniquePerRequestLifecycle()).Singleton().Use(new Repository<PartnerUserRole>());
            
            x.For<IRepository<City>>().LifecycleIs(new UniquePerRequestLifecycle()).Singleton().Use(new Repository<City>());
            x.For<ImageRepository>().LifecycleIs(new UniquePerRequestLifecycle()).Singleton().Use(new ImageRepository());
            x.For<IRepository<ImageSize>>().LifecycleIs(new UniquePerRequestLifecycle()).Singleton().Use(new Repository<ImageSize>());
            x.For<IRepository<Customer>>().LifecycleIs(new UniquePerRequestLifecycle()).Singleton().Use(new Repository<Customer>());
            x.For<IRepository<Order>>().LifecycleIs(new UniquePerRequestLifecycle()).Singleton().Use(new Repository<Order>());
            x.For<IRepository<User>>().LifecycleIs(new UniquePerRequestLifecycle()).Singleton().Use(new Repository<User>());
            x.For<IRepository<OrderStatusHistory>>().LifecycleIs(new UniquePerRequestLifecycle()).Singleton().Use(new Repository<OrderStatusHistory>());
            x.For<IRepository<ProductExtension>>().LifecycleIs(new UniquePerRequestLifecycle()).Singleton().Use(new Repository<ProductExtension>());
            x.For<IRepository<ProductExtensionGroup>>().LifecycleIs(new UniquePerRequestLifecycle()).Singleton().Use(new Repository<ProductExtensionGroup>());
            x.For<IRepository<ShoppingCartItem>>().LifecycleIs(new UniquePerRequestLifecycle()).Singleton().Use(new Repository<ShoppingCartItem>());
            x.For<IRepository<Review>>().LifecycleIs(new UniquePerRequestLifecycle()).Singleton().Use(new Repository<Review>());
        }
    }
}
