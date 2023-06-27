using AutoMapper;
using CommandsService.Repository;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CommandsService.Controllers
{
    [Route("api/commands/[Controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        public readonly ICommandRepo _commandRepo; 
        public readonly IMapper _mapper;
        public PlatformsController(ICommandRepo commandRepo, IMapper mapper)
        {
            _commandRepo = commandRepo;
            _mapper = mapper;
        }


        [HttpPost]
        public ActionResult TestInboundConnection() 
        {          
            Console.WriteLine("--> Inbound POST # Command Service");
            return Ok("Inbound test from platform controller");
            
        }
        [HttpGet]
        public ActionResult <IEnumerable<DTOs.PlatformReadDTO>>GetAllPlatforms() 
        { 
           var obj = _commandRepo.GetAllPlatforms(); 
            if(obj == null)
            {
                return BadRequest();
            }
           
            return Ok(_mapper.Map<IEnumerable<DTOs.PlatformReadDTO>>(obj));  
        }
    }
}
  