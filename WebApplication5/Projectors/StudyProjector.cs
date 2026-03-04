using Eventuous;
using Eventuous.Postgresql.Projections;
using Npgsql;
using WebApplication5.Controllers;
using WebApplication5.Events;

namespace WebApplication5.Projectors
{
    public class StudyProjector : PostgresProjector
    {
        public StudyProjector(NpgsqlDataSource source, IEventStore store) : base(source)
        {
            On<StudyCreatedEvent>((connection, ctx) =>
                {

                    NpgsqlCommand command = connection.CreateCommand();
                    command.CommandText = "insert into study (id, name, code, phase) values(@Id, @Name, @Code, @Phase)";
                    command.Parameters.AddWithValue("Id", ctx.Message.Id);
                    command.Parameters.AddWithValue("Name", ctx.Message.Name);
                    command.Parameters.AddWithValue("Code", ctx.Message.Code);
                    command.Parameters.AddWithValue("Phase", ctx.Message.Phase);

                    return command;
                });

            On<StudyUpdatedEvent>(async (connection, ctx) =>
            {
                NpgsqlCommand command = connection.CreateCommand();
                command.CommandText = "update study set name = @name, phase = @Phase where id = @Id";
                FoldedEventStream<StudyState> stateResult = await store.LoadState<StudyState>(ctx.Stream);
                command.Parameters.AddWithValue("Id", stateResult.State.Id);
                command.Parameters.AddWithValue("Name", ctx.Message.Name ?? stateResult.State.Name);
                command.Parameters.AddWithValue("Phase", ctx.Message.Phase ?? stateResult.State.Phase);

                return command;
            });

            On<StudyCodeUpdatedEvent>((connection, ctx) =>
            {
                NpgsqlCommand command = connection.CreateCommand();
                command.CommandText = "update study set code = @Code where id = @Id";
                command.Parameters.AddWithValue("Id", ctx.Stream.GetId());
                command.Parameters.AddWithValue("Code", ctx.Message.Code);

                return command;
            });

            On<StudyDeletedEvent>((connection, ctx) =>
            {
                NpgsqlCommand command = connection.CreateCommand();
                command.CommandText = "delete from study where id = @Ids";
                command.Parameters.AddWithValue("Id", ctx.Stream.GetId());

                return command;
            });
        }
    }
}


