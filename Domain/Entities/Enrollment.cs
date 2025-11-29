using System;

namespace Domain.Entities
{
    public class Enrollment : BaseEntity
    {
        public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
        public decimal PaidAmount { get; set; }
        public string PaymentTransactionId { get; set; }

        public string StudentId { get; set; }
        public int CourseId { get; set; }

        public virtual ApplicationUser Student { get; set; }
        public virtual Course Course { get; set; }
    }
}
