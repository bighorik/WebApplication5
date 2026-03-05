using Eventuous;

namespace WebApplication5.Events
{
    [EventType(nameof(StudyDeleteRetranslatedEvent))]
    public class StudyDeleteRetranslatedEvent
    {
        public Guid Id { get; set; }
    }
}
