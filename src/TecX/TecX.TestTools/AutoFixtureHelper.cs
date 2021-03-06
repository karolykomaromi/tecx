﻿using System;

using TecX.Common;

namespace TecX.TestTools
{
    public static class AutoFixtureHelper
    {
        public static object ResolveEnum(Type type)
        {
            Guard.AssertNotNull(type, "type");

            if (type.IsEnum)
            {
                var values = Enum.GetValues(type);
                return values.GetValue(new Random().Next(values.Length));
            }

            return null;
        }
    }
}