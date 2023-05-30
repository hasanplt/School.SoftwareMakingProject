namespace School.SoftwareMakingProject.Domain.Responses
{
	public interface IApiResponse
	{
        public bool isSuccess { get; set; }
        public string Message { get; set; }
    }
}
