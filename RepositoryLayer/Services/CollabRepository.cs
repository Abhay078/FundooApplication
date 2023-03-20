using CommonLayer.Model;
using RepositoryLayer.Entity;
using RepositoryLayer.FundooDBContext;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RepositoryLayer.Services
{
    public class CollabRepository:ICollabRepository
    {
        private readonly FunContext context;
        public CollabRepository(FunContext context)
        {
            this.context = context;
        }
        public CollabEntity AddCollab(AddCollabModel model,long UserId)
        {
            var Check = context.Notes.FirstOrDefault(o => o.NoteId == model.NoteId);
            if(Check!= null)
            {
                Regex rx = new Regex(@"^[a-z0-9\w]{3,}@[-a-z0-9]{5,}\.[a-z]{2,5}$");
                Match match = rx.Match(model.Email);
                if (match.Success)
                {
                    CollabEntity entity= new CollabEntity();
                    entity.Email=model.Email;
                    entity.NoteId=model.NoteId;
                    entity.UserId = UserId;
                    context.Collab.Add(entity);
                    context.SaveChanges();
                    return entity;

                }

            }
            return null;
            

        }
        public CollabEntity DeleteCollab(long UserId,string email,long NoteId)
        {
            var CheckNote = context.Collab.FirstOrDefault(o => o.NoteId == NoteId);
            var Check = context.Collab.FirstOrDefault(o => o.Email.Equals(email));
            if(Check!= null && CheckNote !=null)
            {
                context.Collab.Remove(Check);
                context.SaveChanges();
                return Check;
            }
            return null;

        }

        public IEnumerable<CollabEntity> GetAllCollab(long UserId)
        {
            return context.Collab.Where(o => o.UserId == UserId);
        }
    }
}
