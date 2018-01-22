using Semaphore.Infrastructure.Manager;
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
    public partial class Form_CheckBox : Form
    {
        public Form_CheckBox()
        {
            InitializeComponent();
            InitCheckBoxes();
        }

        void InitCheckBoxes()
        {
            for (int i = 0; i < Manager.GetTableCount(); i++)
            {

            }
        }
    }

}
