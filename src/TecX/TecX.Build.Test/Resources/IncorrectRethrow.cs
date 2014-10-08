using System;

namespace TecX.Build.Test.Resources
{
    public class IncorrectRethrow
    {
        public void DoIt()
        {
            try
            {
                string foo = "Foo";
            }
            catch (Exception ex)
            {
                throw  ex;
            }
        }
    }
}
