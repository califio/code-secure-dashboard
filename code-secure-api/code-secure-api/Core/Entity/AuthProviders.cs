using Aguacongas.AspNetCore.Authentication.EntityFramework;

namespace CodeSecure.Core.Entity;

public class AuthProviders: SchemeDefinition
{
    public bool Enable { get; set; }
}