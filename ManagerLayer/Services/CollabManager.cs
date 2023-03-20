using CommonLayer.Model;
using ManagerLayer.Interface;
using RepositoryLayer.Entity;
using RepositoryLayer.FundooDBContext;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLayer.Services
{
    public class CollabManager:ICollabManager
    {
        private readonly ICollabRepository repository;
        public CollabManager(ICollabRepository repository)
        {
            this.repository=repository;
        }
        public CollabEntity AddCollab(AddCollabModel model,long UserId)
        {
            return repository.AddCollab(model,UserId);
        }
        public CollabEntity DeleteCollab(long UserId, string email, long NoteId)
        {
            return repository.DeleteCollab(UserId,email,NoteId);
     
        }
        public IEnumerable<CollabEntity> GetAllCollab(long UserId)
        {
            return repository.GetAllCollab(UserId);
        }
        
    }
}
