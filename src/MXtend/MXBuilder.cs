using MXtend.Attributes;
using System.Reflection;

namespace MXtend
{
    public static class MXBuilder
    {
        internal static HashSet<Assembly> KnownAssemblies { get; private set; } = new();

        public static MauiAppBuilder CreateBuilder()
        {
            var builder = MauiApp.CreateBuilder();
            var appAssembly = Assembly.GetExecutingAssembly();
            RegisterDependencies(builder, appAssembly);
            return builder;
        }

        public static void RegisterProvider(IServiceProvider serviceProvider)
        {
            DependencyService.RegisterSingleton(serviceProvider);
        }

        public static void RegisterDependencies(MauiAppBuilder builder, Assembly assembly)
        {
            KnownAssemblies.Add(assembly);

            var allTypes = assembly.GetTypes();

            foreach (var type in allTypes.Where(type => !type.IsAbstract && type.IsClass))
            {
                var registerAttribute = type.GetCustomAttribute(typeof(RegisterDependencyAttribute)) as RegisterDependencyAttribute;
                if (registerAttribute == null)
                    continue;

                if (registerAttribute.Type == DependencyType.Singleton)
                {
                    var interfaces = type.GetInterfaces();
                    var hasOnlyOneInterface = interfaces.Length == 1;

                    if (registerAttribute.Interface == null && !hasOnlyOneInterface)
                        throw new ArgumentException("RegisterDependency Interface attribute cannot be null if singleton has more than 1 interface or no interfaces at all.");

                    var iface = registerAttribute.Interface != null ? registerAttribute.Interface : interfaces[0];
                    builder.Services.AddSingleton(iface, type);
                }

                if (registerAttribute.Type == DependencyType.Transient)
                    builder.Services.AddTransient(type);
            }
        }
    }
}
