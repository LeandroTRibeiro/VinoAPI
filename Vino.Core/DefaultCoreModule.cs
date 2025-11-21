using Autofac;
using BetterThanYou.Core.Interfaces.Services.Account;
using BetterThanYou.Core.Services.Account;

namespace BetterThanYou.Core;

public class DefaultCoreModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<AccountService>()
            .As<IAccountService>()
            .InstancePerLifetimeScope();
    }
}