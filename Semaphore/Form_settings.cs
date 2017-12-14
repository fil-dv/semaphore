using Semaphore.Infrastructure.Manager;
using Semaphore.Infrastructure.Settings;
using Semaphore.Infrastructure.WorkWithFiles;
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
    public partial class Form_settings : Form
    {
        public Form_settings()
        {
            InitializeComponent();
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            if (textBox_name.Text.Length < 1)
            {
                MessageBox.Show("Введите ваше имя");
            }
            else
            {
                Manager.SetName(textBox_name.Text);
                this.Close();
            }
        }
    }
}
