using System.Collections.Generic;

// 53-1 Custom Validation Error Response
// used when system send mode state errors.
namespace API.Errors
{
    public class ApiValidationErrorResponse : ApiResponse
    {
        public ApiValidationErrorResponse() : base(400)
        {
        }

        // The list of errors, to be used to indicate the various
        // errors that ocurred at a form or anything that use multiple variables.
        public IEnumerable<string> Errors { get; set; }
    }
}