using System;

namespace Vnn88.Web.Infrastructure.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    /// Exception type for app exceptions
    /// </summary>
    public class ApiDomainException : Exception
    {
        /// <inheritdoc />
        /// <summary>
        /// Constructor for Class
        /// </summary>
        public ApiDomainException()
        { }

        /// <inheritdoc />
        /// <summary>
        /// Constructor for Class
        /// </summary>
        public ApiDomainException(string message)
            : base(message)
        { }

        /// <inheritdoc />
        /// <summary>
        /// Constructor for Class
        /// </summary>
        public ApiDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
