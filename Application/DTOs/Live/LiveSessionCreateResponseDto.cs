using System;

namespace Application.DTOs.Live
{
    public class LiveSessionCreateResponseDto
    {
        public Guid SessionId { get; set; }
        public string Status { get; set; }
        public string StreamRoomId { get; set; }
    }
}
