using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Serilog;
using Server.Behavior;
using Server.Entities;
using Server.Modules;
using Server.Options;
using Server.Util.Auth0;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection.PortableExecutable;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, configuration) =>
{
        configuration.ReadFrom
        .Configuration(context.Configuration)
        .WriteTo.Console()
        .Enrich.FromLogContext()
        .Enrich.WithMachineName()
        .Enrich.WithEnvironmentName();
});
builder.Host
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
     .ConfigureContainer<ContainerBuilder>(builder =>
     {
         var configuration = MediatRConfigurationBuilder
           .Create(typeof(Program).Assembly)
           .WithAllOpenGenericHandlerTypesRegistered()
           .Build();

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
builder.Services.AddAuthServiceCollection(builder.Configuration);
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo() { Description = "Shop It Books" });
    string securityDefinitionName = builder.Configuration.GetValue<string>("SwaggerUISecurityMode");
  
    var securityScheme = new OpenApiOAuthSecurityScheme(builder.Configuration.GetValue<string>("Auth0:Domain"), builder.Configuration.GetValue<string>("Auth0:Audience"));
    var securityRequirement = new OpenApiOAuthSecurityRequirement();


    c.AddSecurityDefinition(securityDefinitionName, securityScheme);
    c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
    c.AddSecurityRequirement(securityRequirement);
});
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shop It Books V1");
        c.DocExpansion(DocExpansion.None);
       
        c.OAuthClientId(builder.Configuration["Auth0:ClientId"]);
        c.OAuthClientSecret(builder.Configuration["Auth0:ClientSecret"]);
        c.OAuthAppName("Shop It Books");
        c.OAuthAdditionalQueryStringParams(new Dictionary<string, string> { { "audience", builder.Configuration["Auth0:Audience"] } });
        c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
        
    });
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
