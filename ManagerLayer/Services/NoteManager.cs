using CommonLayer.Model;
using ManagerLayer.Interface;
using Microsoft.AspNetCore.Http;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using RepositoryLayer.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLayer.Services
{
    public class NoteManager:INoteManager
    {
        private readonly INotesRepository notesRepository;
        public NoteManager(INotesRepository notesRepository)
        {
            this.notesRepository = notesRepository;
        }
        public NotesEntity CreateNote(CreateNoteModel model,long UserId)
        {
            return notesRepository.CreateNote(model,UserId);
        }
        public IEnumerable<NotesEntity> GetAllNote(long UserId)
        {
            return notesRepository.GetAllNote(UserId);
        }
        public NotesEntity GetNoteById(long id,long UserId)
        {
            return notesRepository.GetNoteById(id,UserId);
        }
        public NotesEntity UpdateNote(UpdateNoteModel model, long id,long UserId)
        {
            return notesRepository.UpdateNote(model, id,UserId);
        }
        public bool DeleteNote(long id,long UserId)
        {
            return notesRepository.DeleteNote(id,UserId);
        }
        public NotesEntity ArchieveNote(long id, long UserId)
        {
            return notesRepository.ArchieveNote(id,UserId);
        }
        public NotesEntity TrashNote(long id, long UserId)
        {
            return notesRepository.TrashNote(id,UserId);
        }
        public NotesEntity PinNote(long id, long UserId)
        {
            return notesRepository.PinNote(id,UserId);
        }
        public NotesEntity ColorNote(long Noteid, long UserId, string Color)
        {
            return notesRepository.ColorNote(Noteid,UserId,Color);
        }
        public NotesEntity Image(IFormFile image, long Noteid, long UserId)
        {
            return notesRepository.Image(image, Noteid,UserId);
        }
        public IEnumerable<NotesEntity> RetrieveMatching(string Keyword,long UserId,out int count)
        {
            return notesRepository.RetrieveMatching(Keyword,UserId,out count);
        }
        public List<NotesEntity> Pagination(string Keyword, int pageNo, int pageSize,long UserId)
        {
            return notesRepository.Pagination(Keyword, pageNo, pageSize,UserId);
        }
    }
}
