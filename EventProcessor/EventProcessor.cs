using AutoMapper;
using CommandsService.DTOs;
using CommandsService.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;

namespace CommandsService.EventProcessor
{
    public class EventProcessor : IEventProcessor
    {
        public readonly IMapper _mapper; 
        public readonly IServiceScopeFactory _scopeFactory;
        public EventProcessor(IServiceScopeFactory serviceScopeFactory, IMapper mapper)
        {
            _mapper = mapper;
            _scopeFactory = serviceScopeFactory;
        }

        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);
            switch(eventType) 
            {
                case EventType.PlatformPublished:
                    //to do
                    addPlatform(message);
                    break;
                default:break;
            }
        }


        private EventType DetermineEvent(string notificationMessage)
        {
            Console.WriteLine("---> Determining Event ");
            var eventType = JsonSerializer.Deserialize<DTOs.GenericEventDto>(notificationMessage);
            switch (eventType.Event) 
            {
                case "Platform_Published":
                    Console.WriteLine("--> Plateform published event detected");
                    return EventType.PlatformPublished;
                default:
                    Console.WriteLine("===> could not determine event type");
                    return EventType.Undetermined;
            }
        }


        private void addPlatform(string platformPublishedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();
                var platformPublisheddtto = JsonSerializer.Deserialize<PlatformPublishedDTO>(platformPublishedMessage);


                try
                {
                    var plat = _mapper.Map<Models.Platform>(platformPublisheddtto);
                    if(!repo.ExternalPlatformExist(plat.ExternalId))
                    {
                        repo.CreatePlatform(plat);
                        repo.SaveChanges();
                        Console.WriteLine("---> Platform added.....");

                    }
                    else
                    {
                        Console.WriteLine("---> Platform already Exist.....");
                    }

                }catch(Exception ex)
                {
                    Console.WriteLine($"--> could not add platform to DB : {ex.Message}");
                }
            }
        }
    }
    enum EventType
    {
        PlatformPublished,
        Undetermined
    }
}
