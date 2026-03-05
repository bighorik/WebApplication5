using System.Resources;
using Eventuous;
using Mapster;
using WebApplication5.Events;

namespace WebApplication5.Controllers
{

    public class StudyModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;

        public string Code { get; set; } = default!;

        public string Phase { get; set; } = default!;
    }

    public record StudiesState : State<StudiesState>
    {
        public Dictionary<Guid, StudyModel> studies = [];

        public StudiesState()
        {
            On<StudyCreatedEvent>((state, @event) =>
            {
                studies[@event.Id] = @event.Adapt<StudyModel>();

                return state;
            });

            On<StudyCodeUpdatedEvent>((state, @event) =>
            {
                state.studies[@event.Id].Code = @event.Code;

                return state;
            });

            On<StudyDeletedEvent>((state, @event) =>
            {
                state.studies.Remove(@event.Id);

                return state;
            });
        }
    }
}
