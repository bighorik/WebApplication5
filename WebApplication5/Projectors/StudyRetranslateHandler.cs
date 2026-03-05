using Eventuous;
using Mapster;
using WebApplication5.Commands;
using WebApplication5.Controllers;
using WebApplication5.Events;

namespace WebApplication5.Projectors
{
    public class StudyRetranslateHandler : Eventuous.Subscriptions.EventHandler
    {
        public StudyRetranslateHandler(StudyCommandService service)
        {
            On<StudyCreatedEvent>(async context =>
            {
                await service.Handle(context.Message.Adapt<RetranclateCreateStudyCommand>(), default);
            });

            On<StudyCodeUpdatedEvent>(async context =>
            {
                await service.Handle(context.Message.Adapt<RetranclateUpdateStudyCodeCommand>(), default);
            });

            On<StudyDeletedEvent>(async context =>
            {
                await service.Handle(context.Message.Adapt<RetranclateDeleteStudyCommand>(), default);
            });
        }
    }
}