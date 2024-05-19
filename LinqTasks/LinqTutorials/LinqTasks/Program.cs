using LinqTasks.Models;

namespace LinqTasks;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("======= ZAD X =======");

        IEnumerable<Emp> result = Tasks.Task1();

        foreach (Emp emp in result)
        {
            Console.WriteLine(emp);
        }
        
        IEnumerable<Object> result7 = Tasks.Task7();

        foreach (Object emp in result7)
        {
            Console.WriteLine(emp);
        }
    }
}