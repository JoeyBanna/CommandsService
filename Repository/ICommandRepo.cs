using System.Collections;
using System.Collections.Generic;

namespace CommandsService.Repository
{ 
    public interface ICommandRepo
    {
        bool SaveChanges();
        IEnumerable<Models.Platform> GetAllPlatforms();
        void CreatePlatform (Models.Platform platform);
        bool PlatformExists(int  platformId);
        bool ExternalPlatformExist(int externalPlatformId);


        IEnumerable<Models.Command> GetAllCommandsForPlatforms(int platformId);
        Models.Command GetCommand(int platformId, int commadId);
        void CreateCommand (int platformId, Models.Command command);
    }
}
