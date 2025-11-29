using Application.DTOs.Review;
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IReviewService
    {
        Task<BaseResponse<ReviewDto>> CreateReviewsAsync(CreateReviewDto dto);
        Task<BaseResponse<ReviewDto>> UpdateReviewsAsync(UpdateReviewDto dto);
        Task<BaseResponse<bool>> DeleteReviewAsync(int id);
        Task<BaseResponse<ReviewDto>> GetReviewByIdAsync(int id);
        Task<BaseResponse<List<ReviewDto>>> GetReviewsAsync();
        Task<BaseResponse<List<ReviewDto>>> GetActiveReviewAsync();
    }
}
