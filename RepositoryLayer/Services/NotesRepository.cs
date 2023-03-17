using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CommonLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Entity;
using RepositoryLayer.FundooDBContext;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Services
{
    public class NotesRepository:INotesRepository
    {
        private readonly FunContext context;
        private readonly Cloudinary cloudinary;
        
        public NotesRepository(FunContext context,Cloudinary cloudinary)
        { 
            this.context = context;
            this.cloudinary= cloudinary;
            
        }
        
        public NotesEntity CreateNote(CreateNoteModel model,long UserId)
        {
            try
            {
                NotesEntity entity = new NotesEntity();
                entity.Title = model.Title;
                entity.Description=model.Description;
                entity.Reminder= model.Reminder;
                entity.Archived= model.Archived;
                entity.IsPinned= model.IsPinned;
                
                entity.Trash= model.Trash;
                entity.Created=DateTime.Now;
                entity.Edited=DateTime.Now;
                
                entity.UserId = UserId;
                 
                var Check = context.Notes.Add(entity);
                if(Check != null)
                {
                    context.SaveChanges();
                    return entity;

                }
                return null;
                


            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<NotesEntity> GetAllNote(long UserId)
        {
            try
            {
                return context.Notes;

            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public NotesEntity GetNoteById(long Noteid,long UserId)
        {
            try
            {
                var Check = context.Notes.FirstOrDefault(o => o.NoteId == Noteid);
                if (Check != null)
                {
                    return Check;
                }
                return null;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public NotesEntity UpdateNote(UpdateNoteModel model,long Noteid,long UserId)
        {
            try
            {
                var Check = context.Notes.FirstOrDefault(o=>o.NoteId== Noteid);
                if (Check != null)
                {
                    Check.Title= model.Title;
                    Check.Description= model.Description;
                    Check.Edited = DateTime.Now;
                    context.SaveChanges();
                    return Check;


                }
                return null;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool DeleteNote(long Noteid,long UserId)
        {
            try
            {
                var Check = context.Notes.FirstOrDefault(o=>o.NoteId== Noteid); 
                if (Check != null)
                {
                    context.Notes.Remove(Check);
                    context.SaveChanges();
                    return true;
                }
                return false;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public NotesEntity ArchieveNote(long Noteid,long UserId)
        {
            try
            {
                var Check = context.Notes.FirstOrDefault(o=>o.NoteId== Noteid);
                if (Check != null)
                {
                    bool value = Check.Archived;
                    if (value)
                    {
                        Check.Archived = false;
                    }
                    else
                    {
                        Check.Archived= true;
                    }
                    context.SaveChanges();
                }
                return Check;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public NotesEntity TrashNote(long Noteid, long UserId)
        {
            try
            {
                var Check = context.Notes.FirstOrDefault(o => o.NoteId == Noteid);
                if (Check != null)
                {
                    bool value = Check.Trash;
                    if (value)
                    {
                        Check.Trash = false;
                    }
                    else
                    {
                        Check.Trash = true;
                    }
                    context.SaveChanges();
                }
                return Check;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public NotesEntity PinNote(long Noteid, long UserId)
        {
            try
            {
                var Check = context.Notes.FirstOrDefault(o => o.NoteId == Noteid);
                if (Check != null)
                {
                    bool value = Check.IsPinned;
                    if (value)
                    {
                        Check.IsPinned = false;
                    }
                    else
                    {
                        Check.IsPinned = true;
                    }
                    context.SaveChanges();
                }
                return Check;

            }
            catch (Exception)
            {

                throw;
            }
        }
        public NotesEntity ColorNote(long Noteid, long UserId,string Color)
        {
            try
            {
                var Check = context.Notes.FirstOrDefault(o=>o.NoteId== Noteid);
                if(Check!=null)
                {
                    Check.BackgroundColor = Color;
                    Check.Edited=DateTime.Now;
                    context.SaveChanges();
                    return Check;
                }
                return null;


            }
            catch (Exception)
            {

                throw;
            }
        }

        public NotesEntity Image(IFormFile image,long Noteid, long UserId)
        {
            try
            {
                var Check = context.Notes.FirstOrDefault(o => o.NoteId == Noteid);
                if (Check != null)
                {
                    var uploadResult = new ImageUploadResult();
                    if (image.Length > 0)
                    {
                        using (var stream = image.OpenReadStream())
                        {
                            var uploadParamns = new ImageUploadParams
                            {
                                File = new FileDescription(image.FileName, stream)
                            };
                            uploadResult = cloudinary.Upload(uploadParamns);
                        }
                    }
                    Check.Image= uploadResult.SecureUrl.AbsoluteUri;
                    Check.Edited = DateTime.Now;
                    context.SaveChanges();
                    return Check;


                }
                return null;
                


            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
