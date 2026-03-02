using Eventuous;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using WebApplication5.Commands;

namespace WebApplication5.Controllers
{
    [ApiController]
    [Route("/api/study")]
    public class WeatherForecastController(StudyCommandService service, IEventStore eventStore) : ControllerBase
    {

        [HttpGet("id")]
        public async Task<Study> Get(Guid id)
        {
            FoldedEventStream<StudyState> result = await eventStore.LoadState<StudyState>(new StreamName($"Study_{id}"));

            return result.State.Adapt<Study>();
        }

        [HttpPost]
        public async Task<Study> Create(StudyDto dto)
        {
            CreateStudyCommand command = dto.Adapt<CreateStudyCommand>();
            command.Id = Guid.NewGuid();

            Result<StudyState> result = await service.Handle(command, default);
            Study study = result.Get()!.State.Adapt<Study>();
            return study;
        }

        [HttpPut("{id}")]
        public async Task<Study> Update(Guid id, UpdateStudyDto dto)
        {
            UpdateStudyCommand command = new()
            {
                Id = id,
                Name = dto.Name,
                Phase = dto.Phase,
            };

            return (await service.Handle(command, default)).Get()!.State.Adapt<Study>();
        }

        [HttpPut("{id}/code")]
        public async Task<Study> UpdateCode(Guid id, UpdateStudyCodeDto dto)
        {
            UpdateStudyCodeCommand command = new()
            {
                Id = id,
                Code = dto.Code,
            };

            return (await service.Handle(command, default)).Get()!.State.Adapt<Study>();
        }

        [HttpDelete("{id}")]
        public async Task Delete(Guid id)
        {
            DeleteStudyCommand command = new()
            {
                Id = id,
            };

            await service.Handle(command, default);
        }
    }
}


public class Study
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Code { get; set; }

    public string Phase { get; set; }

    public bool IsDeleted { get; set; }
}

public class StudyDto
{
    public string Name { get; set; }

    public string Code { get; set; }

    public string Phase { get; set; }
}


public class UpdateStudyDto
{
    public string Name { get; set; }

    public string Phase { get; set; }
}


public class UpdateStudyCodeDto
{
    public string Code { get; set; }
}

