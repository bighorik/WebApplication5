using System.Resources;
using Eventuous;
using WebApplication5.Events;

namespace WebApplication5.Controllers
{
    public record StudyState : State<StudyState>
    {
        public string Name { get; set; } = default!;

        public string Code { get; set; } = default!;

        public string Phase { get; set; } = default!;

        public bool IsDeleted { get; set; } = false;

        public StudyState()
        {
            On<StudyCreatedEvent>((state, @event) =>
            {
                state.Name = @event.Name;
                state.Code = @event.Code;
                state.Phase = @event.Phase;

                return state;
            });

            On<StudyUpdatedEvent>((state, @event) =>
            {
                state.Name = @event.Name ?? state.Name;
                state.Phase = @event.Phase == null ? state.Phase : @event.Phase;

                return state;
            });

            On<StudyCodeUpdatedEvent>((state, @event) =>
            {
                state.Code = @event.Code;

                return state;
            });

            On<StudyDeletedEvent>((state, @event) =>
            {
                state.IsDeleted = true;

                return state;
            });
        }
    }
}
