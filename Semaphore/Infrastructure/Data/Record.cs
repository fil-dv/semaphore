using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semaphore.Infrastructure.Data
{
    public class DbRecord
    {
        string _tableName;
        string _user;
        bool _isFree;
        DateTime startTime;
    }
}
