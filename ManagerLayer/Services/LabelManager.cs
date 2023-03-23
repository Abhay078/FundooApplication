using CommonLayer.Model;
using ManagerLayer.Interface;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using RepositoryLayer.Services;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;

namespace ManagerLayer.Services
{
    public class LabelManager:ILabelManager
    {
        private readonly ILabelRepository repository;
        public LabelManager(ILabelRepository repository)
        {
            this.repository = repository;
        }
        public LabelEntity AddLabel(long UserId,LabelModel model)
        {
            return repository.AddLabel(UserId,model);
        }
        public IEnumerable<LabelEntity> GetAll(long UserId)
        {
            return repository.GetAll(UserId);
        }
        public LabelEntity UpdateLabel(long NoteId, string LabelName, string UpdatedLabelName)
        {
            return repository.UpdateLabel(NoteId,LabelName,UpdatedLabelName);
        }
        public bool DeleteLabel( string LabelName)
        {
            return repository.DeleteLabel( LabelName);
        }
        public IEnumerable<LabelEntity> GetLabelByNoteId(long NoteId)
        {
            return repository.GetLabelByNoteId(NoteId);
        }
    }
}
