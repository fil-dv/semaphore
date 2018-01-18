using Semaphore.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Semaphore.Infrastructure
{
    public static class Mediator
    {
        static List<DbRecord> _emptyList = new List<DbRecord>();

        public static List<DbRecord> EmptyList { get { return _emptyList; } set { _emptyList = value; } }

        static List<DbRecord> _busyList = new List<DbRecord>();
        public static List<DbRecord> BusyList { get { return _busyList; } set { _busyList = value; } }

        static List<DbRecord> _tableList = new List<DbRecord>();
        public static List<DbRecord> TableList { get { return _tableList; } set { _tableList = value; } }

        static string _messageText;
        public static string MessageText { get { return _messageText; } set { _messageText = value; } }
    }
}
