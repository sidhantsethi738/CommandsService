using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace CommandsService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _serviceFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory serviceFactory, IMapper mapper)
        {
            _serviceFactory = serviceFactory;
            _mapper = mapper;
        }
        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.PlatformPublished:
                    break;
                case EventType.Undetermined:
                    break;
                default:
                    break;
            }
        }

        private EventType DetermineEvent(string message)
        {
            System.Console.WriteLine($"We are determining event");
            var eventType = JsonSerializer.Deserialize<GenericEventDto>(message);

            switch (eventType.Event)
            {
                case "PlatformPublished":
                    System.Console.WriteLine("Platform Publish Event Detected");
                    return EventType.PlatformPublished;

                default:
                    System.Console.WriteLine("could not determine which event detected");
                    return EventType.Undetermined;
            }

        }

        private void AddPlatform(string PlatformPublishMessage)
        {
            using (var scope = _serviceFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();

                var platformPublishDto = JsonSerializer.Deserialize<PlatformPublishDto>(PlatformPublishMessage);

                try
                {
                    var plat = _mapper.Map<Platform>(platformPublishDto);
                    if (!repo.ExternalPlatformExists(plat.ExternalID))
                    {
                        repo.CreatePlatform(plat);
                        repo.SaveChanges();
                    }
                    else
                    {
                        System.Console.WriteLine("Platform already exists");
                    }
                }
                catch (System.Exception ex)
                {
                    System.Console.WriteLine($"Could not add platform to DB -- : {ex}");
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
