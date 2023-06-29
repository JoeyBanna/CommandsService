using AutoMapper;
using CommandsService.Models;
using CommandsService.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace CommandsService.Controllers
{
    [Route("api/commands/platforms/{platformId}/[Controller]")]
    [ApiController]
    public class CommandsController:ControllerBase
    {
        public readonly ICommandRepo _commandRepo;
        public readonly IMapper _mapper;

        public CommandsController(ICommandRepo commandRepo, IMapper mapper)
        {
            _commandRepo = commandRepo;
            _mapper = mapper;
        }
        [HttpGet]
        public ActionResult<IEnumerable<DTOs.CommandReadDTO>> GetCommandsForPlatform(int platformId) 
        {
            Console.WriteLine("--->Get Commands for Platform");
            var obj = _commandRepo.GetAllCommandsForPlatforms(platformId);
            if(!_commandRepo.PlatformExists(platformId))
            {
                Console.WriteLine("Could not be found");
                return NotFound();
            }
            return Ok( _mapper.Map<IEnumerable<DTOs.CommandReadDTO>> (obj));
        }
        [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
        public ActionResult<DTOs.CommandReadDTO> GetCommandForPlatform(int commandId, int platformId)
        {
            Console.WriteLine($"--> Hit GetCommandForPlatform : {platformId} / {commandId}");
            if (!_commandRepo.PlatformExists(platformId)) 
            {
                Console.WriteLine("Could not be found");

                return NotFound();
            }
            var obj = _commandRepo.GetCommand(platformId, commandId);
            return Ok(_mapper.Map<DTOs.CommandReadDTO>(obj));

        }

        [HttpPost("CreateCommand")]
        public ActionResult<DTOs.CommandReadDTO> CreateCommandForPlatform(int platformId, DTOs.CommandCreateDTO createDTO) 
        {
            Console.WriteLine($"--> Hit Create Command For Platform : {platformId}");

            if (!_commandRepo.PlatformExists(platformId))
            {
                Console.WriteLine("Could not be found");

                return NotFound();
            }


            var obj = _mapper.Map<Models.Command>(createDTO);

            _commandRepo.CreateCommand(platformId, obj);

            _commandRepo.SaveChanges();
            var res = _mapper.Map<DTOs.CommandReadDTO>(obj);
            return CreatedAtRoute(nameof(GetCommandForPlatform), new {platformId = platformId, commandId = res.Id}, res);
          
        }

    }
}
 