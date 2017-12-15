using DbLayer;
using Semaphore.Infrastructure.WorkWithFiles;
using Semaphore.Infrastructure.Settings;
using System;
using System.Windows.Forms;
using Semaphore.Infrastructure.Init;

namespace Semaphore.Infrastructure.Manager
{
    public static class Manager
    {
        static OracleConnect _con;

        public static void InitName()
        {
            Initialiser.InitName();
        }

        public static void SetName(string name)
        {
            AppSettings.Name = name;
            FileHandler.WriteToFile(Settings.AppSettings.PathToName, name);
        }

        public static void CreateConnect()
        {            
            try
            {
                _con = new OracleConnect (AppSettings.DbConnectionString);
                _con.OpenConnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception from Semaphore.Infrastructure.Manager.CreateConnect()" + ex.Message);
            }
        }

        public static void ExecCommand(string command)
        {
            if (_con != null)
            {
                _con.ExecCommand(command);
            }
        }

        public static void InitData()
        {

        }
    }
}
