using CodeSecure.Core;

namespace CodeSecure.Application.Module.User;

public class UserModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddScoped<IFindUserByIdHandler, FindUserByIdHandler>();
        builder.AddScoped<ICreateUserHandler, CreateUserHandler>();
        builder.AddScoped<IUpdateUserHandler, UpdateUserHandler>();
        builder.AddScoped<IFindUserByIdHandler, FindUserByIdHandler>();
        builder.AddScoped<IQueryUserInfoHandler, QueryUserInfoHandler>();
        builder.AddScoped<IQueryUserSummaryHandler, QueryUserSummaryHandler>();
        builder.AddScoped<IListProjectManagerUserHandler, ListProjectManagerUserHandler>();
        builder.AddScoped<ISendConfirmEmailHandler, SendConfirmEmailHandler>();
        return builder;
    }
}