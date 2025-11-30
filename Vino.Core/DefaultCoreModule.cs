using Autofac;
using BetterThanYou.Core.Interfaces.File;
using BetterThanYou.Core.Interfaces.Product;
using BetterThanYou.Core.Interfaces.Services.Account;
using BetterThanYou.Core.Services.Account;
using BetterThanYou.Core.Services.Product;
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
    }
}