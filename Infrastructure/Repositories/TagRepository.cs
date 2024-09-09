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
    public class TagRepository : RepositoryBase<Tag>, ITagRepository
    {
        private readonly ApplicationDbContext _context;

        public TagRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
