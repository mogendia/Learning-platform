using System;

namespace Application.DTOs.Live
{
    public class LiveQuestionDto
    {
        public Guid QuestionId { get; set; }
        public Guid SessionId { get; set; }
        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
