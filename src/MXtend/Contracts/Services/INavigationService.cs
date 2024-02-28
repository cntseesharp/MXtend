using MXtend.Contracts.ViewModels;

namespace MXtend.Contracts.Services
{
    public interface INavigationService
    {
        Task NavigateAsync<T>(object? navigationParameter = null, bool animated = true) where T : IViewModel;
        Task NavigateModalAsync<T>(object? navigationParameter = null, bool animated = true) where T : IViewModel;

        Task<object?> NavigateWithResultAsync<T>(object? navigationParameter = null, bool animated = true) where T : IViewModel;
        Task<object?> NavigateModalWithResultAsync<T>(object? navigationParameter = null, bool animated = true) where T : IViewModel;

        Task CloseAsync(IViewModel vm, object? result = null);
        Task CloseModalAsync(IViewModel vm, object? result = null);
    }
}
