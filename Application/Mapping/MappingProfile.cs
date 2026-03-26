using Application.DTOs.Course;
using Application.DTOs.Dashboard;
using Application.DTOs.Lesson;
using Application.DTOs.Review;
using Application.DTOs.User;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            #region Courses 
            CreateMap<Course, CourseDetailsDto>()
              .ForMember(dest => dest.InstructorName, opt => opt.MapFrom(src => src.Instructor.FullName))
              .ForMember(dest => dest.TotalLessons, opt => opt.MapFrom(src => src.Lessons.Count))
              .ForMember(dest => dest.TotalStudents, opt => opt.MapFrom(src => src.Enrollments.Count))
              .ForMember(dest => dest.Lessons, opt => opt.MapFrom(src => src.Lessons.OrderBy(l => l.OrderIndex)));

            CreateMap<Course,CourseListDto>()
                .ForMember(dest => dest.InstructorName,
                    opt => opt.MapFrom(src => src.Instructor.FullName))
                .ForMember(dest => dest.TotalLessons,
                    opt => opt.MapFrom(src => src.Lessons.Count))
                .ForMember(dest => dest.TotalStudents,
                    opt => opt.MapFrom(src => src.Enrollments.Count));

            CreateMap<CreateCourseDto, Course>()
               .ForMember(dest => dest.CoverImageUrl, opt => opt.Ignore())
               .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
               .ForMember(dest => dest.IsPublished, opt => opt.MapFrom(src => true));

            CreateMap<UpdateCourseDto, Course>()
               .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
               .ForMember(dest => dest.CoverImageUrl, opt => opt.Ignore())
               .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            #endregion

            #region Lessons
            CreateMap<Lesson, LessonListDto>();

            CreateMap<Lesson, LessonDetailsDto>();

            CreateMap<CreateLessonDto, Lesson>()
               .ForMember(dest => dest.VideoUrl, opt => opt.Ignore())
               .ForMember(dest => dest.ThumbnailUrl, opt => opt.Ignore())
               .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            #endregion

            // #region Enrollments

            // CreateMap<Enrollment, EnrolledCourseDto>()
            //    .ForMember(dest => dest.CourseId,
            //        opt => opt.MapFrom(src => src.CourseId))
            //    .ForMember(dest => dest.CourseTitle,
            //        opt => opt.MapFrom(src => src.Course.Title))
            //    .ForMember(dest => dest.CoverImageUrl,
            //        opt => opt.MapFrom(src => src.Course.CoverImageUrl))
            //    .ForMember(dest => dest.InstructorName,
            //        opt => opt.MapFrom(src => src.Course.Instructor.FullName))
            //    .ForMember(dest => dest.TotalLessons,
            //        opt => opt.MapFrom(src => src.Course.Lessons.Count))
            //    .ForMember(dest => dest.CompletedLessons, opt => opt.Ignore())
            //    .ForMember(dest => dest.ProgressPercentage, opt => opt.Ignore());
            // #endregion

            #region Students
            CreateMap<Enrollment, StudentProgressDto>()
               .ForMember(dest => dest.StudentId,
                   opt => opt.MapFrom(src => src.StudentId))
               .ForMember(dest => dest.StudentName,
                   opt => opt.MapFrom(src => src.Student.FullName))
               .ForMember(dest => dest.Email,
                   opt => opt.MapFrom(src => src.Student.Email))
               .ForMember(dest => dest.PhoneNumber,
                   opt => opt.MapFrom(src => src.Student.PhoneNumber))
               .ForMember(dest => dest.EnrolledAt,
                   opt => opt.MapFrom(src => src.EnrolledAt))
               .ForMember(dest => dest.CompletedLessons, opt => opt.Ignore())
               .ForMember(dest => dest.TotalLessons, opt => opt.Ignore())
               .ForMember(dest => dest.ProgressPercentage, opt => opt.Ignore());

            CreateMap<Course, CourseStatisticsDto>()
                .ForMember(dest => dest.CourseId,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CourseTitle,
                    opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.CoverImageUrl,
                    opt => opt.MapFrom(src => src.CoverImageUrl))
                .ForMember(dest => dest.TotalEnrolled,
                    opt => opt.MapFrom(src => src.Enrollments.Count))
                //.ForMember(dest => dest.Revenue,
                //    opt => opt.MapFrom(src => src.Enrollments.Sum(e => e.PaidAmount)))
                .ForMember(dest => dest.Students, opt => opt.Ignore());
            #endregion

            #region Application User
            CreateMap<ApplicationUser, UserDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName));
            #endregion

            #region Reviews
            CreateMap<Review, ReviewDto>();
            CreateMap<CreateReviewDto, Review>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));
            #endregion
        }
    }
}
