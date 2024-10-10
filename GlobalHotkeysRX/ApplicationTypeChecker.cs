using System;
using System.Linq;

internal static class ApplicationTypeChecker
{
    internal static bool IsWPF()
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .Any(assembly => assembly.GetType("System.Windows.DependencyObject") != null);
    }
    // Future implementation
    //internal static bool isWinforms()
    //{
    //    return AppDomain.CurrentDomain.GetAssemblies()
    //        .Any(assembly => assembly.GetType("System.Windows.Forms.Form") != null);
    //}

}