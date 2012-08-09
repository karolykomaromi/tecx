namespace TecX.Agile.Phone
{
    using System;

    public static class Guard
    {
        public static void AssertNotNull(object param, string paramName)
        {
            if(param == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        public static void AssertNotEmpty(string param, string paramName)
        {

            if (param == null)
            {
                throw new ArgumentNullException(paramName);
            }

            if(param == string.Empty)
            {
                throw new ArgumentException("String must not be empty.", paramName);
            }
        }
    }
}
