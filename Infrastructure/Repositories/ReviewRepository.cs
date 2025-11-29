using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        public ReviewRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Review>> GetActiveReviewsAsync()
        {
            return await _context.Reviews
                .Where(i=>i.IsActive)
                .OrderBy(o=>o.OrderIndex)
                .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetAllOrderedAsync()
        {
            return await _context.Reviews
                .OrderBy(o => o.OrderIndex)
                .ToListAsync();
        }
    }
}
