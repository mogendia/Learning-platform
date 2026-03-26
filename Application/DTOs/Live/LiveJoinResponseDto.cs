namespace Application.DTOs.Live
{
    public class LiveJoinResponseDto
    {
        public string RoomId { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
        public bool CanSpeak { get; set; }
    }
}
