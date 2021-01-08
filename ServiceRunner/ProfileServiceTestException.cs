using System;
using System.Runtime.Serialization;

namespace OrganizationService.TestHarness
{
    [Serializable]
    public class ProfileServiceTestException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public ProfileServiceTestException()
        {
        }

        public ProfileServiceTestException(string message) : base(message)
        {
        }

        public ProfileServiceTestException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ProfileServiceTestException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}