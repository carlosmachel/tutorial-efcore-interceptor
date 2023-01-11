using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace EFCoreInterceptorsDemo.Data.EF
{
    public class UniversityContext : DbContext
    {
        public UniversityContext(DbContextOptions<UniversityContext> options)
            : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // ALERT - DEMO CODE
            // You may want to inject the interceptors into the context 
            optionsBuilder.AddInterceptors(new DemoDbCommandInterceptor());
        }

        public DbSet<Student> Students { get; set; }
    }

    public class DemoDbCommandInterceptor : DbCommandInterceptor
    {
        public override InterceptionResult<DbDataReader> ReaderExecuting(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<DbDataReader> result)
        {
            ModifyCommand(command);

            return result;
        }

        public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<DbDataReader> result,
            CancellationToken cancellationToken = default)
        {
            ModifyCommand(command);

            return new ValueTask<InterceptionResult<DbDataReader>>(result);
        }

        private static void ModifyCommand(DbCommand command)
        {
            if (!command.CommandText.ToLower().Contains("where", StringComparison.Ordinal))
            {
                Console.WriteLine("DemoDbCommandInterceptor: Applying Where");
                command.CommandText += " Where Id = 1";
            }
            else
            {
                Console.WriteLine("DemoDbCommandInterceptor: Applying Where");
                command.CommandText += " And Id = 2";
            }
        }
    }

    public class Student
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public DateTime DeletedOn { get; set; }

        public override string ToString()
        => $"{{ Id: {Id}, FirstName={FirstName}, LastName:{LastName} }}";
    }
}