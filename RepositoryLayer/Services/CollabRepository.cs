using CommonLayer.Model;
using RepositoryLayer.Entity;
using RepositoryLayer.FundooDBContext;
using RepositoryLayer.Interface;
using System.Collections.Generic;
using System.Linq;

namespace RepositoryLayer.Services
{
    public class CollabRepository : ICollabRepository
    {
        private readonly FunContext context;
        public CollabRepository(FunContext context)
        {
            this.context = context;
        }
        public CollabEntity AddCollab(AddCollabModel model, long UserId)
        {
            try
            {
                var Check = context.Notes.FirstOrDefault(o => o.NoteId == model.NoteId);
                if (Check != null)
                {
                    CollabEntity entity = new CollabEntity();
                    entity.Email = model.Email;
                    entity.NoteId = model.NoteId;
                    entity.UserId = UserId;
                    context.Collab.Add(entity);
                    context.SaveChanges();
                    return entity;

                }

                return null;

            }
            catch (System.Exception)
            {

                throw;
            }



        }
        public CollabEntity DeleteCollab(long UserId, string email, long NoteId)
        {
            try
            {
                var CheckNote = context.Collab.FirstOrDefault(o => o.NoteId == NoteId);
                var Check = context.Collab.FirstOrDefault(o => o.Email.Equals(email));
                if (Check != null && CheckNote != null)
                {
                    context.Collab.Remove(Check);
                    context.SaveChanges();
                    return Check;
                }
                return null;

            }
            catch (System.Exception)
            {

                throw;
            }
            

        }

        public IEnumerable<CollabEntity> GetAllCollab(long UserId)
        {
            try
            {
                return context.Collab.Where(o => o.UserId == UserId);

            }
            catch (System.Exception)
            {

                throw;
            }
            
        }
    }
}
