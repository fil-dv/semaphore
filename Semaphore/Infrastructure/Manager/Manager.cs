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

        //public static void InitName()
        //{
        //    Initialiser.InitName();
        //}

        //public static void SetName(string name)
        //{
        //    AppSettings.Name = name;
        //    FileHandler.WriteToFile(Settings.AppSettings.PathToName, name);
        //}

        public static void CreateConnect()
        {
            try
            {
                _con = new OracleConnect(AppSettings.DbConnectionString);
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
            ClearLists();

            string query = "select table_name, user_name, start_time from import_user.semaphore";

            List<DbRecord> recList = new List<DbRecord>();

            OracleDataReader reader = _con.GetReader(query);
            while (reader.Read())
            {
                DbRecord rec = new DbRecord();
                rec.TableName = reader[0].ToString();
                rec.UserName = reader[1].ToString();
                //if (reader[2].ToString() == "") rec.StartTime = null;
               // else
                rec.StartTime = reader[2].ToString();

                if (rec.UserName.Length < 1)
                {
                    Mediator.EmptyList.Add(rec);
                }
                else
                {
                    Mediator.BusyList.Add(rec);
                }
            }
            reader.Close();
        }

        static void ClearLists()
        {
            Mediator.BusyList.Clear();
            Mediator.EmptyList.Clear();
        }

        public static void SetTableIsUsed(string tableName)
        {
            try
            {
                var curTime = String.Format("{0:T}", DateTime.Now);
                string updQuery = "update semaphore set user_name = '" + Environment.UserName + "', start_time = '" + curTime + "' where table_name = '" + tableName + "'";
                _con.ExecCommand(updQuery);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void SetTableIsFree(string tableName)
        {
            try
            {
                var curTime = String.Format("{0:T}", DateTime.Now);
                string updQuery = "update semaphore set user_name = null, start_time = null where table_name = '" + tableName + "'";
               
                _con.ExecCommand(updQuery);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string CalculateTime(string startTime)
        {
            int res = 0;
            try
            {
                string[] startArr = startTime.Split(':');
                int startTimeInt = (Convert.ToInt32(startArr[0]) * 60) + Convert.ToInt32(startArr[1]);

                string curTime = String.Format("{0:T}", DateTime.Now);
                string[] curArr = curTime.Split(':');
                int curTimeInt = (Convert.ToInt32(curArr[0]) * 60) + Convert.ToInt32(curArr[1]);

                res = curTimeInt - startTimeInt;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception from Semaphore.Infrastructure.Manager.CalculateTime()" + ex.Message);
            }
            return TimeStringBuilder(res);
        }

        public static string TimeStringBuilder(int minutes)
        {
            string res;

            if (minutes < 0)
            {
                res = "со вчера.";
            }
            else if (minutes == 0)
            {
                res = "только что.";
            }
            else if (minutes < 60)
            {
                res = minutes.ToString() + " мин.";
            }
            else
            {
                int hours = minutes / 60;
                minutes = minutes - hours * 60;
                res = hours.ToString() + " ч. " + minutes + " мин.";
            }
            return res;
        }

    }
}
