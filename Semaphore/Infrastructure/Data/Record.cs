using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semaphore.Infrastructure.Data
{
    public class DbRecord
    {
        public string TableName { get; set; }
        public string UserName { get; set; }
        public DateTime StartTime { get; set; }
    }
}
