using Autofac;
using BetterThanYou.Core.Interfaces.Client;
using BetterThanYou.Core.Interfaces.File;
using BetterThanYou.Core.Interfaces.Order;
using BetterThanYou.Core.Interfaces.Product;
using BetterThanYou.Core.Interfaces.Route;
using BetterThanYou.Core.Interfaces.Services.Account;
using BetterThanYou.Core.Services.Account;
using BetterThanYou.Core.Services.Client;
using BetterThanYou.Core.Services.Order;
using BetterThanYou.Core.Services.Product;
using BetterThanYou.Core.Services.Route;
using BetterThanYou.Infrastructure.Services;

namespace BetterThanYou.Core;

public class DefaultCoreModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<AccountService>()
            .As<IAccountService>()
            .InstancePerLifetimeScope();
        
        builder.RegisterType<ProductService>()
            .As<IProductService>()
            .InstancePerLifetimeScope();
        
        builder.RegisterType<FileStorageService>()
            .As<IFileStorageService>()
            .InstancePerLifetimeScope();
        
        builder.RegisterType<ClientService>()
            .As<IClientService>()
            .InstancePerLifetimeScope();
        
        builder.RegisterType<OrderService>()
            .As<IOrderService>()
            .InstancePerLifetimeScope();
        
        builder.RegisterType<RouteService>()
            .As<IRouteService>()
            .InstancePerLifetimeScope();

        builder.RegisterType<GeocodingService>()
            .As<IGeocodingService>()
            .InstancePerLifetimeScope();
    }
}