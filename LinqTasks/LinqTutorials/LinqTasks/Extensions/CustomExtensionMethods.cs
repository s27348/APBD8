using LinqTasks.Models;

namespace LinqTasks.Extensions;

public static class CustomExtensionMethods
{
    //Put your extension methods here
    public static IEnumerable<Emp> GetEmpsWithSubordinates(this IEnumerable<Emp> emps)
    {
        return emps.Where(e => emps.Any(sub => sub.Mgr != null && sub.Mgr.Empno == e.Empno))
            .OrderBy(e => e.Ename)
            .ThenByDescending(e => e.Salary);
    }
}