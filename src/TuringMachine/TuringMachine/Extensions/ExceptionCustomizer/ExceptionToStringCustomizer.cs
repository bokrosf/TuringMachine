using System;
using System.Text;

namespace TuringMachine.Extensions.ExceptionCustomizer
{
    /// <summary>
    /// Defines methods for easier overriding of <see cref="Exception.ToString"/> method.
    /// </summary>
    internal static class ExceptionToStringCustomizer
    {
        public static string CustomizeToString<TException>(this TException exception, params string[] customTexts) 
            where TException : Exception
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"{exception.GetType()}: {exception.Message}");

            foreach (var ct in customTexts)
            {
                builder.AppendLine(ct);
            }

            builder.Append(exception.StackTrace);

            return builder.ToString();
        }
    }
}
