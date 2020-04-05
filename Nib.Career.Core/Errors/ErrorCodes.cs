using System;
using System.Collections.Generic;
using System.Text;

namespace Nib.Career.Core.Errors
{
    public static class ErrorCodes
    {
        public const string ServerError = "urn:nib:career:api:server-error";

        public static class Location 
        {
            public const string Error = "urn:nib:career:api:location:client-failed";
        }

        public static class Job
        {
            public const string NotExists = "urn:nib:career:api:job:not-exists";
        }
    }
}
