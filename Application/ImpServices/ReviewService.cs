using Application.DTOs.Review;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ImpServices
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unit;
        private readonly ICloudinaryService _cloudinary;
        private readonly IMapper _mapper;

        public ReviewService(IUnitOfWork unit,ICloudinaryService cloudinary,IMapper mapper)
        {
            _unit = unit;
            _cloudinary = cloudinary;
            _mapper = mapper;
        }

        public async Task<BaseResponse<ReviewDto>> CreateReviewsAsync(CreateReviewDto dto)
        {
            try
            {
                var imageUrl = await _cloudinary.UploadImageAsync(dto.Image);

                var review = new Review
                {
                    ImageUrl = imageUrl,
                    OrderIndex = dto.OrderIndex,
                    IsActive = true,
                    StudentName = dto.StudentName,
                    Description = dto.Description,
                    CreatedAt = DateTime.Now
                };
                await _unit.Reviews.AddAsync(review);
                await _unit.SaveAsync();

                var reviewDto = _mapper.Map<ReviewDto>(review);
                return BaseResponse<ReviewDto>.SuccessResult(reviewDto,"Review Created Successfully");
            }
            catch (Exception ex)
            {
                return BaseResponse<ReviewDto>.FailureResult($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<bool>> DeleteReviewAsync(int id)
        {
            try
            {
                var review = await _unit.Reviews.GetByIdAsync(id);
                if (review == null)
                    return BaseResponse<bool>.FailureResult("Review Not Found");
                await _unit.Reviews.DeleteAsync(review);
                await _unit.SaveAsync();
                return BaseResponse<bool>.SuccessResult(true, "Review Deleted Successfully");

            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.FailureResult($"Error :{ex.Message}");
            }

        }

        public async Task<BaseResponse<List<ReviewDto>>> GetActiveReviewAsync()
        {
            try
            {
                var review = await _unit.Reviews.GetActiveReviewsAsync();
                var reviewDto = _mapper.Map<List<ReviewDto>>(review);
                return BaseResponse<List<ReviewDto>>.SuccessResult(reviewDto);
            }
            catch (Exception ex)
            {
                return BaseResponse<List<ReviewDto>>.FailureResult($"Error :{ex.Message}");
            }
        }

        public async Task<BaseResponse<ReviewDto>> GetReviewByIdAsync(int id)
        {
            try
            {
                var review = await _unit.Reviews.GetByIdAsync(id);
                if (review == null)
                    return BaseResponse<ReviewDto>.FailureResult("Review Not Found");
                var reviewDto = _mapper.Map<ReviewDto>(review);
                return BaseResponse<ReviewDto>.SuccessResult(reviewDto, "Get All Reviewed");
            }
            catch (Exception ex)
            {
                return BaseResponse<ReviewDto>.FailureResult($"Error :{ex.Message}");
            }
        }

        public async Task<BaseResponse<List<ReviewDto>>> GetReviewsAsync()
        {
            try { 
            var review = await _unit.Reviews.GetAllOrderedAsync();
            if(review == null)
                return BaseResponse<List<ReviewDto>>.FailureResult("Review Not Found");
            var reviewDto = _mapper.Map<List<ReviewDto>>(review);
            return BaseResponse<List<ReviewDto>>.SuccessResult(reviewDto, "Get All Reviewed");
        }
            catch (Exception ex)
            {
                return BaseResponse<List<ReviewDto>>.FailureResult($"Error :{ex.Message}");
            }

}

        public async Task<BaseResponse<ReviewDto>> UpdateReviewsAsync(UpdateReviewDto dto)
        {
            try
            {
                var review = await _unit.Reviews.GetByIdAsync(dto.Id);
                if (review == null)
                    return BaseResponse<ReviewDto>.FailureResult("Review Not Found");
                if (dto.Image != null)
                    review.ImageUrl = await _cloudinary.UploadImageAsync(dto.Image);

                review.OrderIndex = dto.OrderIndex;
                review.StudentName = dto.StudentName;
                review.Description = dto.Description;
                review.IsActive = dto.IsActive;

                await _unit.Reviews.UpdateAsync(review);
                await _unit.SaveAsync();

                var reviewDto = _mapper.Map<ReviewDto>(review);
                return BaseResponse<ReviewDto>.SuccessResult(reviewDto, "Review Updated Successfully");
            }
            catch (Exception ex)
            {
                return BaseResponse<ReviewDto>.FailureResult($"Error :{ex.Message}");
            }
        }
    }
}
