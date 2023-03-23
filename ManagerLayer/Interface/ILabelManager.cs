using CommonLayer.Model;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLayer.Interface
{
    public interface ILabelManager
    {
        public LabelEntity AddLabel(long UserId,LabelModel model);
        public IEnumerable<LabelEntity> GetAll(long UserId);
        public LabelEntity UpdateLabel( long NoteId, string LabelName, string UpdatedLabelName);
        public bool DeleteLabel(string LabelName);
        public IEnumerable<LabelEntity> GetLabelByNoteId(long NoteId);
    }
}
