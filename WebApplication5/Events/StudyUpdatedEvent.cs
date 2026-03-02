using Eventuous;

namespace WebApplication5.Events
{
    [EventType(nameof(StudyUpdatedEvent))]
    public class StudyUpdatedEvent
    {
        public string? Name { get; set; }

        public string? Phase { get; set; }
    }
}
