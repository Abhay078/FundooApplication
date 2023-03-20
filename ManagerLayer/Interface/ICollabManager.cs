using CommonLayer.Model;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLayer.Interface
{
    public interface ICollabManager
    {
        public CollabEntity AddCollab(AddCollabModel model, long UserId);
        public CollabEntity DeleteCollab(long UserId, string email, long NoteId);
        public IEnumerable<CollabEntity> GetAllCollab(long UserId);
    }
}
