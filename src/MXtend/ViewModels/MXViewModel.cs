using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MXtend.Attributes;
using MXtend.Contracts.Services;
using MXtend.Contracts.ViewModels;

namespace MXtend.ViewModels
{
    [RegisterDependency]
    public partial class MXViewModel : ObservableObject, IViewModel
    {
        protected INavigationService _navigationService;

        public bool IsModal { get; set; }

        public MXViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public virtual async Task OnLoad() { }

        public virtual async Task OnUnload() { }

        public virtual void OnAppearing() { }

        public virtual void OnDisappearing() { }

        public virtual async Task Prepare(object parameter) { }

        [RelayCommand]
        private async Task NavigateBack()
        {
            if (IsModal)
                await _navigationService.CloseModalAsync(this);

            if (!IsModal)
                await _navigationService.CloseAsync(this);
        }
    }
}
