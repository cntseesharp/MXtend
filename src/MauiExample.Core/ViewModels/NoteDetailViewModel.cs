using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiExample.Data.Notes;
using MXtend.Contracts.Services;
using MXtend.ViewModels;

namespace MauiExample.Core.ViewModels
{
    public partial class NoteDetailViewModel : MXViewModel
    {
        [ObservableProperty]
        private NoteModel? _currentNote = null;

        public NoteDetailViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        public override async Task Prepare(object parameter)
        {
            if (parameter is not NoteModel note)
            {
                await _navigationService.CloseAsync(this);
                return;
            }

            CurrentNote = note;
        }

        [RelayCommand]
        private async Task SaveChanges()
        {
            await _navigationService.CloseAsync(this, _currentNote);
        }
    }
}
