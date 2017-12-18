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
            Manager.InitName();
            Manager.CreateConnect();
            Manager.InitData();
            FillCombo();
        }

        void FillCombo()
        {
            foreach (var item in Mediator.EmptyList)
            {
                comboBox_empty.Items.Add(item.TableName);
            }

            foreach (var item in Mediator.BusyList)
            {
                comboBox_busy.Items.Add(item.TableName);
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
            Manager.SetTableIsUsed(comboBox_empty.SelectedItem.ToString(), AppSettings.Name);
            ReInitData();
        }

        void ReInitData()
        {
            Manager.InitData();
            comboBox_busy.Items.Clear();
            comboBox_empty.Items.Clear();
            FillCombo();
        }
    }
}
