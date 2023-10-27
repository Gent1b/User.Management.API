namespace User.Management.API.Models
{
    public class ApiResponse
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public int StatusCode { get; set; }
        public object Response { get; set; }
    }
}
