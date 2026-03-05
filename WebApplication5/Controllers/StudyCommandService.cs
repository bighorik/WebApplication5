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
            On<RetranclateCreateStudyCommand>()
                .InState(ExpectedState.New)
                .GetStream(command => new StreamName($"Study-{command.Id}"))
                .Act((state, _, command) =>
                {
                    StudyCreateRetranslatedEvent @event = new StudyCreateRetranslatedEvent()
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
                .GetStream(command => new StreamName($"Study-{command.Id}"))
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

            On<RetranclateUpdateStudyCodeCommand>()
                .InState(ExpectedState.Existing)
                .GetStream(command => new StreamName($"Study-{command.Id}"))
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

                    StudyCodeUpdateRetranslatedEvent @event = new StudyCodeUpdateRetranslatedEvent()
                    {
                        Code = command.Code,
                    };

                    return [@event];
                });


            On<RetranclateDeleteStudyCommand>()
                .InState(ExpectedState.Existing)
                .GetStream(command => new StreamName($"Study-{command.Id}"))
                .Act((state, _, command) =>
                {
                    if (state.IsDeleted)
                    {
                        return [];
                    }

                    return [new StudyDeleteRetranslatedEvent()];
                });
        }
    }
}
