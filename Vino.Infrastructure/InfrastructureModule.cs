using Autofac;
using BetterThanYou.Core.Interfaces.Services.Account;
using BetterThanYou.Infrastructure.Repositories;

namespace BetterThanYou.Infrastructure;

public class InfrastructureModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<AccountRepository>()
            .As<IAccountRepository>()
            .InstancePerLifetimeScope();
    }
    
}