using Semaphore.Infrastructure;
using Semaphore.Infrastructure.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Semaphore
{
    public partial class AlterForm : Form
    {
        public AlterForm()
        {
            InitializeComponent();
            Init();
        }

        void Init()
        {
            // dataGridView_sema.
           // string query = "select * from semafor";
            GetData("select * from semaphore");
        }

        private void GetData(string selectCommand)
        {
            try
            {
                // Specify a connection string. Replace the given value with a 
                // valid connection string for a Northwind SQL Server sample
                // database accessible to your system.
                //String connectionString = AppSettings.DbConnectionString;

                // Create a new data adapter based on the specified query.
                //SqlDataAdapter dataAdapter = new SqlDataAdapter(selectCommand, connectionString);

                // Create a command builder to generate SQL update, insert, and
                // delete commands based on selectCommand. These are used to
                // update the database.
                //SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);

                // Populate a new data table and bind it to the BindingSource.
                //DataTable table = new DataTable();
                //table.Locale = System.Globalization.CultureInfo.InvariantCulture;
                //dataAdapter.Fill(table);
                bindingSource1.DataSource = Mediator.TableList;

                // Resize the DataGridView columns to fit the newly loaded content.
                dataGridView_sema.AutoResizeColumns(
                    DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
            }
            catch (SqlException ex)
            {
                MessageBox.Show("To run this example, replace the value of the " +
                    "connectionString variable with a connection string that is " +
                    "valid for your system. " + ex.Message);
            }
        }
    }
}
