using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    public interface ILabelRepository
    {
        public LabelEntity AddLabel(long UserId, long NoteId, string LabelName);
        public IEnumerable<LabelEntity> GetAll(long UserId);
        public LabelEntity UpdateLabel(long NoteId, string LabelName, string UpdatedLabelName);
        public bool DeleteLabel( string LabelName);
        public IEnumerable<LabelEntity> GetLabelByNoteId(long NoteId);
    }
}
