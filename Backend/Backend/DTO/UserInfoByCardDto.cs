namespace Backend.DTO
{
    public class UserInfoByCardDto
    {
        public int PersonId { get; set; }
        public string Name { get; set; } = "";
        public string Gender { get; set; } = "";
        public DateOnly? DateOfBirth { get; set; }
        public string Email { get; set; } = "";
        public string Address { get; set; } = "";
        public string Phone { get; set; } = "";
        public int CardId { get; set; }
    }

}
