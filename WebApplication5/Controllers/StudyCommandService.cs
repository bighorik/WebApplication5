using Eventuous;
using System.ComponentModel.Design;
using WebApplication5.Commands;
using WebApplication5.Events;

namespace WebApplication5.Controllers
{
    public class StudyCommandService : CommandService<StudyState>
    {
        public StudyCommandService(IEventStore store) : base(store)
        {
            On<CreateStudyCommand>()
            .InState(ExpectedState.New)
            .GetStream(command => new StreamName($"Study_{command.Id}"))
            .Act((state, _, command) =>
            {
                StudyCreatedEvent @event = new StudyCreatedEvent()
                {
                    Name = command.Name,
                    Code = command.Code,
                    Phase = command.Phase,
                };

                return [@event];
            });
        }
    }
}
