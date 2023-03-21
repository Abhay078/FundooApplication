using CommonLayer.Model;
using ManagerLayer.Interface;
using ManagerLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using RepositoryLayer.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FundooNotesApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        private readonly IMemoryCache memoryCache;
        private readonly ILabelManager manager;
        private readonly IDistributedCache distributedCache;
        public LabelController(ILabelManager manager,IMemoryCache memoryCache,IDistributedCache distributedCache)
        {
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;
            this.manager = manager;
        }
        [Authorize]
        [HttpPost]

        public ActionResult AddLabel(LabelModel model)
        {
            long UserId = Convert.ToInt64(User.FindFirst("Id").Value);
            var Check = manager.AddLabel(UserId,model);
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
        public async Task<IActionResult> GetLabels()
        {
            long UserId = Convert.ToInt64(User.FindFirst("Id").Value);

            var cacheKey = "LabelList";
            string serializedLabel;
            IEnumerable<LabelEntity> Check;
            var redisLabelList = await distributedCache.GetAsync(cacheKey);
            if(redisLabelList != null)
            {
                serializedLabel = Encoding.UTF8.GetString(redisLabelList);
                Check=JsonConvert.DeserializeObject <IEnumerable<LabelEntity>>(serializedLabel);
            }
            else
            {
                Check = manager.GetAll(UserId);
                serializedLabel = JsonConvert.SerializeObject(Check);
                redisLabelList = Encoding.UTF8.GetBytes(serializedLabel);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await distributedCache.SetAsync(cacheKey, redisLabelList, options);

            }
            if (Check.Any())
            {
                return Ok(new ResponseModel<IEnumerable<LabelEntity>> { Status = true, Message = "All Label is Retrieved", Data = Check });
            }
            else
            {
                return BadRequest(new ResponseModel<IEnumerable<LabelEntity>> { Status = false, Message = "Failed to retrived", Data = Check });
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
