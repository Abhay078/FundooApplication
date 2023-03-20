using Microsoft.EntityFrameworkCore.Internal;
using RepositoryLayer.Entity;
using RepositoryLayer.FundooDBContext;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Services
{
    public class LabelRepository:ILabelRepository
    {
        private readonly FunContext context;
        public LabelRepository(FunContext context)
        {
            this.context = context;
        }
        public LabelEntity AddLabel(long UserId,long NoteId,string LabelName)
        {
            LabelEntity labelEntity= new LabelEntity();
            var Check = context.Label.FirstOrDefault(o => o.LabelName.ToLower().Equals(LabelName.ToLower()) && o.NoteId==NoteId);
            if(Check != null)
            {
                return null;
            }
            labelEntity.UserId = UserId;
            labelEntity.NoteId = NoteId;
            labelEntity.LabelName = LabelName;
            context.Add(labelEntity);
            context.SaveChanges();
            return labelEntity;

        }
        public IEnumerable<LabelEntity> GetAll(long UserId)
        {
            return context.Label.Where(o => o.UserId == UserId).ToList();

        }
        public LabelEntity UpdateLabel(long NoteId, string LabelName,string UpdatedLabelName)
        {
            var check = context.Label.FirstOrDefault(o => o.LabelName.ToLower().Equals(LabelName.ToLower()));
            var Check = GetLabelByNoteId(NoteId);
            
            if (check != null && !Check.Any(o => o.LabelName.ToLower().Equals(UpdatedLabelName.ToLower())))
            {
                check.NoteId= NoteId;
                check.LabelName= UpdatedLabelName;
                context.SaveChanges();
                return check;

            }
            else
            {
                return null;
            }
            

        }
        public bool DeleteLabel(string LabelName)
        {
            var Check=context.Label.Where(o => o.LabelName.ToLower().Equals(LabelName.ToLower()));
            if (Check.Any())
            {
                context.Label.RemoveRange(Check);
                context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
        public IEnumerable<LabelEntity> GetLabelByNoteId(long NoteId)
        {
            var Check=context.Label.Where(o=>o.NoteId== NoteId).ToList();
            if(Check.Any())
            {
                return Check;
            }
            return null;
        }
    }
}
