namespace Shop3.Utilities.Dtos
{
    public class ApiResponse<T>
    {
        public ApiResponse(T result)
        {
            Successful = true;
            Result = result;
        }

        public ApiResponse(T result,string message)
        {
            Successful = true;
            Result = result;
            Message = message;
        }

        public ApiResponse(bool successful)
        {
            Successful = successful;
        }

        public ApiResponse(bool successful,string message)
        {
            Successful = successful;
            Message = message;
        }

        public ApiResponse(int errorCode, string errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        public ApiResponse(int errorCode, string errorMessage,T result)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
            Result = result;
        }

        public bool Successful { get; set; }

        public T Result { get; set; }

        public int? ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public string SuccessCode { get; set; }

        public string Message { get; set; }

       


    }

}
