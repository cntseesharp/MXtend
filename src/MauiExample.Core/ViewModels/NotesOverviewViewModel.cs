using CommunityToolkit.Mvvm.Input;
using MauiExample.Contracts.Services;
using MauiExample.Data.Notes;
using MXtend.Contracts.Services;
using MXtend.ViewModels;
using System.Collections.ObjectModel;

namespace MauiExample.Core.ViewModels
{
    public partial class NotesOverviewViewModel : MXViewModel
    {
        private ObservableCollection<NoteModel> _notes = [];
        public ObservableCollection<NoteModel> Notes => _notes;

        private readonly INoteRepository _noteRepository;

        public NotesOverviewViewModel(INavigationService navigationService, INoteRepository noteRepository) : base(navigationService)
        {
            _noteRepository = noteRepository;
        }

        public override async Task OnLoad()
        {
            var all = await _noteRepository.ReadAllNotes();
            foreach (var note in all)
                Notes.Add(note);
        }

        /*
        
        I'd like to express my eternal disgust to the quality of Microsoft products.
        This has to be the single most stupid mistake I've seen in the past 10 years.
        How dare you to mess up so much, that on creating ViewCell its BindingContext is THE viewmodel.
        Not even null, simply - THE BINDING CONTEXT OF THE PAGE.
        I'm utterly flabbergasted that I had to waste 30 minutes of my life on a bug that shouldn't even be possible.
        
        Command="{Binding BindingContext.DeleteNoteCommand, Source={RelativeSource AncestorType={x:Type vm:NotesOverviewViewModel}}}" CommandParameter="{Binding .}"
        ^^^^
        This binding completely crashes element creation, because ListView on initializing a ViewCell sets its BindingContext from the parent, instead of ItemsSource element.
        Why do they work at all? Cause then this BindingContext is changed to the correct one. But on initializing it shits all over the place...
        This is why you all see binding errors, even though everything works.

        */

        [RelayCommand]
        private async Task AddNote()
        {
            // Yes, class is a reference type and I could just keep it in local variable to save later.
            // However, in this case there is no way to cancel creation, so I have to use the result.
            var result = await _navigationService.NavigateWithResultAsync<NoteDetailViewModel>(new NoteModel { Id = Guid.NewGuid() });
            if (result is not NoteModel newNote)
                return;

            _notes.Add(newNote);
            await _noteRepository.CreateNote(newNote);
        }

        [RelayCommand]
        private async Task DeleteNote(object? obj)
        {
            if (obj is not NoteModel selectedNote)
                return;

            Notes.Remove(selectedNote);
            await _noteRepository.DeleteNote(selectedNote);
        }

        [RelayCommand]
        private async Task OpenNoteDetails(object? obj)
        {
            if (obj is not NoteModel selectedNote)
                return;

            // Copy the model to prevent changes in the UI.
            var update = await _navigationService.NavigateWithResultAsync<NoteDetailViewModel>(selectedNote.Copy());
            if (update is not NoteModel updateNote)
                return;

            selectedNote.From(updateNote);
            await _noteRepository.UpdateNote(selectedNote);
        }
    }
}
