using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using redis_common.Infrastructure;
using redis_common.Models;

namespace redis_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class User2Controller : ControllerBase
    {
        private readonly IDistributedCacheService _distributedCache;
        private readonly IMessagePublisher _messageQueue;

        public User2Controller(IDistributedCacheService distributedCache, IMessagePublisher messageQueue)
        {
            _distributedCache = distributedCache;
            _messageQueue = messageQueue;
        }

        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            var models = await _distributedCache.GetAsync<List<UserDto>>("models") ?? new List<UserDto>();
            var model = new UserDto(Faker.Name.First(), Faker.Name.Last(), Faker.Identification.DateOfBirth());
            models.Add(model);
            await _distributedCache.SetAsync("models", models);
            await _messageQueue.UserCreatedAsync(model);

            return Ok(model);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var models = await _distributedCache.GetAsync<List<UserDto>>("models");
            if (models != null)
            {
                try
                {
                    var guidId = new Guid(id);
                    var model = models.FirstOrDefault(p => p.Id == guidId);
                    if (model != null)
                    {
                        return Ok(model);
                    }
                }
                catch (System.Exception)
                {
                }
            }
            return BadRequest("Üye bulunamadı");
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var cache = await _distributedCache.GetAsync<List<UserDto>>("models");
            return Ok(cache);
        }
    }
}