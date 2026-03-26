using System;

namespace Domain.Entities
{
    public class LiveParticipant : BaseEntity
    {
        public Guid SessionId { get; set; }
        public string UserId { get; set; }
        public LiveParticipantRole Role { get; set; }
        public bool CanSpeak { get; set; }

        public LiveSession Session { get; set; }
        public ApplicationUser User { get; set; }
    }
}
