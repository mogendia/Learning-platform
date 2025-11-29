using Application.DTOs.Auth;
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IAuthService
    {
        Task<BaseResponse<AuthResponseDto>> RegisterAsync(RegisterDto model);
        Task<BaseResponse<AuthResponseDto>> LoginAsync(LoginDto model);
    }
}
