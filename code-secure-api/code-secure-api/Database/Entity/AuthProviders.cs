using Aguacongas.AspNetCore.Authentication.EntityFramework;

namespace CodeSecure.Database.Entity;

public class AuthProviders: SchemeDefinition
{
    public bool Enable { get; set; }
}