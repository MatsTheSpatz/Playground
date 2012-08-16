using System.Net;

namespace RecipeOcr
{
    public class ProcessingErrorException : WebException
    {
        public ProcessingErrorException(string message, WebException e)
            : base(message, e)
        {
        }
    }
}