namespace Hydra.Import
{
    using System;
    using System.Diagnostics;

    [DebuggerDisplay("Sheet={Sheet} Type={Type.AssemblyQualifiedName}")]
    public class SheetToTypeMapping
    {
        public string Sheet { get; set; }

        public Type Type { get; set; }
    }
}