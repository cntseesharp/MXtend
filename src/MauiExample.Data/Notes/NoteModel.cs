using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;

namespace MauiExample.Data.Notes
{
    public partial class NoteModel : ObservableObject
    {
        [ObservableProperty]
        private Guid _id;
        [ObservableProperty]
        private string? _title;
        [ObservableProperty]
        private string? _content;

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Title)}: {Title}, {nameof(Content)}: {Content}";
        }

        public NoteModel Copy()
        {
            var model = new NoteModel();
            model.Id = Id;
            model.Title = Title;
            model.Content = Content;
            return model;
        }

        public void From(NoteModel noteModel)
        {
            this.Id = noteModel.Id;
            this.Title = noteModel.Title;
            this.Content = noteModel.Content;
        }
    }
}
