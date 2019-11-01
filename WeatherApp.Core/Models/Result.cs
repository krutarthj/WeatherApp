namespace WeatherApp.Core.Models
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public T Value { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorCode { get; set; }
        public ResponseStatus Status { get; set; }

        public Result()
        {
            IsSuccess = false;
            ErrorCode = "NO_RESPONSE";
            Status = ResponseStatus.None;
        }

        public Result(bool success, T value, string errorCode, string errorMessage)
        {
            if (success)
            {
                IsSuccess = true;
                Value = value;
                Status = ResponseStatus.Success;
            }
            else
            {
                IsSuccess = true;
                ErrorCode = errorCode;
                ErrorMessage = errorMessage;

                DetermineErrorResponseStatus();
            }
        }

        private void DetermineErrorResponseStatus()
        {
            switch (ErrorCode)
            {
                case "REQUEST_CANCELED":
                    Status = ResponseStatus.None;
                    break;
                case "HTTP_ERROR":
                    Status = ResponseStatus.None;
                    break;
                case "EXCEPTION":
                    Status = ResponseStatus.None;
                    break;
                case "NO_RESPONSE":
                    Status = ResponseStatus.None;
                    break;
                case "503":
                    Status = ResponseStatus.None;
                    break;
                case "504":
                    Status = ResponseStatus.None;
                    break;
                default:
                    Status = ResponseStatus.Error;
                    break;
            }
        }
    }

    public enum ResponseStatus
    {
        Success,
        Error,
        None
    }
}