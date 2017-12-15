using Semaphore.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Semaphore.Infrastructure
{
    class Mediator
    {
        public List<DbRecord> EmptyList { get; set; }
        public List<DbRecord> BusyList { get; set; }
    }
}
