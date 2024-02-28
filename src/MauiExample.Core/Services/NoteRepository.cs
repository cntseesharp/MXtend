using MauiExample.Contracts.Services;
using MauiExample.Data.Notes;
using MXtend.Attributes;
using Newtonsoft.Json;

namespace MauiExample.Core.Services
{
    // Yes, this isn't how you're supposed to do it.
    // This is purely for demonstrational purposes.
    // Do NOT use it as a guidance.
    [RegisterDependency(Type = DependencyType.Singleton)]
    public class NoteRepository : INoteRepository
    {
        private class NoteComparer : IEqualityComparer<NoteModel>
        {
            public bool Equals(NoteModel x, NoteModel y)
            {
                return x.Id == y.Id;
            }

            public int GetHashCode(NoteModel obj)
            {
                return obj.Id.GetHashCode();
            }
        }

        private HashSet<NoteModel> _notesCache = null;
        private const string StorageKey = "notesCache";

        public NoteRepository()
        {
            CacheCheck();
        }

        public async Task<bool> CreateNote(NoteModel model)
        {
            _notesCache.Add(model);

            await CacheSave();
            return true;
        }

        public async Task<IEnumerable<NoteModel>> ReadAllNotes()
        {
            return _notesCache.ToList();
        }

        public async Task<bool> UpdateNote(NoteModel model)
        {
            _notesCache.Remove(model);
            _notesCache.Add(model);

            await CacheSave();
            return true;
        }

        public async Task<bool> DeleteNote(NoteModel model)
        {
            _notesCache.Remove(model);

            await CacheSave();
            return true;
        }

        private async Task CacheCheck()
        {
            if (_notesCache != null)
                return;

            _notesCache = new();

            var jsonString = await SecureStorage.Default.GetAsync(StorageKey);
            if (jsonString == null)
                return;

            var cache = JsonConvert.DeserializeObject<List<NoteModel>>(jsonString);
            if (cache == null)
                return;

            cache.ForEach(x => _notesCache.Add(x));
        }

        private async Task CacheSave()
        {
            await SecureStorage.Default.SetAsync(StorageKey, JsonConvert.SerializeObject(_notesCache));
        }
    }
}
