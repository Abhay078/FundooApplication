using CommonLayer.Model;
using ManagerLayer.Interface;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
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
    public class CollabController : ControllerBase
    {
        private readonly ICollabManager manager;
        private readonly IDistributedCache distributedCache;
        private readonly IBus bus;
        private readonly ILogger<CollabController> logger;
        
        

        public CollabController(ICollabManager manager, IDistributedCache distributedCache, IBus bus, ILogger<CollabController> logger)
        {
            this.manager = manager;
            this.distributedCache = distributedCache;
            this.bus = bus;
            this.logger = logger;
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddCollab(AddCollabModel model)
        {
            CollabEntity Check = null;
           
            try
            {
                var UserId = Convert.ToInt64(User.FindFirst("Id").Value);
                Check = manager.AddCollab(model, UserId);
               
                if (Check != null && model != null)
                {
                    Uri url = new Uri("rabbitmq://localhost/CollabQueue");
                    var endPoint = await bus.GetSendEndpoint(url);
                    await endPoint.Send(model);
                    logger.LogInformation("Collaborator is added successfully....");
                    return Ok(new ResponseModel<CollabEntity> { Status = true, Message = "Collaborator is added Successfully and an email will be sent to collaborator", Data = Check });
                }
                return BadRequest(new ResponseModel<CollabEntity> { Status = false, Message = "collaborator is not added", Data = Check });


            }
            catch (CustomException ex)
            {
                
                logger.LogError(ex.Message);
                throw;

            }

        }

        [Authorize]
        [HttpDelete("{NoteId}")]
        public IActionResult DeleteCollab(long NoteId, string Email)
        {
            
            try
            {
                var UserId = Convert.ToInt64(User.FindFirst("Id").Value);
                var Check = manager.DeleteCollab(UserId, Email, NoteId);
                if (Check != null)
                {
                    logger.LogInformation("Collaborator is deleted successfully");
                    return Ok(new ResponseModel<CollabEntity> { Status = true, Message = "Collaborator is deleted Successfully", Data = Check });
                }

                return BadRequest(new ResponseModel<CollabEntity> { Status = false, Message = "Deletion is failed", Data = Check });

            }
            catch (Exception ex)
            {
                

                logger.LogError(ex.Message);
                throw;
            }


        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllCollab()
        {
            
            try
            {
                var UserId = Convert.ToInt64(User.FindFirst("Id").Value);

                var cacheKey = "collabList";
                IEnumerable<CollabEntity> Check;
                string serializedCollabList;
                var redisLabelList = distributedCache.Get(cacheKey);
                if (redisLabelList != null)
                {
                    logger.LogInformation("Collab List is fetching from Cache");
                    serializedCollabList = Encoding.UTF8.GetString(redisLabelList);
                    Check = JsonConvert.DeserializeObject<IEnumerable<CollabEntity>>(serializedCollabList);
                }
                else
                {
                    logger.LogInformation("Collaborators are fetching from database");
                    Check = manager.GetAllCollab(UserId);
                    serializedCollabList = JsonConvert.SerializeObject(Check);
                    redisLabelList = Encoding.UTF8.GetBytes(serializedCollabList);
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(1));
                    await distributedCache.SetAsync(cacheKey, redisLabelList, options);

                }
                if (Check.Any())
                {
                    logger.LogInformation("All Collaborator are fetched");
                    return Ok(Check);
                }


                return BadRequest(new ResponseModel<CollabEntity> { Status = false, Message = "No Collaborators exists" });

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;

            }

        }

    }
}
