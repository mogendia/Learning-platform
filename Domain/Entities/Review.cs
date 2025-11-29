using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Review : BaseEntity
    {
        public string ImageUrl { get; set; }
        public int OrderIndex { get; set; }
        public bool IsActive { get; set; }

        public string? StudentName { get; set; }
        public string? Description { get; set; }
    }
}
