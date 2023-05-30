namespace School.SoftwareMakingProject.Domain.Responses
{
	public class ApiResponse<T> : IApiResponse
	{
        public T? Value { get; set; }
        public bool isSuccess { get; set; }
		public string Message { get; set; }
        public ApiResponse(T? Value, bool isSuccess, string message)
        {
            this.Value = Value;
            this.isSuccess = isSuccess;
            this.Message = message; 
        }
    }
}
