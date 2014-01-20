namespace Infrastructure.Events
{
    using System;

    public delegate bool TypeFilter(Type m, object filterCriteria);
}