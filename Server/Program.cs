using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Keycloak.AuthServices.Sdk.Admin;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using Server.Behavior;
using Server.Entities;
using Server.Modules;
using Server.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Host
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
     .ConfigureContainer<ContainerBuilder>(builder =>
     {
         var configuration = MediatRConfigurationBuilder
           .Create(typeof(Program).Assembly)
           .WithAllOpenGenericHandlerTypesRegistered()
           .Build();

         // this will add all your Request- and Notificationhandler
         // that are located in the same project as your program-class

         builder.RegisterMediatR(configuration);

         
         builder.RegisterModule<BehaviorModule>();
         builder.RegisterModule<ShopItModule>();
         builder.RegisterType<ShopItContext>();
     });

// Add services to the container.
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddDbContext<ShopItContext>(
    contextLifetime: ServiceLifetime.Transient,
optionsAction: options =>
        options
            .UseNpgsql(builder.Configuration.GetConnectionString("Database"))
            .UseSnakeCaseNamingConvention()
);
builder.Services.AddOptions<AwsS3Options>()
    .BindConfiguration(AwsS3Options.ConfigSection)
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddKeycloakAuthentication(builder.Configuration);

builder.Services.AddAuthorization().AddKeycloakAuthorization(builder.Configuration);
builder.Services.AddKeycloakAdminHttpClient(builder.Configuration, httpClient =>
{
    httpClient.BaseAddress = new Uri("http://localhost:8080/auth");

    // using Microsoft.Net.Http.Headers;
    httpClient.DefaultRequestHeaders.Add(
        HeaderNames.Accept, "application/json");
    httpClient.DefaultRequestHeaders.TryAddWithoutValidation(
        HeaderNames.ContentType, "application/json");
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
