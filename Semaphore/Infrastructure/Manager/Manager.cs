using DbLayer;
using Semaphore.Infrastructure.WorkWithFiles;
using Semaphore.Infrastructure.Settings;
using System;
using System.Windows.Forms;
using Semaphore.Infrastructure.Init;
using Oracle.ManagedDataAccess.Client;
using Semaphore.Infrastructure.Data;
using System.Collections.Generic;

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
            string query = "select table_name, user_name, start_time from import_user.semaphore";

            List<DbRecord> recList = new List<DbRecord>();

            OracleDataReader reader = _con.GetReader(query);
            while (reader.Read())
            {
                DbRecord rec = new DbRecord();
                rec.TableName = reader[0].ToString();
                rec.UserName = reader[1].ToString();
                if (reader[2].ToString() == "") rec.StartTime = null;
                else rec.StartTime = Convert.ToDateTime(reader[2].ToString());

                if (rec.UserName == null)
                {
                    Mediator.EmptyList.Add(rec);
                }
                else
                {
                    Mediator.EmptyList.Add(rec);
                }
            }
            reader.Close();
        }
    }
}
