﻿namespace Hydra.CodeQuality.Test.TestFiles
{
    using System;

    class TooManyConstructorArguments
    {
        public TooManyConstructorArguments(int one, string two, byte[] three, long four, object five, Type six)
        {
        }
    }
}
