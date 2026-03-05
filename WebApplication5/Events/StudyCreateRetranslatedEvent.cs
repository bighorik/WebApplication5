using Eventuous;

namespace WebApplication5.Events
{
    [EventType(nameof(StudyCreateRetranslatedEvent))]
    public class StudyCreateRetranslatedEvent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Code { get; set; }

        public string Phase { get; set; }
    }
}
