using CommandsService.Data;
using CommandsService.Models;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CommandsService.Repository
{
    public class CommandRepo : ICommandRepo
    {
        public readonly ApplicationDbContext _context;

        public CommandRepo(ApplicationDbContext applicationDbContext)
        {
            this._context = applicationDbContext;
        }

        public void CreateCommand(int platformId, Command command)
        {
            command.PlatformId = platformId;
            _context.Commands.Add(command);
            _context.SaveChanges();
            
        }

        public void CreatePlatform(Platform platform)
        {
            _context.Platforms.Add(platform);
            _context.SaveChanges();
        }

        public bool ExternalPlatformExist(int externalPlatformId)
        {
            return _context.Platforms.Any(opt => opt.ExternalId == externalPlatformId);

        }

        public IEnumerable<Command> GetAllCommandsForPlatforms(int platformId)
        {
           var commands = _context.Commands.Where(opt => opt.PlatformId == platformId).ToList();
            return commands;
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            var obj = _context.Platforms.ToList();
            return obj;
        }

        public Models.Command GetCommand(int platformId, int commadId)
        {
            return _context.Commands.SingleOrDefault(opt => opt.Id == commadId && opt.PlatformId == platformId);
        }

        public bool PlatformExists(int platformId)
        {
            return _context.Platforms.Any(opt=>opt.Id == platformId);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
