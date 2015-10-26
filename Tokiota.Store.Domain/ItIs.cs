namespace Tokiota.Store.Domain
{
    using System;

    public static class ItIs
    {
        public static void NotNull<TParameter>(TParameter parameter, string parameterName) where TParameter : class
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        public static void NotEmpty(string parameter, string parameterName)
        {
            if (String.IsNullOrEmpty(parameter))
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        public static void NotEmpty(Guid parameter, string parameterName)
        {
            AreEquals(parameter, Guid.Empty, parameterName);
        }

        public static void AreEquals(Guid guid1, Guid guid2, string parameterName)
        {
            if (guid1 == guid2)
            {
                throw new ArgumentNullException(parameterName);
            }
        }
    }
}
