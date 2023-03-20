using CommonLayer.Model;
using ManagerLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entity;
using System;

namespace FundooNotesApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollabController : ControllerBase
    {
        private readonly ICollabManager manager;
        public CollabController(ICollabManager manager)
        {
            this.manager = manager;
        }
        [Authorize]
        [HttpPost]
        public ActionResult AddCollab(AddCollabModel model)
        {
            var UserId = Convert.ToInt64(User.FindFirst("Id").Value);
            var Check = manager.AddCollab(model,UserId);
            if (Check != null)
            {
                return Ok(new ResponseModel<CollabEntity> { Status=true,Message="Collaborator is added Successfully",Data= Check}); 
            }
            return BadRequest(new ResponseModel<CollabEntity> { Status = false, Message = "Collaborator is not added", Data = Check });
        }
        [Authorize]
        [HttpDelete("{NoteId}")]
        public ActionResult DeleteCollab(long NoteId,string Email) 
        {
            var UserId = Convert.ToInt64(User.FindFirst("Id").Value);
            var Check=manager.DeleteCollab(UserId,Email,NoteId);
            if(Check!= null)
            {
                return Ok(new ResponseModel<CollabEntity> { Status = true, Message = "Collaborator is deleted Successfully", Data = Check });
            }
            return BadRequest(new ResponseModel<CollabEntity> { Status = false, Message = "Deletion is failed", Data = Check });

        }
        [Authorize]
        [HttpGet]
        public ActionResult GetAllCollab()
        {
            var UserId = Convert.ToInt64(User.FindFirst("Id").Value);
            var Check = manager.GetAllCollab(UserId);
            if (Check != null)
            {
                return Ok(Check);
            }
            return BadRequest(new ResponseModel<CollabEntity> { Status = false, Message = "No Collaborators exists"});
        }
        
    }
}
