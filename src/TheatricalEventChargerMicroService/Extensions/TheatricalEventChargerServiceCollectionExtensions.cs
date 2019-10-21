using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TheatricalEventChargerApplication.Commands.Validators;
using TheatricalEventChargerDomain.Abstracts;
using TheatricalEventChargerDomain.BillCalculationStrategies;
using TheatricalEventChargerDomain.Entities;
using TheatricalEventChargerRepository.Abstracts;
using TheatricalEventChargerRepository.DocumentStore.Stores;

namespace TheatricalEventChargerMicroService.Extensions
{
    public static class TheatricalEventChargerServiceCollectionExtensions
    {

        public static IServiceCollection AddBillCalculationStrategies(this IServiceCollection services)
        {
            services.AddSingleton<IBillCalculationStrategyFactory, BillCalculationStrategyFactory>((provider) => {
                var billCalculationStrategies = new Dictionary<string, IBillCalculationStrategy>();

                billCalculationStrategies.Add("default", new DefaultBillCalculationStrategy());
                billCalculationStrategies.Add("tragedy", new TragedyBillCalculationStrategy());
                billCalculationStrategies.Add("comedy", new ComedyBillCalculationStrategy());

                return new BillCalculationStrategyFactory(billCalculationStrategies);
            });

            return services;
        }

        public static IServiceCollection AddRavenDBStores(this IServiceCollection services)
        {
            services.AddScoped<IRepository<string, CustomerCharge>, CustomerChargeDocumentStore>();
            services.AddScoped<IReadOnlyRepository<string, TheatricalPlayCatalogItem>, TheatricalPlayCatalogDocumentStore>();

            return services;
        }

        public static IServiceCollection AddMediator(this IServiceCollection services)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Module module in assembly.GetModules().Where(m => m.Name.Contains(nameof(TheatricalEventChargerApplication))))
                {
                    AssemblyScanner
                        .FindValidatorsInAssembly(assembly)
                        .ForEach(result => services.AddScoped(result.InterfaceType, result.ValidatorType));
                }
            }
            services.Add(new ServiceDescriptor(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>), ServiceLifetime.Scoped));

            var TheatricalEventChargerApplicationAssembly = AppDomain.CurrentDomain.Load(nameof(TheatricalEventChargerApplication));

            services.AddMediatR(TheatricalEventChargerApplicationAssembly);

            return services;
        }
    }
}
