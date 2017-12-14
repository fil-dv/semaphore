using DbLayer;
using Semaphore.Infrastructure.WorkWithFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Semaphore.Infrastructure.Manager
{
    public static class Manager
    {
       
        public static void SetName(string name)
        {
            Settings.Settings.Name = name;
            FileHandler.WriteToFile(Settings.Settings.PathToName, name);
        }

        public static void CreateConnect()
        {            
            try
            {
                OracleConnect.
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception from Semaphore.Infrastructure.Manager.CreateConnect()" + ex.Message);
            }
        }

        public static void ExecCommand(string command)
        {
            OracleCommand cmd;
        }

    }
}
