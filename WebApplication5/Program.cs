using Eventuous.EventStore;
using Eventuous;
using Minio;
using System.Text.Json.Serialization;
using System.Text.Json;
using WebApplication5.Controllers;
using WebApplication5.Events;
using FluentMigrator.Runner;
using Eventuous.EventStore.Subscriptions;
using Eventuous.Postgresql.Subscriptions;
using WebApplication5.Projectors;
using EventStore.Client;
using StreamSubscription = Eventuous.EventStore.Subscriptions.StreamSubscription;

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

            builder.Services.AddNpgsqlDataSource("Host=localhost;Port=5432;Database=pg4;Username=postgres;Password=admin");
            builder.Services.AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddPostgres()
                    .WithGlobalConnectionString("Host=localhost;Port=5432;Database=pg4;Username=postgres;Password=admin")
                    .ScanIn(typeof(Program).Assembly).For.Migrations());

            builder.Services.AddOptions<PostgresCheckpointStoreOptions>().Configure(options => options.Schema = "public");
            builder.Services.AddCheckpointStore<PostgresCheckpointStore>();

            builder.Services.AddCommandService<StudyCommandService, StudyState>();
            builder.Services.AddCommandService<StudiesCommandService, StudiesState>();
            TypeMap.RegisterKnownEventTypes(typeof(StudyCreatedEvent).Assembly);

            builder.Services.AddSubscription<StreamSubscription, StreamSubscriptionOptions>("StudyProjectorSubscription",
                builder => builder
                .Configure(subscriptionOptions =>
                {
                    subscriptionOptions.StreamName = new StreamName("$ce-Study");
                    subscriptionOptions.ResolveLinkTos = true;
                })
                .UseCheckpointStore<PostgresCheckpointStore>()
                .AddEventHandler<StudyProjector>());


            builder.Services.AddSubscription<StreamPersistentSubscription, StreamPersistentSubscriptionOptions>("StudyPersistentSubscription",
                builder => builder
                .Configure(subscriptionOptions =>
                {
                    subscriptionOptions.StreamName = new StreamName("Studies");
                    subscriptionOptions.ResolveLinkTos = true;
                    subscriptionOptions.SubscriptionSettings = new PersistentSubscriptionSettings(resolveLinkTos: true, StreamPosition.Start);
                })
                .AddEventHandler<StudyRetranslateHandler>());

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            using var scope = app.Services.CreateScope();
            var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();

            app.Run();
        }
    }
}
