namespace TodoSphere.Users.API.Extensions;

public static class Extension
{
    public static void UserSwaggerExtension(this IApplicationBuilder builder)
    {
        builder.UseSwagger();
        builder.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo Sphere User service API"); });
    }
}