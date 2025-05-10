using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Errors
{
    /// <summary>
    /// Represents metadata for a specific type of error (standardized structure).
    /// </summary>
    public class ErrorDetail
    {
        /// <summary>
        /// A short, human-readable title for the error.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// A unique code identifying the error (e.g., "License.Invalid").
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// The default error message (can include placeholders like {0} for formatting).
        /// </summary>
        public string DefaultMessage { get; }

        public ErrorDetail(string title, string code, string defaultMessage)
        {
            Title = title;
            Code = code;
            DefaultMessage = defaultMessage;
        }
    }
}
