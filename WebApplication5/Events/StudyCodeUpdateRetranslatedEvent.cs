using Eventuous;

namespace WebApplication5.Events
{
    [EventType(nameof(StudyCodeUpdateRetranslatedEvent))]
    public class StudyCodeUpdateRetranslatedEvent
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
    }
}
