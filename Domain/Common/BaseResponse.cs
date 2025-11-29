using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    public class BaseResponse<T>
    {
        public string Message { get; set; } = "";
        public T Data { get; set; } 
        public bool Success { get; set; }

        public List<string> Errors { get; set; } = new List<string>();

        public static BaseResponse<T> SuccessResult(T data,string message = "SUCCESS")
        {
            return new BaseResponse<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }
        public static BaseResponse<T> FailureResult(string message, List<string> errors = null)
        {
            return new BaseResponse<T>
            {
                Success = false,
                Message = message,
                Errors = errors ?? []
            };
        }
    }
}
