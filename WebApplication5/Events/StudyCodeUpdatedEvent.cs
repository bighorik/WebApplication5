using Eventuous;

namespace WebApplication5.Events
{
    [EventType(nameof(StudyCodeUpdatedEvent))]
    public class StudyCodeUpdatedEvent
    {
        public string Code { get; set; }
    }
}
