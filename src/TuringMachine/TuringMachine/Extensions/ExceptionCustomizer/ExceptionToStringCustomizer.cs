using System;
using System.Text;

namespace TuringMachine.Extensions.ExceptionCustomizer
{
    /// <summary>
    /// Defines methods for easier overriding of <see cref="Exception.ToString"/> method.
    /// </summary>
    internal static class ExceptionToStringCustomizer
    {
        /// <summary>
        /// Adds custom text to the specified exception's <see cref="Exception.ToString"/> result.
        /// </summary>
        /// <typeparam name="TException">Type of the exception.</typeparam>
        /// <param name="exception">Exception.</param>
        /// <param name="customTexts">Texts that are inserted between the message and stacktrace parts.</param>
        /// <returns><see cref="string"/> that contains the typename, message, custom texts and the stacktrace in this order.</returns>
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
