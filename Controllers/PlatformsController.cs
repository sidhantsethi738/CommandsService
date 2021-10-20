using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CommandsService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly ICommandRepo _repo;
        private readonly IMapper _mapper;

        public PlatformsController(ICommandRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            System.Console.WriteLine("Inbount Test ==>");
            return Ok("test Inbound Connection");
        }

        [Route("GetAllPlatforms")]
        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetAllPlatforms()
        {
            System.Console.WriteLine("Getting Platforms from Command Service");
            var result = _repo.GetAllPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(result));
        }

    }
}
