using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class EventStatisticsRepository : RepositoryBase<EventStatistics>, IEventStatisticsRepository
    {
        private readonly ApplicationDbContext _context;

        public EventStatisticsRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
