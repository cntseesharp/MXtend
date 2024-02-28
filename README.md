# MXtend
Jumpstart your MAUI development with my MVVMCross-inspired library. Designed to streamline your app development process, this lib embraces the principles of MVVM (Model-View-ViewModel) architecture, empowering you to create robust and maintainable cross-platform applications with ease, while removing the annoying parts, like route registration and mess of dependencies registration.

[![NuGet Downloads](https://img.shields.io/nuget/v/MXtend.svg?style=flat-square)](https://www.nuget.org/packages/MXtend)


#### What is included?
- Example of platform-specific Depencdency Injection;
- Navigation service that takes away the burden of route registrations;
- Two-way data transfer on navigation;
- An easy way to register and resolve dependencies;
- CommunityToolkit, to reduce boilerplate;
- Lifecycle management;
- Example for everything, mentioned above;

# 💡 How to use
Modify your `MauiProgram` class. Instead of `MauiApp` use `MXBuilder`. Register your dependency on assembly-basis, using MXBuilder.RegisterDependencies
```csharp
        var builder = MXBuilder.CreateBuilder();

        // Register dependencies from following assemblies: MauiExample, MauiExample.Core
        var appAssembly = Assembly.GetExecutingAssembly();
        MXBuilder.RegisterDependencies(builder, appAssembly);

        var coreAssembly = Assembly.Load("MauiExample.Core");
        MXBuilder.RegisterDependencies(builder, coreAssembly);
```

Modify `App.cs`. Change `Application` to `MXApp`.
Example:
```csharp
public partial class App : MXApp
{
    public App(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        InitializeComponent();

        MainPage = new NavigationPage();
        MainPage.ToolbarItems.Add(new ToolbarItem { Text = "Title Text" });
        MainPage.MenuBarItems.Add(new MenuBarItem { Text = "Menu text" });

        serviceProvider.GetService<INavigationService>().NavigateAsync<MainViewModel>();
    }
}
```

Modify `App.xaml` to inherit `MXApp`.
```xml
<mx:MXApp xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:MauiExample"
             xmlns:mx="clr-namespace:MXtend;assembly=MXtend"
             x:Class="MauiExample.App">
    <!-- Resources, etc. -->
</mx:MXApp>
```

Create a ViewModel
```csharp
public class MyViewModel : MXViewModel
{
    // ViewModel implementation...
}
```

Declare a page
```csharp
public class MyPage : MXPage<MyViewModel>
{
    // Page implementation...
}
```
```xml
<mx:MXPage
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:mx="clr-namespace:MXtend.Pages;assembly=MXtend"
    xmlns:viewModels="clr-namespace:MauiExample.Core.ViewModels;assembly=MauiExample.Core"
    x:DataType="viewModels:MyViewModel"
    x:TypeArguments="viewModels:MyViewModel"
    x:Class="MauiExample.Pages.MyPage">
    
    <!-- Your page content goes here -->

</mx:MXPage>
```

# 🔹 Details

### Automated Dependency Injection and registration
For this project I went with a simple Interface-Implementation approach to the services. 
User `RegisterDependency` attribute for the class to be automatically registered in DI container.
```csharp
    [RegisterDependency(Type = DependencyType.Singleton)]
    public class MyService : IMyService { }
```
You can also use this property to registed any dependency.
In case your service inherits multiple interfaces you can use Interface property to specify which interface to use to resolve an instance for dependency injection.
```csharp
    [RegisterDependency(Interface = typeof(IMyService), Type = DependencyType.Singleton)]
    public class MyService : IMyService, ISecondService
```
### ❗ All ViewModels and Pages that inherit MXPage and MXViewModel are automatically  registered ❗

## MXPage\<TViewModel\>
The MXPage\<TViewModel\> class serves as a base class for your MAUI views, providing essential functionality for integrating view models and managing lifecycle events. Designed with simplicity and extensibility in mind, this class streamlines the development of cross-platform applications using the Model-View-ViewModel (MVVM) architectural pattern.

Ensure that your view models implement the IViewModel interface to maintain compatibility with the `MXPage<TViewModel>` class.

#### XAML Declaration
It's important to add x:DataType to the XAML declaration. Otherwise code generator will not resolve MXPage generic argument.
```xml
<mx:MXPage
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:mx="clr-namespace:MXtend.Pages;assembly=MXtend"
    xmlns:viewModels="clr-namespace:MauiExample.Core.ViewModels;assembly=MauiExample.Core"
    x:DataType="viewModels:MyViewModel"
    x:TypeArguments="viewModels:MyViewModel"
    x:Class="MauiExample.Pages.MyPage">
    
    <!-- Your page content goes here -->

</mx:MXPage>
```
#### Code behind
```csharp
public partial class MyPage : MXPage<MyViewModel>
{
    public MyPage() => InitializeComponent();
}
```

## Navigation Service
The `NavigationService` class provides a convenient way to navigate between views in your MAUI application. It utilizes a dependency injection approach and automates the resolution of ViewModel-Model relations for seamless navigation.

#### Usage:
Navigate from your ViewModel:
```csharp
await navigationService.NavigateAsync<MyViewModel>();
```
Or close current page:
```csharp
await navigationService.Close(this);
```

#### Passing parameters between pages
##### Pass model from `A` to `B`.
```csharp
await _navigationService.NavigateAsync<B>(parameter);
// or
var result = await _navigationService.NavigateWithResultAsync<B>(parameter);
```
To accept the parameter in `B` you must override Prepare method. 
If parameter is null
```csharp
public override async Task Prepare(object parameter)
{
    if (parameter is not MyModel myModel)
        throw new ArgumentException();

    Data = myModel;
}
```

##### Passing data from B to A.
```csharp
// In A navigate using NavigateWithResultAsync method
var result = await _navigationService.NavigateWithResultAsync<B>();
if (result is not MyModel myModel)
    return;

// In B to return data pass it in Close method
await _navigationService.CloseAsync(this, myModel);
```