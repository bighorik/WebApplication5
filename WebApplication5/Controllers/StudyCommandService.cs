using Eventuous;
using System.ComponentModel.Design;
using System.Reflection.Metadata.Ecma335;
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
                        Id = command.Id,
                        Name = command.Name,
                        Code = command.Code,
                        Phase = command.Phase,
                    };

                    return [@event];
                });

            On<UpdateStudyCommand>()
                .InState(ExpectedState.Existing)
                .GetStream(command => new StreamName($"Study_{command.Id}"))
                .Act((state, _, command) =>
                {
                    if (state.IsDeleted)
                    {
                        return [];
                    }

                    if (state.Name == command.Name && state.Phase == command.Phase)
                    {
                        return [];
                    }

                    StudyUpdatedEvent @event = new StudyUpdatedEvent()
                    {
                        Name = command.Name == state.Name ? null : command.Name,
                        Phase = command.Phase == state.Phase ? null : command.Phase,
                    };

                    return [@event];
                });

            On<UpdateStudyCodeCommand>()
                .InState(ExpectedState.Existing)
                .GetStream(command => new StreamName($"Study_{command.Id}"))
                .Act((state, _, command) =>
                {
                    if (state.IsDeleted)
                    {
                        return [];
                    }

                    if (state.Code == command.Code)
                    {
                        return [];
                    }

                    StudyCodeUpdatedEvent @event = new StudyCodeUpdatedEvent()
                    {
                        Code = command.Code,
                    };

                    return [@event];
                });


            On<DeleteStudyCommand>()
                .InState(ExpectedState.Existing)
                .GetStream(command => new StreamName($"Study_{command.Id}"))
                .Act((state, _, command) =>
                {
                    if (state.IsDeleted)
                    {
                        return [];
                    }

                    return [new StudyDeletedEvent()];
                });
        }
    }
}
