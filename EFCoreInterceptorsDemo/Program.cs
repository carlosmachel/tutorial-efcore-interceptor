using EFCoreInterceptorsDemo.Data.EF;
using Microsoft.EntityFrameworkCore;

//AddStudents();

Console.WriteLine("---------------------------------------------");
Console.WriteLine("With Interceptor....");
await GetAllStudents(applyInterceptor: true);
Console.WriteLine("---------------------------------------------");

Console.WriteLine("Without Interceptor....");
await GetAllStudents(applyInterceptor: false);
Console.WriteLine("---------------------------------------------");

void AddStudents()
{
    using var context = BuildUniversityContext();
    context.Add(
        new Student
        {
            Id =  1,
            FirstName = "John",
            LastName = "Doe",
            Address = "4 Privet Drive",
        });

    context.Add(
        new Student
        {
            Id = 2,
            FirstName = "Jane",
            LastName = "Doe",
            Address = "4 Privet Drive",
        });
    context.SaveChanges();
}

async Task GetAllStudents(bool applyInterceptor)
{
    using var context = BuildUniversityContext();

    IList<Student> studentCollection = null;
    if (applyInterceptor)
    {
        studentCollection = await context.Students.Where(s => s.LastName != "").ToListAsync();
    }
    else
    {
        studentCollection = await context.Students.ToListAsync();
    }

    Console.WriteLine("Printing List of Students");
    foreach (var student in studentCollection)
    {
        Console.WriteLine(student);
    }
}

UniversityContext BuildUniversityContext()
{
    var dbContextBuilder = new DbContextOptionsBuilder<UniversityContext>();

    // HARDCODED - For this demo.  
    var connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=University; Integrated Security=True;";

    dbContextBuilder.UseSqlServer(connectionString);
    return new UniversityContext(dbContextBuilder.Options);
}