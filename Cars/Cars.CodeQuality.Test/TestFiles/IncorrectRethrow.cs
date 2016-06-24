namespace Cars.CodeQuality.Test.TestFiles
{
    using System;

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
                throw ex;
            }
        }
    }
}