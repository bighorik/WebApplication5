using Eventuous;

namespace WebApplication5.Events
{
    [EventType(nameof(StudyCreatedEvent))]
    public class StudyCreatedEvent
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public string Phase { get; set; }
    }
}
