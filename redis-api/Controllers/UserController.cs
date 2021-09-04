using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using redis_common.Infrastructure;
using redis_common.Models;

namespace redis_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ICacheService _cacheService;
        private readonly IMessagePublisher _messageQueue;

        public UserController(ICacheService cacheService, IMessagePublisher messageQueue)
        {
            _cacheService = cacheService;
            _messageQueue = messageQueue;
        }

        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            var models = await _cacheService.GetAsync<List<UserDto>>("models") ?? new List<UserDto>();
            var model = new UserDto(Faker.Name.First(), Faker.Name.Last(), Faker.Identification.DateOfBirth());
            models.Add(model);
            await _cacheService.SetAsync("models", models);
            await _messageQueue.UserCreatedAsync(model);

            return Ok(model);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var models = await _cacheService.GetAsync<List<UserDto>>("models");
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
            var cache = await _cacheService.GetAsync<List<UserDto>>("models");
            return Ok(cache);
        }
    }
}