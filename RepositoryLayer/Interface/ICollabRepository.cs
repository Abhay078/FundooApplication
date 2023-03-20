using CommonLayer.Model;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    public interface ICollabRepository
    {
        public CollabEntity AddCollab(AddCollabModel model,long UserId);
        public CollabEntity DeleteCollab(long UserId, string email, long NoteId);
        public IEnumerable<CollabEntity> GetAllCollab(long UserId);
        
    }
}
