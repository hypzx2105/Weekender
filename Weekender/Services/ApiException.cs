namespace Weekender.Services
{
    
    public class ApiException : Exception
    {
        
        private string serviceName;
        private int statusCode;

        
        public string ServiceName { get { return serviceName; } }
        public int StatusCode { get { return statusCode; } }

        
        public ApiException(string serviceName, string message) : base(message)
        {
            this.serviceName = serviceName;
            statusCode = 0;
        }

        public ApiException(string serviceName, string message, int statusCode, Exception? innerException) : base(message, innerException)
        {
            this.serviceName = serviceName;
            this.statusCode = statusCode;
        }

       
        public override string ToString()
        {
            return $"ApiException - {serviceName} (status {statusCode}): {Message}";
        }
    }
}