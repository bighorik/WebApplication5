using Eventuous;
using System.ComponentModel.Design;
using System.Reflection.Metadata.Ecma335;
using WebApplication5.Commands;
using WebApplication5.Events;

namespace WebApplication5.Controllers
{
    public class StudiesCommandService : CommandService<StudiesState>
    {
        public StudiesCommandService(IEventStore store) : base(store)
        {
            On<CreateStudyCommand>()
                .InState(ExpectedState.Any)
                .GetStream(command => new StreamName($"Studies"))
                .Act((state, _, command) =>
                {
                    StudyCreatedEvent @event = new StudyCreatedEvent()
                    {
                        Id = command.Id,
                        Name = command.Name,
                        Code = command.Code,
                        Phase = command.Phase,
                    };

                    return [@event];
                });

            On<UpdateStudyCodeCommand>()
                .InState(ExpectedState.Existing)
                .GetStream(command => new StreamName($"Studies"))
                .Act((state, _, command) =>
                {
                    if (!state.studies.ContainsKey(command.Id))
                    {
                        return [];
                    }

                    if (!state.studies.Any(study => study.Value.Code == command.Code))
                    {
                        return [];
                    }

                    StudyCodeUpdatedEvent @event = new StudyCodeUpdatedEvent()
                    {
                        Id = command.Id,
                        Code = command.Code,
                    };

                    return [@event];
                });


            On<DeleteStudyCommand>()
                .InState(ExpectedState.Existing)
                .GetStream(command => new StreamName($"Studies"))
                .Act((state, _, command) =>
                {
                    if (!state.studies.ContainsKey(command.Id))
                    {
                        return [];
                    }

                    return [new StudyDeletedEvent()];
                });
        }
    }
}
