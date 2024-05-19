﻿using LinqTasks.Extensions;
using LinqTasks.Models;

namespace LinqTasks;

public static partial class Tasks
{
    public static IEnumerable<Emp> Emps { get; set; }
    public static IEnumerable<Dept> Depts { get; set; }

    static Tasks()
    {
        Depts = LoadDepts();
        Emps = LoadEmps();
    }

    /// <summary>
    ///     SELECT * FROM Emps WHERE Job = "Backend programmer";
    /// </summary>
    public static IEnumerable<Emp> Task1()
    {
        return Emps
            .Where(x => x.Job == "Backend programmer");
    }

    /// <summary>
    ///     SELECT * FROM Emps Job = "Frontend programmer" AND Salary>1000 ORDER BY Ename DESC;
    /// </summary>
    public static IEnumerable<Emp> Task2()
    {
        return Emps
            .Where(x => x.Job == "Frontend programmer" && x.Salary > 1000)
            .OrderByDescending(x => x.Ename);
    }


    /// <summary>
    ///     SELECT MAX(Salary) FROM Emps;
    /// </summary>
    public static int Task3()
    {
        return Emps
            .Max(x => x.Salary);

    }

    /// <summary>
    ///     SELECT * FROM Emps WHERE Salary=(SELECT MAX(Salary) FROM Emps);
    /// </summary>
    public static IEnumerable<Emp> Task4()
    {
        return Emps
            .Where(x => x.Salary == Emps.Max(x => x.Salary));
    }

    /// <summary>
    ///    SELECT ename AS Nazwisko, job AS Praca FROM Emps;
    /// </summary>
    public static IEnumerable<object> Task5()
    {
        return Emps
            .Select(x => new {Nazwisko = x.Ename, Praca = x.Job});
    }

    /// <summary>
    ///     SELECT Emps.Ename, Emps.Job, Depts.Dname FROM Emps
    ///     INNER JOIN Depts ON Emps.Deptno=Depts.Deptno
    ///     Rezultat: Złączenie kolekcji Emps i Depts.
    /// </summary>
    public static IEnumerable<object> Task6()
    {
        return Emps
            .Join(Depts,
                Emps => Emps.Deptno,
                Depts => Depts.Deptno,
                (Emps, Depts) => new
                {
                    Emps.Ename,
                    Emps.Job,
                    Depts.Dname
                });
    }

    /// <summary>
    ///     SELECT Job AS Praca, COUNT(1) LiczbaPracownikow FROM Emps GROUP BY Job;
    /// </summary>
    public static IEnumerable<object> Task7()
    {
        return Emps
            .GroupBy(x => x.Job)
            .Select(y => new {Praca = y.Key, LiczbaPracownikow = y.Count()});

    }

    /// <summary>
    ///     Zwróć wartość "true" jeśli choć jeden
    ///     z elementów kolekcji pracuje jako "Backend programmer".
    /// </summary>
    public static bool Task8()
    { 
        return Emps
            .Any(x => x.Job == "Frontend programmer");
    }

    /// <summary>
    ///     SELECT TOP 1 * FROM Emp WHERE Job="Frontend programmer"
    ///     ORDER BY HireDate DESC;
    /// </summary>
    public static Emp Task9()
    {
        return Emps
            .Where(x => x.Job == "Frontend programmer")
            .OrderByDescending(x => x.HireDate)
            .FirstOrDefault();
    }

    /// <summary>
    ///     SELECT Ename, Job, Hiredate FROM Emps
    ///     UNION
    ///     SELECT "Brak wartości", null, null;
    /// </summary>
    public static IEnumerable<object> Task10()
    {
        return Emps
            .Select(x => new {Ename = x.Ename, Job = x.Job, HireDate = x.HireDate})
            .Union(new List<object> {new {Ename = "Brak wartości", Job = (string)null, HireDate = (DateTime?)null}});
    }

    /// <summary>
    ///     Wykorzystując LINQ pobierz pracowników podzielony na departamenty pamiętając, że:
    ///     1. Interesują nas tylko departamenty z liczbą pracowników powyżej 1
    ///     2. Chcemy zwrócić listę obiektów o następującej srukturze:
    ///     [
    ///     {name: "RESEARCH", numOfEmployees: 3},
    ///     {name: "SALES", numOfEmployees: 5},
    ///     ...
    ///     ]
    ///     3. Wykorzystaj typy anonimowe
    /// </summary>
    public static IEnumerable<object> Task11()
    {
        return Emps.Join(Depts,
                Emps => Emps.Deptno,
                Depts => Depts.Deptno,
                (Emps, Depts) => new
                {
                    Emps.Ename,
                    Emps.Job,
                    Depts.Dname
                })
            .GroupBy(x => x.Dname)
            .Select(y => new {name = y.Key, numOfEmployees = y.Count()})
            .Where(z => z.numOfEmployees > 0);
    }

    /// <summary>
    ///     Napisz własną metodę rozszerzeń, która pozwoli skompilować się poniższemu fragmentowi kodu.
    ///     Metodę dodaj do klasy CustomExtensionMethods, która zdefiniowana jest poniżej.
    ///     Metoda powinna zwrócić tylko tych pracowników, którzy mają min. 1 bezpośredniego podwładnego.
    ///     Pracownicy powinny w ramach kolekcji być posortowani po nazwisku (rosnąco) i pensji (malejąco).
    /// </summary>
    public static IEnumerable<Emp> Task12()
    {
        IEnumerable<Emp> result = Emps.GetEmpsWithSubordinates();
        
        return result;
    }

    /// <summary>
    ///     Poniższa metoda powinna zwracać pojedyczną liczbę int.
    ///     Na wejściu przyjmujemy listę liczb całkowitych.
    ///     Spróbuj z pomocą LINQ'a odnaleźć tę liczbę, które występuja w tablicy int'ów nieparzystą liczbę razy.
    ///     Zakładamy, że zawsze będzie jedna taka liczba.
    ///     Np: {1,1,1,1,1,1,10,1,1,1,1} => 10
    /// </summary>
    public static int Task13(int[] arr)
    {
        return arr.GroupBy(x => x)
            .Single(g => g.Count() % 2 != 0)
            .Key;
    }

    /// <summary>
    ///     Zwróć tylko te departamenty, które mają 5 pracowników lub nie mają pracowników w ogóle.
    ///     Posortuj rezultat po nazwie departament rosnąco.
    /// </summary>
    public static IEnumerable<Dept> Task14()
    {
        return Depts
            .GroupJoin(Emps,
                dept => dept.Deptno,
                emp => emp.Deptno,
                (dept, empGroup) => new
                {
                    Dept = dept,
                    EmployeeCount = empGroup.Count()
                })
            .Where(d => d.EmployeeCount == 5 || d.EmployeeCount == 0)
            .Select(d => d.Dept)
            .OrderBy(d => d.Dname);
    }
}