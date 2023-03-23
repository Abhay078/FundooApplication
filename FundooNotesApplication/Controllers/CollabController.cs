﻿using CommonLayer.Model;
using ManagerLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FundooNotesApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollabController : ControllerBase
    {
        private readonly ICollabManager manager;
        private readonly IDistributedCache distributedCache;

        public CollabController(ICollabManager manager, IDistributedCache distributedCache)
        {
            this.manager = manager;
            this.distributedCache = distributedCache;

        }
        [Authorize]
        [HttpPost]
        public ActionResult AddCollab(AddCollabModel model)
        {
            try
            {
                var UserId = Convert.ToInt64(User.FindFirst("Id").Value);
                var Check = manager.AddCollab(model, UserId);
                if (Check != null)
                {
                    return Ok(new ResponseModel<CollabEntity> { Status = true, Message = "Collaborator is added Successfully", Data = Check });
                }
                return BadRequest(new ResponseModel<CollabEntity> { Status = false, Message = "Collaborator is not added", Data = Check });

            }
            catch (Exception)
            {

                throw;
            }

        }
        [Authorize]
        [HttpDelete("{NoteId}")]
        public ActionResult DeleteCollab(long NoteId, string Email)
        {
            try
            {
                var UserId = Convert.ToInt64(User.FindFirst("Id").Value);
                var Check = manager.DeleteCollab(UserId, Email, NoteId);
                if (Check != null)
                {
                    return Ok(new ResponseModel<CollabEntity> { Status = true, Message = "Collaborator is deleted Successfully", Data = Check });
                }
                return BadRequest(new ResponseModel<CollabEntity> { Status = false, Message = "Deletion is failed", Data = Check });

            }
            catch (Exception)
            {

                throw;
            }


        }
        [Authorize]
        [HttpGet]
        public ActionResult GetAllCollab()
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
                    serializedCollabList = Encoding.UTF8.GetString(redisLabelList);
                    Check = JsonConvert.DeserializeObject<IEnumerable<CollabEntity>>(serializedCollabList);
                }
                else
                {
                    Check = manager.GetAllCollab(UserId);
                    serializedCollabList = JsonConvert.SerializeObject(Check);
                    redisLabelList = Encoding.UTF8.GetBytes(serializedCollabList);
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(1));
                    distributedCache.Set(cacheKey, redisLabelList, options);

                }
                if (Check.Any())
                {
                    return Ok(Check);
                }
                return BadRequest(new ResponseModel<CollabEntity> { Status = false, Message = "No Collaborators exists" });

            }
            catch (Exception)
            {

                throw;
            }

        }

    }
}
