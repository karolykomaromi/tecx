namespace TecX.Logging
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Wrapper class for Win32 API calls
    /// </summary>
    public static class Win32
    {
        /// <summary>
        /// Allocates a new console for the calling process.
        /// </summary>
        /// <returns><c>true</c> if the function succeeds; otherwise <c>false</c>
        /// </returns>
        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();

        /// <summary>
        /// Detaches the calling process from its console.
        /// </summary>
        /// <returns><c>true</c> if the function succeeds; otherwise <c>false</c>
        /// </returns>
        [DllImport("kernel32.dll")]
        public static extern bool FreeConsole();
    }
}