using MXtend.Attributes;
using MXtend.Contracts.Pages;
using MXtend.Contracts.ViewModels;

namespace MXtend.Pages
{
    [RegisterDependency]
    public class MXPage<TViewModel> : ContentPage, IPage
        where TViewModel : IViewModel
    {
        private readonly TViewModel _viewModel;
        public IViewModel ViewModel => _viewModel;

        public MXPage()
        {
            var serviceProvider = DependencyService.Resolve<IServiceProvider>();
            _viewModel = (TViewModel)serviceProvider.GetService(typeof(TViewModel)); // or any DI container's resolve method
            BindingContext = ViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            ViewModel.OnDisappearing();
        }
    }
}
