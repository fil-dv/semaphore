//using Semaphore.Infrastructure.WorkWithFiles;
using Semaphore.Infrastructure.Settings;
using Semaphore.Infrastructure.WorkWithFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semaphore.Infrastructure.Init
{
    public static class Initialiser
    {
        public static void InitName()
        {
            string name = FileHandler.ReadFile(Settings.Settings.PathToName);
            if (name.Length < 1)
            {
                Form_init_name fin = new Form_init_name();
                fin.ShowDialog();
            }
            else
            {
                Settings.Settings.Name = name;
            }

            //List<string> listStr = settingsStr.Split(';').ToList();
            //if (listStr[0].Contains("name"))
            //{
            //    string[] arr = listStr[0].Split(':');
            //    if()
            //}
        }

        public static void InitTables()
        {

        }
    }
}
