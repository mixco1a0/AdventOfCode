
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AoC.Solid.Core.Interfaces;

namespace AoC.Solid.Services;

public class DayService
{
    public static IDaySolution? GetDaySolution(int year, int day)
    {
        IEnumerable<Type>? allDaySolutionTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(t => typeof(IDaySolution).IsAssignableFrom(t) && !t.IsInterface)
            .Where(t => (t.Namespace ?? "").Contains(year.ToString()));
        if (!(allDaySolutionTypes ??= []).Any())
        {
            return null;
        }

        foreach(Type daySolutionType in allDaySolutionTypes)
        {
            IDaySolution? instance = (IDaySolution)Activator.CreateInstance(daySolutionType)!;
            if (instance != null)
            {
                if (instance.Year == year && instance.Day == day)
                {
                    return instance;
                }
            }
        }

        return null;
    }
}