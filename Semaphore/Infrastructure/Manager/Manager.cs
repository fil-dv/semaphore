using Semaphore.Infrastructure.WorkWithFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semaphore.Infrastructure.Manager
{
    public static class Manager
    {
        public static void SetName(string name)
        {
            Settings.Settings.Name = name;
            FileHandler.WriteToFile(Settings.Settings.PathToName, name);
        }
    }
}
