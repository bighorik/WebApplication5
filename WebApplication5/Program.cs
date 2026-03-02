using Eventuous.EventStore;
using Eventuous;
using Minio;
using System.Text.Json.Serialization;
using System.Text.Json;
using WebApplication5.Controllers;
using WebApplication5.Events;

namespace WebApplication5
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddEventStoreClient("esdb://admin:changeit@localhost:2113?tls=false&tlsVerifyCert=false");
            builder.Services.AddEventStore<EsdbEventStore>();
            builder.Services.AddSingleton<IEventSerializer>(new DefaultEventSerializer(
                new JsonSerializerOptions(JsonSerializerDefaults.Web)
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                }));

            builder.Services.AddCommandService<StudyCommandService, StudyState>();
            TypeMap.RegisterKnownEventTypes(typeof(StudyCreatedEvent).Assembly);

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
