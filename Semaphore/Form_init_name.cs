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
    public partial class Form_init_name : Form
    {
        public Form_init_name()
        {
            InitializeComponent();
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox_name.Text.Length < 1)
                {
                    MessageBox.Show("Введите ваше имя.");
                }
                else
                {
                    Settings.Name = textBox_name.Text;
                    FileHandler.WriteToFile(@"..\\..\\settings\\name.txt", textBox_name.Text);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("From Semaphore.Form_init_name.button_ok_Click()" + ex.Message);
            }            
        }
    }
}
