using CommonLayer.Model;
using ManagerLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooNotesApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INoteManager manager;
        private readonly IDistributedCache distributedCache;
        public NotesController(INoteManager manager, IDistributedCache distributedCache)
        {
            this.manager = manager;
            this.distributedCache = distributedCache;

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
                var Key = "NoteList";
                string serializedList;
                IEnumerable<NotesEntity> Check;
                var redisList = distributedCache.Get(Key);
                if (redisList != null)
                {
                    serializedList = Encoding.UTF8.GetString(redisList);
                    Check = JsonConvert.DeserializeObject<IEnumerable<NotesEntity>>(serializedList);
                }
                else
                {
                    Check = manager.GetAllNote(UserId);
                    serializedList = JsonConvert.SerializeObject(Check);
                    redisList = Encoding.UTF8.GetBytes(serializedList);
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(1));
                    distributedCache.Set(Key, redisList, options);
                }

                if (Check.Any())
                {
                    return Ok(Check);
                }

                return BadRequest(new ResponseModel<NotesEntity> { Status = false, Message = "Failed to get Data" });

            }
            catch (System.Exception)
            {

                throw;
            }

        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNotesById(long id)
        {
            try
            {
                var UserId = Convert.ToInt64(User.FindFirst("Id").Value);
                var Key = "NoteById";
                string serializedList;
                NotesEntity Check;
                var redisList = await distributedCache.GetAsync(Key);
                if (redisList != null)
                {
                    serializedList = Encoding.UTF8.GetString(redisList);
                    Check = JsonConvert.DeserializeObject<NotesEntity>(serializedList);
                }
                else
                {
                    Check = manager.GetNoteById(id, UserId);
                    serializedList = JsonConvert.SerializeObject(Check);
                    redisList = Encoding.UTF8.GetBytes(serializedList);
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(1));
                    distributedCache.Set(Key, redisList, options);
                }

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
        public ActionResult Color(long NoteId, string color)
        {
            try
            {
                var UserId = Convert.ToInt64(User.FindFirst("Id").Value);
                var Check = manager.ColorNote(NoteId, UserId, color);
                if (Check != null)
                {
                    return Ok(new ResponseModel<NotesEntity> { Status = true, Message = "Color Changed", Data = Check });
                }
                return BadRequest(new ResponseModel<NotesEntity> { Status = false, Message = "Some Error Occured", Data = Check });

            }
            catch (Exception)
            {

                throw;
            }
            


        }
        [Authorize]
        [HttpPost("Image{NoteId}")]
        public ActionResult Image(long NoteId, IFormFile image)
        {


            try
            {
                var UserId = Convert.ToInt64(User.FindFirst("Id").Value);
                var Check = manager.Image(image, NoteId, UserId);
                if (Check != null)
                {
                    return Ok(new ResponseModel<NotesEntity> { Status = true, Message = "Color Changed", Data = Check });
                }
                return BadRequest(new ResponseModel<NotesEntity> { Status = false, Message = "Some Error Occured", Data = Check });
            }
            catch (Exception)
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
                var Check = manager.RetrieveMatching(Keyword, UserId, out count);
                if (Check != null)
                {
                    return Ok(new ResponseModel<IEnumerable<NotesEntity>> { Status = true, Message = "Data is Retrieved", Data = Check });
                }
                else
                {
                    return BadRequest(new ResponseModel<IEnumerable<NotesEntity>> { Status = false, Message = "Notes not found ", Data = Check });
                }


            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpGet("Page/{Keyword}/{PageSize}/{PageNumber}")]
        [Authorize]
        public ActionResult Pagination(string Keyword, int PageSize, int PageNumber)
        {
            try
            {
                var UserId = Convert.ToInt64(User.FindFirst("Id").Value);
                var Check = manager.Pagination(Keyword, PageNumber, PageSize, UserId);
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
