using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Exam.Web.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Storage;

namespace Exam.Web.Infrastructure.IOC
{
    public static class ServiceCollectionExtensions
    {
        private static IServiceCollection Services { get; set; }
        private static List<Assembly> Assemblies { get; }
        private const string SOLUTION_NAME = "Exam";

        static ServiceCollectionExtensions()
        {
            var coreLoad = new Student();
            Assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(t => t.FullName.Contains(SOLUTION_NAME))
                .ToList();
        }

        public static void InjectMySqlDbContext<TContext>(this IServiceCollection collection, string connectionString, string assembly) where TContext : DbContext
        {
            collection.AddDbContext<TContext>(options =>
                options.UseMySql(connectionString,
                    b => b.MigrationsAssembly(assembly)
                        .ServerVersion(new ServerVersion(new Version(5, 7, 12)))
                        .CharSet(CharSet.Latin1)
                ));
            var serviceProvider = collection.BuildServiceProvider();
            var dbContext = serviceProvider.GetRequiredService<TContext>();
            dbContext.Database.Migrate();
        }

        public static void InjectForNamespace(this IServiceCollection collection, string nameSpace)
        {
            Services = collection;
            InjectDependenciesForAssembly(Assemblies.FirstOrDefault(t => nameSpace.Contains(t.GetName().Name)), nameSpace);
        }

        private static void InjectDependenciesForAssembly(Assembly assembly, string nameSpace)
        {
            if (assembly == null)
                return;

            var typesToRegister = assembly.GetTypes()
                .Where(x => x.Namespace != null && x.Namespace.Contains(nameSpace, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
            typesToRegister.ForEach(RegisterType);
        }

        private static void RegisterType(Type typeToRegister)
        {
            var registrationType = typeToRegister.GetCustomAttribute<RegistrationKindAttribute>();
            if (registrationType == null)
            {
                if (typeToRegister.GetInterfaces().Any())
                    Services.AddScoped(typeToRegister.GetInterfaces().First(), typeToRegister);
                return;
            }

            switch (registrationType.Type)
            {
                case RegistrationType.Singleton:
                    if (registrationType.AsSelf)
                        Services.AddSingleton(typeToRegister);
                    else
                        Services.AddSingleton(typeToRegister.GetInterfaces().First(), typeToRegister);
                    break;
                case RegistrationType.Scoped:
                    if (registrationType.AsSelf)
                        Services.AddScoped(typeToRegister);
                    else
                        Services.AddScoped(typeToRegister.GetInterfaces().First(), typeToRegister);
                    break;
                case RegistrationType.Transient:
                    if (registrationType.AsSelf)
                        Services.AddTransient(typeToRegister);
                    else
                        Services.AddTransient(typeToRegister.GetInterfaces().First(), typeToRegister);
                    break;
                default:
                    Services.AddScoped(typeToRegister.GetInterfaces().First(), typeToRegister);
                    break;
            }
        }
    }
}