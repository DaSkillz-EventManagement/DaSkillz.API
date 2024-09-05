using Domain.Entities;
using Domain.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IEventRepo : IRepository<Event>
    {
    }
}
