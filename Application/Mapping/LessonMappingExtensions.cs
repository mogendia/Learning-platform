using Application.DTOs.Lesson;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapping
{
    public static class LessonMappingExtensions
    {
        public static LessonListDto ToLessonListDto(
            this Lesson lesson,
            LessonProgress? progress = null)
        {
            var dto = new LessonListDto
            {
                Id = lesson.Id,
                Title = lesson.Title,
                ThumbnailUrl = lesson.ThumbnailUrl,
                DurationInMins = lesson.DurationInMins,
                OrderIndex = lesson.OrderIndex,
                IsCompleted = progress?.IsCompleted ?? false,
                watchedMins = progress?.WatchedMinutes ?? 0
            };

            return dto;
        }
    }
}
