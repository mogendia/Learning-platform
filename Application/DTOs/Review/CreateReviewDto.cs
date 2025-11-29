using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Review
{
    public class CreateReviewDto
    {
        public IFormFile Image { get; set; }
        public int OrderIndex { get; set; }
        public string? StudentName { get; set; }
        public string? Description { get; set; }

    }
}
