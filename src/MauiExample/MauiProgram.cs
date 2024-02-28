using MauiExample.Contracts.Services;
using MXtend;
using System.Reflection;

namespace MauiExample;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MXBuilder.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Register dependencies from following assemblies: MauiExample, MauiExample.Core
        var appAssembly = Assembly.GetExecutingAssembly();
        MXBuilder.RegisterDependencies(builder, appAssembly);

        var coreAssembly = Assembly.Load("MauiExample.Core");
        MXBuilder.RegisterDependencies(builder, coreAssembly);

        return builder.Build();
    }
}
