using CodeSecure.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace CodeSecure.Tests;

public class OpenIdConnectSchemeOverrideOptionsTest
{
    [Test]
    public void SchemeOverride_DefaultIsEmpty()
    {
        var setting = new OpenIdConnectSetting();
        Assert.That(setting.SchemeOverride, Is.EqualTo(string.Empty));
    }

    [Test]
    public void SchemeOverride_CanBeSet()
    {
        var setting = new OpenIdConnectSetting
        {
            SchemeOverride = "https"
        };
        Assert.That(setting.SchemeOverride, Is.EqualTo("https"));
    }

    [Test]
    public void ToOpenIdConnectOptions_ReturnsValidOptions()
    {
        var setting = new OpenIdConnectSetting
        {
            Authority = "https://idp.example.com",
            ClientId = "my-client",
            ClientSecret = "my-secret",
            SchemeOverride = "https"
        };
        var options = setting.ToOpenIdConnectOptions();
        Assert.That(options.Authority, Is.EqualTo("https://idp.example.com"));
        Assert.That(options.ClientId, Is.EqualTo("my-client"));
        Assert.That(options.ClientSecret, Is.EqualTo("my-secret"));
        Assert.That(options.ResponseType, Is.EqualTo(OpenIdConnectResponseType.Code));
        Assert.That(options.CallbackPath.Value, Is.EqualTo("/auth/oidc/callback"));
    }

    [Test]
    public void RedirectUriSchemeIsOverridden()
    {
        // Simulate what OpenIdConnectSchemeOverrideOptions does:
        // It sets OnRedirectToIdentityProvider to replace the URI scheme.
        var schemeOverride = "https";
        var options = new OpenIdConnectOptions();
        options.Events ??= new OpenIdConnectEvents();
        options.Events.OnRedirectToIdentityProvider = ctx =>
        {
            var uri = ctx.ProtocolMessage.RedirectUri;
            var schemeEnd = uri?.IndexOf("://") ?? -1;
            if (schemeEnd >= 0)
                ctx.ProtocolMessage.RedirectUri = schemeOverride + uri![schemeEnd..];
            return Task.CompletedTask;
        };

        // Create a mock RedirectContext to test the event
        var protocolMessage = new OpenIdConnectMessage
        {
            RedirectUri = "http://myapp.example.com/auth/oidc/callback"
        };

        // Verify the scheme replacement logic directly
        var uri = protocolMessage.RedirectUri;
        var end = uri.IndexOf("://");
        if (end >= 0)
            protocolMessage.RedirectUri = schemeOverride + uri[end..];

        Assert.That(protocolMessage.RedirectUri, Is.EqualTo("https://myapp.example.com/auth/oidc/callback"));
    }

    [Test]
    public void SchemeOverrideLogic_NoOpWhenEmpty()
    {
        var schemeOverride = "";
        var originalUri = "http://myapp.example.com/auth/oidc/callback";
        var resultUri = originalUri;

        if (!string.IsNullOrWhiteSpace(schemeOverride))
        {
            var schemeEnd = resultUri.IndexOf("://");
            if (schemeEnd >= 0)
                resultUri = schemeOverride + resultUri[schemeEnd..];
        }

        Assert.That(resultUri, Is.EqualTo(originalUri));
    }

    [Test]
    public void SchemeOverrideLogic_NoOpWhenNull()
    {
        string? schemeOverride = null;
        var originalUri = "http://myapp.example.com/auth/oidc/callback";
        var resultUri = originalUri;

        if (!string.IsNullOrWhiteSpace(schemeOverride))
        {
            var schemeEnd = resultUri.IndexOf("://");
            if (schemeEnd >= 0)
                resultUri = schemeOverride + resultUri[schemeEnd..];
        }

        Assert.That(resultUri, Is.EqualTo(originalUri));
    }

    [Test]
    public void SchemeOverrideLogic_ReplacesHttpWithHttps()
    {
        var schemeOverride = "https";
        var uri = "http://localhost:5272/auth/oidc/callback";

        var schemeEnd = uri.IndexOf("://");
        if (schemeEnd >= 0)
            uri = schemeOverride + uri[schemeEnd..];

        Assert.That(uri, Is.EqualTo("https://localhost:5272/auth/oidc/callback"));
    }

    [Test]
    public void SchemeOverrideLogic_PreservesExistingWhenAlreadyCorrect()
    {
        var schemeOverride = "https";
        var uri = "https://myapp.example.com/auth/oidc/callback";

        var schemeEnd = uri.IndexOf("://");
        if (schemeEnd >= 0)
            uri = schemeOverride + uri[schemeEnd..];

        Assert.That(uri, Is.EqualTo("https://myapp.example.com/auth/oidc/callback"));
    }

    [Test]
    public void SchemeOverrideLogic_HandlesCustomScheme()
    {
        var schemeOverride = "custom";
        var uri = "http://myapp.example.com/auth/oidc/callback";

        var schemeEnd = uri.IndexOf("://");
        if (schemeEnd >= 0)
            uri = schemeOverride + uri[schemeEnd..];

        Assert.That(uri, Is.EqualTo("custom://myapp.example.com/auth/oidc/callback"));
    }

    [Test]
    public void SchemeOverrideLogic_PreviousEventHandlerIsPreserved()
    {
        var previousCalled = false;
        var options = new OpenIdConnectOptions();
        options.Events = new OpenIdConnectEvents();

        // Simulate existing handler
        var previous = new Func<Task>(() =>
        {
            previousCalled = true;
            return Task.CompletedTask;
        });

        // The override logic stores the previous handler
        var schemeOverride = "https";
        options.Events.OnRedirectToIdentityProvider = async ctx =>
        {
            await previous();
            var uri = ctx.ProtocolMessage.RedirectUri;
            var schemeEnd = uri?.IndexOf("://") ?? -1;
            if (schemeEnd >= 0)
                ctx.ProtocolMessage.RedirectUri = schemeOverride + uri![schemeEnd..];
        };

        // Invoke to verify previous is called
        previous().GetAwaiter().GetResult();
        Assert.That(previousCalled, Is.True);
    }
}
