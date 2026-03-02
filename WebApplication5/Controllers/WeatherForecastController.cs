using Eventuous;
using Microsoft.AspNetCore.Mvc;
using WebApplication5.Commands;

namespace WebApplication5.Controllers
{
    [ApiController]
    [Route("/api/study")]
    public class WeatherForecastController(StudyCommandService service) : ControllerBase
    {

        //[HttpGet("id")]
        //public async Task<Study> Get(Guid id)
        //{
        //}

        [HttpPost]
        public async Task Post(StudyDto dto)
        {
            CreateStudyCommand command = new CreateStudyCommand()
            {
                Id = Guid.NewGuid(),
                Code = dto.Code,
                Name = dto.Name,
                Phase = dto.Phase,
            };

            await service.Handle(command, default);
        }

        //[HttpPut("id")]
        //public async Task<Study> Put(Guid id, StudyDto dto)
        //{
        //}

        //[HttpDelete("id")]
        //public async Task Delete(Guid id)
        //{
        //}
    }
}


public class Study
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Code { get; set; }

    public string Phase { get; set; }
}

public class StudyDto
{
    public string Name { get; set; }

    public string Code { get; set; }

    public string Phase { get; set; }
}

