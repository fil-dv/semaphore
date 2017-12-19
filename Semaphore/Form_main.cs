using Semaphore.Infrastructure;
using Semaphore.Infrastructure.Init;
using Semaphore.Infrastructure.Manager;
using Semaphore.Infrastructure.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Semaphore
{
    public partial class Form_main : Form
    {
        public Form_main()
        {
            InitializeComponent();
            Init();
        }

        void Init()
        {
            Manager.CreateConnect();
            RefreshData();
        }

        void RefreshData()
        {
            Manager.InitData();
            comboBox_busy.Items.Clear();
            comboBox_empty.Items.Clear();
            FillCombo();
            SetIconColor();
        }

        void SetIconColor()
        {
            if (Mediator.EmptyList.Count == 0)
            {
                this.Icon = Properties.Resources.IconRed;
            }
            else
            {
                this.Icon = Properties.Resources.IconGreen;
            }
        }

        void FillCombo()
        {
            foreach (var item in Mediator.EmptyList)
            {
                comboBox_empty.Items.Add(item.TableName);
            }

            foreach (var item in Mediator.BusyList)
            {
                string time = Manager.CalculateTime(item.StartTime);
                comboBox_busy.Items.Add(item.TableName + " (" + item.UserName + ") занята " + time);
            }

            comboBox_busy.Text = "";
            comboBox_empty.Text = "";
        }

        private void настройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_settings fs = new Form_settings();
            fs.ShowDialog();
        }

        private void button_use_table_Click(object sender, EventArgs e)
        {
            if (comboBox_empty.SelectedItem == null)
            {
                return;
            }
            Manager.SetTableIsUsed(comboBox_empty.SelectedItem.ToString());
            RefreshData();
        }

        private void button_dismiss_table_Click(object sender, EventArgs e)
        {
            if (comboBox_busy.SelectedItem == null)
            {
                return;
            }
            string[] arr = comboBox_busy.SelectedItem.ToString().Split('(');
            string tableName = arr[0].Trim();
            Manager.SetTableIsFree(tableName);
            RefreshData();
        }        

        private void comboBox_empty_SelectedIndexChanged(object sender, EventArgs e)
        {
            button_use_table.Enabled = true;
        }

        private void comboBox_busy_SelectedIndexChanged(object sender, EventArgs e)
        {
            button_dismiss_table.Enabled = true;
        }

        private void button_refresh_Click(object sender, EventArgs e)
        {
            RefreshData();
        }
    }
}
