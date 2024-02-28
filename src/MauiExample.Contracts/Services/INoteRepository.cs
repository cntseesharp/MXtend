using MauiExample.Data.Notes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiExample.Contracts.Services
{
    public interface INoteRepository
    {
        Task<bool> CreateNote(NoteModel model);
        Task<IEnumerable<NoteModel>> ReadAllNotes();
        Task<bool> UpdateNote(NoteModel model);
        Task<bool> DeleteNote(NoteModel model);
    }
}
