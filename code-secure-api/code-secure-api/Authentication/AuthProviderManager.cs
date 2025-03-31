using Aguacongas.AspNetCore.Authentication;
using CodeSecure.Core.Entity;
using Microsoft.AspNetCore.Authentication;

namespace CodeSecure.Authentication;

public sealed class AuthProviderManager(
    IAuthenticationSchemeProvider schemeProvider,
    OptionsMonitorCacheWrapperFactory wrapperFactory,
    IDynamicProviderStore<AuthProviders> store,
    IEnumerable<Type> managedTypes)
    : NoPersistentDynamicManager<AuthProviders>(schemeProvider, wrapperFactory, managedTypes)

{
    private readonly IAuthenticationSchemeProvider schemeProvider = schemeProvider;

    public void Load()
    {
        foreach (var authProvider in store.SchemeDefinitions)
        {
            if (!authProvider.Enable) continue;
            if (ManagedHandlerType.Contains<Type>(authProvider.HandlerType))
                base.AddAsync(authProvider).GetAwaiter().GetResult();
        }
    }

    public override async Task AddAsync(AuthProviders definition, CancellationToken cancellationToken = new CancellationToken())
    {
        await store.AddAsync(definition, cancellationToken);
        if (definition.Enable)
        {
            await base.AddAsync(definition, cancellationToken);
        }
    }
    
    public override async Task UpdateAsync(
        AuthProviders definition,
        CancellationToken cancellationToken = default (CancellationToken))
    {
        if (definition.Enable)
        {
            if (await schemeProvider.GetSchemeAsync(definition.Scheme) == null)
            {
                await base.AddAsync(definition, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                await base.UpdateAsync(definition, cancellationToken).ConfigureAwait(false);
            }
        }
        else
        {
            await base.RemoveAsync(definition.Scheme, cancellationToken).ConfigureAwait(false);
        }
        
        await store.UpdateAsync(definition, cancellationToken).ConfigureAwait(false);
    }
    
    public override async Task RemoveAsync(string name, CancellationToken cancellationToken = default (CancellationToken))
    {
        await base.RemoveAsync(name, cancellationToken).ConfigureAwait(false);
        var definition = await store.FindBySchemeAsync(name, cancellationToken);
        if (definition == null)
            return;
        await store.RemoveAsync(definition, cancellationToken).ConfigureAwait(false);
    }
    
    public async Task<AuthProviders?> FindBySchemeAsync(string scheme)
    {
        if (!string.IsNullOrWhiteSpace(scheme))
        {
            return await store.FindBySchemeAsync(scheme);
        }
        return null;
    }
}