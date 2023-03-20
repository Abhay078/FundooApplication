using CommonLayer.Model;
using Microsoft.AspNetCore.Http;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    public interface INotesRepository
    {
        public NotesEntity CreateNote(CreateNoteModel model,long UserId);
        public IEnumerable<NotesEntity> GetAllNote(long userId);

        public NotesEntity GetNoteById(long id,long UserId);
        public NotesEntity UpdateNote(UpdateNoteModel model, long id, long UserId);
        public bool DeleteNote(long id, long UserId);
        public NotesEntity ArchieveNote(long id, long UserId);
        public NotesEntity TrashNote(long id, long UserId);
        public NotesEntity PinNote(long id, long UserId);

        public NotesEntity ColorNote(long Noteid, long UserId, string Color);
        public NotesEntity Image(IFormFile Image, long Noteid, long UserId);
        public IEnumerable<NotesEntity> RetrieveMatching(string Keyword,long UserId,out int count);
        public List<NotesEntity> Pagination(string Keyword, int pageNo, int pageSize,long UserId);
    }
}
