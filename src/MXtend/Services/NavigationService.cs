using MXtend.Attributes;
using MXtend.Contracts.Services;
using MXtend.Contracts.ViewModels;
using MXtend.Pages;
using MXtend.ViewModels;

namespace MXtend.Services
{
    [RegisterDependency(Interface = typeof(INavigationService), Type = DependencyType.Singleton)]
    public class NavigationService : INavigationService
    {
        private readonly Dictionary<Type, Type> _navigationMatching = new();
        private readonly IServiceProvider _serviceProvider;

        private INavigation Navigation => MXApp.Current.MainPage is NavigationPage ? MXApp.Current.MainPage.Navigation : Shell.Current.Navigation;

        private Dictionary<IViewModel, TaskCompletionSource<object?>> _awaitingResults = new Dictionary<IViewModel, TaskCompletionSource<object?>>();

        public NavigationService()
        {
            _serviceProvider = DependencyService.Resolve<IServiceProvider>();
            RegisterRoutes();
        }

        public async Task NavigateAsync<T>(object? navigationParameter = null, bool animated = true) where T : IViewModel
        {
            if (Navigation.ModalStack.Count > 0)
            {
                await NavigateModalAsync<T>(navigationParameter, animated);
                return;
            }

            var page = await CreatePage<T>(navigationParameter);
            await Navigation.PushAsync(page);
        }

        public async Task NavigateModalAsync<T>(object? navigationParameter = null, bool animated = true) where T : IViewModel
        {
            var page = await CreatePage<T>(navigationParameter, true);
            await Navigation.PushModalAsync(page);
        }

        public Task<object?> NavigateWithResultAsync<T>(object? navigationParameter = null, bool animated = true) where T : IViewModel
        {
            if (Navigation.ModalStack.Count > 0)
                return NavigateModalWithResultAsync<T>(navigationParameter, animated);

            var page = CreatePage<T>(navigationParameter).Result; // TODO: Hmmm...
            Navigation.PushAsync(page).ConfigureAwait(false);

            return CreateTask(page.ViewModel);
        }

        public Task<object?> NavigateModalWithResultAsync<T>(object? navigationParameter = null, bool animated = true) where T : IViewModel
        {
            var page = CreatePage<T>(navigationParameter, true).Result;
            Navigation.PushModalAsync(page).ConfigureAwait(false);

            return CreateTask(page.ViewModel);
        }

        public async Task CloseAsync(IViewModel vm, object? result = null)
        {
            if (Navigation.ModalStack.Count > 0)
            {
                await CloseModalAsync(vm, result);
                return;
            }

            await vm.OnUnload();
            await Navigation.PopAsync();
            ReturnResultIfNeeded(vm, result);
        }

        public async Task CloseModalAsync(IViewModel vm, object? result = null)
        {
            await vm.OnUnload();
            await Navigation.PopModalAsync();
            ReturnResultIfNeeded(vm, result);
        }

        private Task<object?> CreateTask(IViewModel viewModel)
        {
            var task = new TaskCompletionSource<object?>();
            _awaitingResults.Add(viewModel, task);
            return task.Task;
        }

        private void ReturnResultIfNeeded(IViewModel vm, object? result = null)
        {
            if (_awaitingResults.ContainsKey(vm))
            {
                _awaitingResults[vm].SetResult(result);
                _awaitingResults.Remove(vm);
            }
        }

        private async Task<MXPage<T>> CreatePage<T>(object? navigationParameter, bool setModalFlag = false) where T : IViewModel
        {
            if (!_navigationMatching.ContainsKey(typeof(T)))
                throw new ArgumentException("ViewModel wasn't registered in the NavigationService. Check PageBase<> generic and presence of RegisterDependency attribute.");

            var page = _serviceProvider.GetService(_navigationMatching[typeof(T)]) as MXPage<T>;
            if (page == null)
                throw new NullReferenceException("Page wasn't resolved during NavigationService instantiation and cannot be created. Check the page declaration");

            if(navigationParameter != null)
                await page.ViewModel.Prepare(navigationParameter);

            await page.ViewModel.OnLoad();

            page.ViewModel.IsModal = setModalFlag;
            return page;
        }

        // Automatically resolve ViewModel-Model relations.
        // TODO: Improve code quality. Class rework (?)
        private void RegisterRoutes()
        {
            var allTypes = MXBuilder.KnownAssemblies.SelectMany(x => x.GetTypes());
            var typeBase = typeof(MXPage<MXViewModel>);

            foreach (var type in allTypes.Where(type => !type.IsAbstract && type.IsClass))
            {
                if (type.BaseType == null || type.BaseType.Name != typeBase.Name) // Yeah, I wish for a better method
                    continue;

                var viewModelType = type.BaseType.GenericTypeArguments[0];
                _navigationMatching.Add(viewModelType, type);
            }
        }
    }
}
