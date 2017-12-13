using Semaphore.Infrastructure.Init;
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
            MyInit();
        }

        void MyInit()
        {
            Initialiser.InitName();
            this.Text += (" (" + Settings.Name + ")"); 
        }

        private void настройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_settings fs = new Form_settings();
            fs.ShowDialog();
        }
    }
}
