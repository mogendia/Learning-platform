using System;

namespace Application.DTOs.Live
{
    public class LiveSessionDetailsDto
    {
        public Guid SessionId { get; set; }
        public int CourseId { get; set; }
        public string InstructorId { get; set; }
        public string StreamRoomId { get; set; }
        public string Status { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
    }
}
