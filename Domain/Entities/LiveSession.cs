using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class LiveSession : BaseEntity
    {
        public Guid SessionId { get; set; }
        public int CourseId { get; set; }
        public string InstructorId { get; set; }
        public LiveSessionStatus Status { get; set; }
        public string StreamRoomId { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }

        public Course Course { get; set; }
        public ApplicationUser Instructor { get; set; }
        public ICollection<LiveQuestion> Questions { get; set; } = new HashSet<LiveQuestion>();
        public ICollection<LiveParticipant> Participants { get; set; } = new HashSet<LiveParticipant>();
    }
}
