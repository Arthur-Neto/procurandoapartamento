// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ProcurandoApartamento.Configuration;
using ProcurandoApartamento.Infrastructure.Configuration;
using ProcurandoApartamento.Infrastructure.Data;
using System;

[assembly: ApiController]

namespace ProcurandoApartamento
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        private IConfiguration Configuration { get; }

        public IHostEnvironment Environment { get; }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services
                .AddAppSettingsModule(Configuration);

            AddDatabase(services);

            services
                .AddSecurityModule()
                .AddProblemDetailsModule(Environment)
                .AddAutoMapperModule()
                .AddSwaggerModule()
                .AddWebModule()
                .AddRepositoryModule()
                .AddServiceModule();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app, IHostEnvironment env, IServiceProvider serviceProvider,
            ApplicationDatabaseContext context, IOptions<SecuritySettings> securitySettingsOptions)
        {
            var securitySettings = securitySettingsOptions.Value;
            app
                .UseApplicationSecurity(securitySettings)
                .UseApplicationProblemDetails()
                .UseApplicationSwagger()
                .UseApplicationWeb(env)
                .UseApplicationDatabase(serviceProvider, env);
        }

        protected virtual void AddDatabase(IServiceCollection services)
        {
            services.AddDatabaseModule(Configuration);
        }
    }
}
