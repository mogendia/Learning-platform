using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class LiveQuestion : BaseEntity
    {
        public Guid QuestionId { get; set; }
        public Guid SessionId { get; set; }
        public string StudentId { get; set; }
        public LiveQuestionStatus Status { get; set; }
        public string Message { get; set; }

        public LiveSession Session { get; set; }
        public ApplicationUser Student { get; set; }
    }
}
