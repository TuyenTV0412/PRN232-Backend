namespace Backend.DTO
{
    public class VerifyOtpRequest
    {
        public string Email { get; set; }
        public string ResetCode { get; set; }
    }

}
