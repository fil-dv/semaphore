using System;
using System.Collections.Generic;
//using System.Data.OracleClient;
using Oracle.DataAccess.Client;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbLayer
{
    public class OracleConnect
    {
        OracleConnection _con;
        OracleCommand _cmd;
        public OracleConnect()
        {
            _con = new OracleConnection();
            _con.ConnectionString = "User ID=import_user;password=sT7hk9Lm;Data Source=CD_WORK";
        }
        public void OpenConnect()
        {
            _con.Open();
        }

        public void CloseConnect()
        {
            _con.Close();
            _con.Dispose();
        }

        public void ExecCommand(string query)
        {
            _cmd = new OracleCommand(query, _con);
            _cmd.ExecuteNonQuery();
            _cmd.Dispose();
        }

    }
}
