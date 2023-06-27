using AutoMapper;
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
                    break;
                default:break;
            }
        }


        private EventType DetermineEvent(string notificationMessage)
        {
            Console.WriteLine("---> etermining Event ");
            var eventType = JsonSerializer.Deserialize<DTOs.GenericEventDto>(notificationMessage);
            switch (eventType.Event) 
            {
                case "platform_Published":
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
            }
        }
    }
    enum EventType
    {
        PlatformPublished,
        Undetermined
    }
}
