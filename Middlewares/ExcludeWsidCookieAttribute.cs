using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace PeakLogix.PickProApi.Middlewares;

[AttributeUsage(AttributeTargets.Class)]
public class ExcludeWsidCookieAttribute : Attribute
{
    public static readonly ImmutableList<string> ExcludedControllers;

    static ExcludeWsidCookieAttribute()
    {
        var assembly = Assembly.GetExecutingAssembly();

        // just get the controller names
        ExcludedControllers = [.. assembly.GetTypes()
            .Where(type => type.GetCustomAttribute<ExcludeWsidCookieAttribute>() != null)
            .Select(type => type.Name.Replace("Controller", ""))
            .ToList()];
    }

}
