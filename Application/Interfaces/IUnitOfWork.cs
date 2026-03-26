using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICourseRepository Courses { get; }
        ILessonRepository Lessons { get; }
        IEnrollmentRepository Enrollments { get; }
        ILessonProgressRepository LessonProgresses { get; }
        IReviewRepository Reviews { get; }
        ILiveSessionRepository LiveSessions { get; }
        ILiveQuestionRepository LiveQuestions { get; }
        ILiveParticipantRepository LiveParticipants { get; }
        Task<int> SaveAsync();
    }
}
