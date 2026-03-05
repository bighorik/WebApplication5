using System.Resources;
using Eventuous;
using WebApplication5.Events;

namespace WebApplication5.Controllers
{
    public record StudyState : State<StudyState>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;

        public string Code { get; set; } = default!;

        public string Phase { get; set; } = default!;

        public bool IsDeleted { get; set; } = false;

        public StudyState()
        {
            On<StudyCreateRetranslatedEvent>((state, @event) =>
            {
                state.Id = @event.Id;
                state.Name = @event.Name;
                state.Code = @event.Code;
                state.Phase = @event.Phase;

                return state;
            });

            On<StudyUpdatedEvent>((state, @event) =>
            {
                state.Name = @event.Name ?? state.Name;
                state.Phase = @event.Phase ?? state.Name;

                return state;
            });

            On<StudyCodeUpdateRetranslatedEvent>((state, @event) =>
            {
                state.Code = @event.Code;

                return state;
            });

            On<StudyDeleteRetranslatedEvent>((state, @event) =>
            {
                state.IsDeleted = true;

                return state;
            });
        }
    }
}
