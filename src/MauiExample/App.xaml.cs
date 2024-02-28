using MauiExample.Core.ViewModels;
using MXtend;
using MXtend.Contracts.Services;

namespace MauiExample;

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
