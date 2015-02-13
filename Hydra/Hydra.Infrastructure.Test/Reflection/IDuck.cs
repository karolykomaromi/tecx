namespace Hydra.Infrastructure.Test.Reflection
{
    using System;

    public interface IDuck
    {
        int Foo { get; set; }

        string Bar { get; set; }

        void Baz(object sender, EventArgs args);

        int TheAnswer();
    }
}