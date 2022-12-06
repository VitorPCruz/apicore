using DevIO.Api.Configuration;
using DevIO.API.Configuration;
using DevIO.Data.Context;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

#region Builder
var builder = WebApplication.CreateBuilder(args);

var connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<MeuDbContext>(
    options => options.UseSqlServer(connection));

builder.Services.AddIdentityConfiguration(builder.Configuration);

builder.Services.AddControllers()
    .AddJsonOptions(options => 
        options.JsonSerializerOptions
            .ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.WebApiConfig();

builder.Services.AddSwaggerConfig();

builder.Services.ResolveDependencies();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddEndpointsApiExplorer();

//builder.Services.AddSwaggerGen(config =>
//{
//    config.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
//    {
//        Title = "DevIO API",
//        Version = "v1"
//    });
//});

#endregion

#region App
var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseCors("Development");


    app.UseDeveloperExceptionPage();
}
else
{
    //app.UseCors("Production");

}

//app.UseSwagger();
//app.UseSwaggerUI(config =>
//{
//    config.SwaggerEndpoint("/swagger/v1/swagger.json", "DevIO API v1");
//});
app.UseMvcConfiguration();

var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
app.UseSwaggerConfig(apiVersionDescriptionProvider);

app.UseHsts();
app.MapControllers();
app.Run();
#endregion