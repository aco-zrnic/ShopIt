using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Server.Behavior;
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
     });

// Add services to the container.
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddOptions<AwsS3Options>()
    .BindConfiguration(AwsS3Options.ConfigSection)
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
