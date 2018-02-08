using DbLayer;
using Semaphore.Infrastructure.WorkWithFiles;
using Semaphore.Infrastructure.Settings;
using System;
using System.Windows.Forms;
using Semaphore.Infrastructure.Init;
using Oracle.ManagedDataAccess.Client;
using Semaphore.Infrastructure.Data;
using System.Collections.Generic;
using System.IO;

namespace Semaphore.Infrastructure.Manager
{
    public static class Manager
    {
        static OracleConnect _con;

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
            try
            {
                if (_con != null)
                {
                    _con.ExecCommand(command);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception from Semaphore.Infrastructure.Manager.ExecCommand()" + ex.Message);
            }            
        }

        public static void InitData()
        {
            try
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
                    rec.StartTime = reader[2].ToString();

                    if (rec.UserName.Length < 1)
                    {
                        Mediator.EmptyList.Add(rec);
                    }
                    else
                    {
                        Mediator.BusyList.Add(rec);
                    }
                    Mediator.TableList.Add(rec);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception from Semaphore.Infrastructure.Manager.InitData()" + ex.Message);
            }           
        }

        //static public int GetTableCount()
        //{
        //    int count = 0;
        //    try
        //    {
        //        string query = "select count(*) from import_user.semaphore";

        //        OracleDataReader reader = _con.GetReader(query);
        //        while (reader.Read())
        //        {
        //            count = Convert.ToInt32(reader[0]);
        //        }
        //        reader.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Exception from Semaphore.Infrastructure.Manager.GetTableCount()" + ex.Message);
        //    }            
        //    return count;
        //}

        static void ClearLists()
        {
            try
            {
                Mediator.BusyList.Clear();
                Mediator.EmptyList.Clear();
                Mediator.TableList.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception from Semaphore.Infrastructure.Manager.ClearLists()" + ex.Message);
            }            
        }

        public static void SetTableIsUsed(string tableName)
        {
            try
            {
                var curTime = String.Format("{0:T}", DateTime.Now);
                string updQuery = "update semaphore set user_name = '" + Environment.UserName + "', start_time = '" + curTime + "' where table_name = '" + tableName + "'";
                _con.ExecCommand(updQuery);
                FileChanger(AppSettings.PathToSynchronizerFile, tableName, "uses");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception from Semaphore.Infrastructure.Manager.SetTableIsUsed() " + ex.Message);
            }
        }


        public static void SetTableIsFree(string tableName, bool isSystemCeaning = false)
        {
            bool isOwner;
            if (isSystemCeaning)
            {
                isOwner = true;
            }
            else
            {
                isOwner = CheckIsOwner(tableName);
            } 
            try
            {
                if (isOwner)
                {                    
                    var curTime = String.Format("{0:T}", DateTime.Now);
                    string updQuery = "update semaphore set user_name = null, start_time = null where table_name = '" + tableName + "'";
                    _con.ExecCommand(updQuery);                    
                    FileChanger(AppSettings.PathToSynchronizerFile, tableName, "freed");                    
                }
                else
                {
                    MessageBox.Show("Освободить таблицу может только тот, кто её занял.", "Oops...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception from Semaphore.Infrastructure.Manager.SetTableIsFree() " + ex.Message);
            }
        }

        static string GetTableOwnerName(string tableName)
        {
            string query = "select user_name from import_user.semaphore where table_name = '" + tableName + "'";
            string userName = "";
            try
            {
                OracleDataReader reader = _con.GetReader(query);

                while (reader.Read())
                {
                    userName = reader[0].ToString();
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception from Semaphore.Infrastructure.Manager.GetTableOwnerName()" + ex.Message);
            }            
            return userName;
        }

        static bool CheckIsOwner(string tableName)
        {
            bool res = false;
            try
            {
                string userName = GetTableOwnerName(tableName);

                if (userName == Environment.UserName)
                {
                    res = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception from Semaphore.Infrastructure.Manager.CheckIsOwner()" + ex.Message);
            }            
            return res;
        }

        static void FileChanger(string fullPath, string tableName, string whatDone)
        {
            try
            {
                using (var tw = new StreamWriter(fullPath, false))
                {
                    string str = Environment.UserName + " " + whatDone + " " + tableName; ;
                    tw.Write(str);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception from Semaphore.Infrastructure.Manager.FileChanger()" + ex.Message);
            }            
        }

        public static string FileReader(string fullPath)
        {
            string fileText = "";
            
            try
            {
                using (var streamReader = File.OpenText(fullPath))
                {
                    fileText = streamReader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception from Semaphore.Infrastructure.Manager.FileReader()" + ex.Message);
            }
            return fileText;
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
            string res = "";
            try
            {
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception from Semaphore.Infrastructure.Manager.TimeStringBuilder()" + ex.Message);
            }            
            return res;
        }

        static public string TipBuilder()
        {
            string res = "Свободны: \n";
            try
            {                
                for (int i = 0; i < Mediator.EmptyList.Count; ++i)
                {
                    if (Mediator.EmptyList[i].TableName == "IMPORT_UPDATE_COMISIYA")
                    {
                        res += "IUC";
                    }
                    else if (Mediator.EmptyList[i].TableName == "IMPORT_CLNT_EXAMPLE")
                    {
                        res += "ICE";
                    }
                    else
                    {
                        res += Mediator.EmptyList[i].TableName;
                    }

                    if (i < Mediator.EmptyList.Count - 1)
                    {
                        res += ", \n";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception from Semaphore.Infrastructure.Manager.TipBuilder()" + ex.Message);
            }            
            return res;
        }

        public static string FirstCharToUpper(string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input[0].ToString().ToUpper() + input.Substring(1);
            }
        }

        internal static void SendMessage(string message)
        {
            try
            {
                using (var tw = new StreamWriter(AppSettings.PathToSynchronizerFile, false))
                {
                    string str = Environment.UserName + ": " + message; ;
                    tw.Write(str);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception from Semaphore.Infrastructure.Manager.SendMessage()" + ex.Message);
            }
        }

        static List<string> GetMyTables()
        {
            List<string> myTablesName = new List<string>();
            try
            {
                string query = "select table_name from import_user.semaphore where user_name = '" + Environment.UserName + "'";
                                
                OracleDataReader reader = _con.GetReader(query);
                while (reader.Read())
                {                    
                    myTablesName.Add(reader[0].ToString());
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception from Semaphore.Infrastructure.Manager.GetMyTables()" + ex.Message);
            }
            return myTablesName;
        }

        public static void CleanMyTables()
        {
            try
            {
                List<string> myList = GetMyTables();
                if (myList.Count > 0)
                {
                    foreach (var item in myList)
                    {
                        SetTableIsFree(item);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception from Semaphore.Infrastructure.Manager.CleanMyTables()" + ex.Message);
            }
        }
    }
}
