namespace Tokiota.Store.Domain
{
    using System;
    using System.Diagnostics;

    public static class InDebugItIs
    {
        [Conditional("DEBUG")]
        public static void NotNull<TParameter>(TParameter parameter, string parameterName) where TParameter : class
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }
    }
}
