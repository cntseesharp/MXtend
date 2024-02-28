using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MXtend.Attributes;
using MXtend.Contracts.Services;
using MXtend.ViewModels;

namespace MauiExample.Core.ViewModels
{
    public partial class MainViewModel : MXViewModel
    {
        [ObservableProperty]
        private string _myText;

        public MainViewModel(INavigationService navigationService) : base(navigationService)
        {
            MyText = "Hello, world!";
        }

        [RelayCommand]
        private async Task ChangeText()
        {
            MyText = "The value of the Label should update.";
        }

        [RelayCommand]
        private async Task NavigateToSecondPage()
        {
            await _navigationService.NavigateAsync<NotesOverviewViewModel>();
        }

        [RelayCommand]
        private async Task NavigateToSecondPageModal()
        {
            await _navigationService.NavigateModalAsync<NotesOverviewViewModel>();
        }
    }
}
