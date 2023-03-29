using CommonLayer.Model;
using FundooNotesApplication;
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
        public LabelEntity AddLabel(long UserId,LabelModel model)
        {
            try
            {
                LabelEntity labelEntity = new LabelEntity();
                var Check = context.Label.FirstOrDefault(o => o.LabelName.ToLower().Equals(model.LabelName.ToLower()) && o.NoteId == model.NoteId);
                if (Check != null)
                {
                    throw new CustomException("Label is already exists for same user");
                }
                labelEntity.UserId = UserId;
                labelEntity.NoteId = model.NoteId;
                labelEntity.LabelName = model.LabelName;
                context.Add(labelEntity);
                context.SaveChanges();
                return labelEntity;


            }
            catch (Exception)
            {

                throw;
            }
            

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
                throw new KeyNotFoundException("LabelName not found");
            }
            

        }
        public bool DeleteLabel(string LabelName)
        {
            try
            {
                var Check = context.Label.Where(o => o.LabelName.ToLower().Equals(LabelName.ToLower()));
                if (Check.Any())
                {
                    context.Label.RemoveRange(Check);
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    throw new KeyNotFoundException("Labelname not found");
                }

            }
            catch (Exception)
            {

                throw;
            }
            
        }
        public IEnumerable<LabelEntity> GetLabelByNoteId(long NoteId)
        {
            var Check=context.Label.Where(o=>o.NoteId== NoteId).ToList();
            if(Check.Any())
            {
                return Check;
            }
            throw new KeyNotFoundException("NoteId not found");
        }
    }
}
