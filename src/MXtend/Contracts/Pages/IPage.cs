using MXtend.Contracts.ViewModels;

namespace MXtend.Contracts.Pages
{
    public interface IPage
    {
        IViewModel ViewModel { get; }
    }
}
