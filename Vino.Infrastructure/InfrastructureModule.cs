using Autofac;
using BetterThanYou.Core.Interfaces.Client;
using BetterThanYou.Core.Interfaces.File;
using BetterThanYou.Core.Interfaces.Order;
using BetterThanYou.Core.Interfaces.Product;
using BetterThanYou.Core.Interfaces.Services.Account;
using BetterThanYou.Infrastructure.Repositories;
using BetterThanYou.Infrastructure.Services;

namespace BetterThanYou.Infrastructure;

public class InfrastructureModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<AccountRepository>()
            .As<IAccountRepository>()
            .InstancePerLifetimeScope();
        
        builder.RegisterType<ProductRepository>()
            .As<IProductRepository>()
            .InstancePerLifetimeScope();
        
        builder.RegisterType<CloudinaryFileStorageService>()
            .As<IFileStorageService>()
            .InstancePerLifetimeScope();
        
        builder.RegisterType<ClientRepository>()
            .As<IClientRepository>()
            .InstancePerLifetimeScope();
        
        builder.RegisterType<OrderRepository>()
            .As<IOrderRepository>()
            .InstancePerLifetimeScope();
    }
    
}