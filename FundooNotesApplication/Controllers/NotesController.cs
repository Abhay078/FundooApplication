﻿using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CommonLayer.Model;
using ManagerLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Entity;
using RepositoryLayer.FundooDBContext;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Claims;

namespace FundooNotesApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INoteManager manager;
        public NotesController(INoteManager manager)
        {
            this.manager = manager;

        }
        [Authorize]
        [HttpPost]
        public ActionResult CreateNotes(CreateNoteModel model)
        {
            try
            {
                var UserId = User.FindFirst("Id").Value.ToString();
                long Id = Convert.ToInt64(UserId);
                var Check = manager.CreateNote(model, Id);
                if (Check != null)
                {
                    return Ok(new ResponseModel<NotesEntity> { Status = true, Message = "Note Created Successfully", Data = Check });
                }
                return BadRequest(new ResponseModel<NotesEntity> { Status = false, Message = "Notes Not Created ", Data = Check });

            }
            catch (System.Exception)
            {

                throw;
            }

        }
        [Authorize]
        [HttpGet]
        public ActionResult GetAllNotes()
        {
            try
            {
                var UserId = Convert.ToInt64(User.FindFirst("Id").Value);
                var Check = manager.GetAllNote(UserId);
                if (Check != null)
                {
                    return Ok(Check);
                }

                return BadRequest(new ResponseModel<NotesEntity> { Status = false, Message = $"Failed to get Data" });

            }
            catch (System.Exception)
            {

                throw;
            }

        }
        [Authorize]
        [HttpGet("{id}")]
        public ActionResult GetNotesById(long id)
        {
            try
            {
                var UserId = Convert.ToInt64(User.FindFirst("Id").Value);
                var Check = manager.GetNoteById(id, UserId);
                if (Check != null)
                {
                    return Ok(new ResponseModel<NotesEntity> { Status = true, Message = "Successfull", Data = Check });
                }

                return BadRequest(new ResponseModel<NotesEntity> { Status = false, Message = "Id not Found", Data = Check });


            }
            catch (System.Exception)
            {

                throw;
            }


        }
        [Authorize]
        [HttpPut("Update{id}")]
        public ActionResult UpdateNote(UpdateNoteModel model, long id)
        {
            try
            {
                var UserId = Convert.ToInt64(User.FindFirst("Id").Value);
                var Check = manager.UpdateNote(model, id, UserId);
                if (Check != null)
                {
                    return Ok(new ResponseModel<NotesEntity> { Status = true, Message = "Successfull", Data = Check });
                }
                return BadRequest(new ResponseModel<NotesEntity> { Status = false, Message = "Updation failed", Data = Check });

            }
            catch (System.Exception)
            {

                throw;
            }
        }
        [Authorize]
        [HttpDelete("{id}")]
        public ActionResult DeleteNote(long id)
        {
            try
            {
                var UserId = Convert.ToInt64(User.FindFirst("Id").Value);
                var Check = manager.DeleteNote(id, UserId);
                if (Check)
                {
                    return Ok(new ResponseModel<string> { Status = true, Message = "Note Deleted Successfully" });
                }
                return BadRequest(new ResponseModel<string> { Status = false, Message = "Deletion Failed" });

            }
            catch (Exception)
            {

                throw;
            }
            
        }
        [Authorize]
        [HttpPut("Archieve{NoteId}")]
        public ActionResult Archieve(long NoteId)
        {
            try
            {
                var UserId = Convert.ToInt64(User.FindFirst("Id").Value);
                var Check = manager.ArchieveNote(NoteId, UserId);
                if (Check != null)
                {
                    return Ok(new ResponseModel<NotesEntity> { Status = true, Message = "Toggling Done", Data = Check });
                }
                return BadRequest(new ResponseModel<NotesEntity> { Status = false, Message = "Some Error Occurred", Data = Check });

            }
            catch (Exception)
            {

                throw;
            }
            

        }

        [Authorize]
        [HttpPut("Trash{NoteId}")]
        public ActionResult Trash(long NoteId)
        {
            try
            {
                var UserId = Convert.ToInt64(User.FindFirst("Id").Value);
                var Check = manager.TrashNote(NoteId, UserId);
                if (Check != null)
                {
                    return Ok(new ResponseModel<NotesEntity> { Status = true, Message = "Toggling Done", Data = Check });
                }
                return BadRequest(new ResponseModel<NotesEntity> { Status = false, Message = "Some Error Occurred", Data = Check });

            }
            catch (Exception)
            {

                throw;
            }
           

        }
        [Authorize]
        [HttpPut("Pin{NoteId}")]
        public ActionResult Pinned(long NoteId)
        {
            try
            {
                var UserId = Convert.ToInt64(User.FindFirst("Id").Value);
                var Check = manager.PinNote(NoteId, UserId);
                if (Check != null)
                {
                    return Ok(new ResponseModel<NotesEntity> { Status = true, Message = "Toggling Done", Data = Check });
                }
                return BadRequest(new ResponseModel<NotesEntity> { Status = false, Message = "Some Error Occurred", Data = Check });

            }
            catch (Exception)
            {

                throw;
            }
            

        }

        [Authorize]
        [HttpPut("Color{NoteId}")]
        public ActionResult Color(long NoteId,string color) 
        {
            var UserId = Convert.ToInt64(User.FindFirst("Id").Value);
            var Check = manager.ColorNote(NoteId, UserId, color);
            if (Check != null)
            {
                return Ok(new ResponseModel<NotesEntity> { Status = true, Message = "Color Changed", Data = Check });
            }
            return BadRequest(new ResponseModel<NotesEntity> { Status = false, Message = "Some Error Occured", Data = Check });


        }
        [Authorize]
        [HttpPost("Image{NoteId}")]
        public ActionResult Image(long NoteId,IFormFile image)
        {


            try
            {
                var UserId = Convert.ToInt64(User.FindFirst("Id").Value);
                var Check = manager.Image(image,NoteId,UserId);
                if (Check != null)
                {
                    return Ok(new ResponseModel<NotesEntity> { Status = true, Message = "Color Changed", Data =Check  });
                }
                return BadRequest(new ResponseModel<NotesEntity> { Status = false, Message = "Some Error Occured", Data = Check });
            }
            catch(Exception) 
            {
                throw;

            }

        }
        [HttpGet("Retrieve/{Keyword}")]
        [Authorize]
        public ActionResult RetrieveMatching(string Keyword)
        {
            try
            {
                var UserId = Convert.ToInt64(User.FindFirst("Id").Value);
                int count;
                var Check=manager.RetrieveMatching(Keyword,UserId,out count);
                if (Check!=null)
                {
                    return Ok(new ResponseModel<IEnumerable<NotesEntity>> { Status=true,Message="Data is Retrieved",Data=Check});
                }
                else
                {
                    return BadRequest(new ResponseModel<IEnumerable<NotesEntity>> { Status = false, Message = "Notes not found ",Data=Check });
                }
               

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpGet("Page/{Keyword}/{PageSize}/{PageNumber}")]
        [Authorize]
        public ActionResult Pagination(string Keyword,int PageSize,int PageNumber) {
            try
            {
                var UserId = Convert.ToInt64(User.FindFirst("Id").Value);
                var Check=manager.Pagination(Keyword,PageNumber,PageSize,UserId);
                if (Check != null)
                {
                    return Ok(new ResponseModel<IEnumerable<NotesEntity>> { Status = true, Message = "Data is Retrieved", Data = Check });
                }
                else
                {
                    return BadRequest(new ResponseModel<IEnumerable<NotesEntity>> { Status = false, Message = "Notes is not Available", Data = Check });


                }

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}