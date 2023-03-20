using CommonLayer.Model;
using ManagerLayer.Interface;
using ManagerLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entity;
using System;
using System.Collections;
using System.Collections.Generic;

namespace FundooNotesApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        private readonly ILabelManager manager;
        public LabelController(ILabelManager manager)
        {
            this.manager = manager;
        }
        [Authorize]
        [HttpPost("{NoteId}/{LabelName}")]

        public ActionResult AddLabel(string LabelName, long NoteId)
        {
            long UserId = Convert.ToInt64(User.FindFirst("Id").Value);
            var Check = manager.AddLabel(UserId, NoteId, LabelName);
            if (Check != null)
            {
                return Ok(new ResponseModel<LabelEntity> { Status = true, Message = "Label is created successfully", Data = Check });
            }
            else
            {
                return BadRequest(new ResponseModel<LabelEntity> { Status = false, Message = "Label already Exists", Data = Check });

            }

        }
        [Authorize]
        [HttpGet]
        public IActionResult GetLabels()
        {
            long UserId = Convert.ToInt64(User.FindFirst("Id").Value);
            var Check = manager.GetAll(UserId);
            if (Check != null)
            {
                return Ok(new ResponseModel<IEnumerable<LabelEntity>> { Status = true, Message = "All Label is Retrieved", Data = Check });
            }
            else
            {
                return BadRequest(new ResponseModel<IEnumerable<LabelEntity>> { Status = false, Message = "All Label is Retrieved", Data = Check });
            }
        }
        [Authorize]
        [HttpPut("{NoteId}/{LabelName}")]
        public IActionResult UpdateLabel(long NoteId, string LabelName, string UpdatedLabelName)
        {
            
            var Check = manager.UpdateLabel(NoteId, LabelName, UpdatedLabelName);
            if (Check != null)
            {
                return Ok(new ResponseModel<LabelEntity> { Status = true, Message = "Updation Successfull", Data = Check });
            }
            return BadRequest(new ResponseModel<LabelEntity> { Status = false, Message = "Updation Unsuccessfull", Data = Check });
        }
        [Authorize]
        [HttpDelete("{labelName}")]
        public ActionResult Delete(string labelName)
        {
            
            var Check = manager.DeleteLabel(labelName);
            if (Check)
            {
                return Ok(new ResponseModel<bool> { Status = true, Message = "Deletion of Label Successfull", Data = Check });
            }
            else
            {
                return BadRequest(new ResponseModel<bool> { Status = false, Message = "Deletion Failed", Data = Check });
            }
        }
        [Authorize]
        [HttpGet("{NoteId}")]
        public ActionResult GetLabelByNoteId(long NoteId)
        {
            var Check = manager.GetLabelByNoteId(NoteId);
            if(Check != null)
            {
                return Ok(new ResponseModel <IEnumerable<LabelEntity>> { Status = true, Message = "Label Found", Data = Check });
            }
            else
            {
                return BadRequest(new ResponseModel<IEnumerable<LabelEntity>> { Status = false, Message = "Label not Found", Data = Check });
            }
        }
        
    }
}
