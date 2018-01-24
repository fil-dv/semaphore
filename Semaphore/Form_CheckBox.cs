using Semaphore.Infrastructure;
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
    public partial class Form_CheckBox : Form
    {
        public NotifyIcon _appNotifyIcon = new NotifyIcon();
        List<CheckBox> _checklist = new List<CheckBox>();
        DateTime _lastUpdateTime = DateTime.Now;
        bool _isProgrammaticallyUpdate = false;

        public Form_CheckBox()
        {
            InitializeComponent();                     
            RefreshData(true);
            CreateHandlers();
            this.Width = 450;
            this.Height = 86 + (Mediator.TableList.Count * 50);            
        }

        void CreateHandlers()
        {
            foreach (var item in _checklist)
            {
                item.CheckStateChanged += Item_CheckStateChanged;
            }
        }

        private void Item_CheckStateChanged(object sender, EventArgs e)
        {
            if (_isProgrammaticallyUpdate)
            {
                return;
            }
            CheckBox cb = sender as CheckBox;
            if (cb.CheckState == CheckState.Checked)
            {
                Manager.SetTableIsUsed(cb.Name);
            }
            else
            {
                Manager.SetTableIsFree(cb.Name);
            }
        }       

        public void RefreshData(bool isInit = false)
        {
            try
            {
                if (!isInit)
                {
                    TimeSpan span = DateTime.Now - _lastUpdateTime;
                    _lastUpdateTime = DateTime.Now;
                    long ms = (long)span.TotalMilliseconds;
                    if (ms < 100) // не обновляемся чаще, чем 100 миллисеккунд
                    {
                        return;
                    }
                }
                Manager.InitData();              
                SetIconColor();
                _appNotifyIcon.Text = Manager.TipBuilder(); // устанавливаем текст подсказки в трэе
                if (!isInit) // если это не первая инициализация, то отправляем Toast message
                {
                    _appNotifyIcon.ShowBalloonTip(1000, "", Manager.FileReader(AppSettings.PathToSynchronizerFile), ToolTipIcon.Info);
                }
                if (isInit)  // если это первая инициализация, то создает список чекБоксов
                {
                    FillCheckBoxList();
                }
                _isProgrammaticallyUpdate = true;
                RefreshCheckBoxes();
                _isProgrammaticallyUpdate = false;
            }
            catch (Exception ex)
            {
               MessageBox.Show("Exception from Semaphore.Form_CheckBox.RefreshData() " + ex.Message);               
            }
        }

        void FillCheckBoxList()
        {
            for (int i = 0; i < Mediator.TableList.Count; i++)
            {
                CheckBox cb = new CheckBox();
                cb.Left = 50;
                cb.Top = 30 + (i * 50);
                cb.Width = 350;
                cb.Height = 30;
                cb.Appearance = Appearance.Button;
                cb.TextAlign = ContentAlignment.MiddleCenter;
               _checklist.Add(cb);
            }
        }

        void CheckBoxesPreparer()
        {
            foreach (var item in _checklist)
            {
                item.Text = "";
                item.Enabled = true;
                item.BackColor = ColorTranslator.FromHtml("#b3ff99");  // зеленый
            }
        }

        void RefreshCheckBoxes()
        {
            this.Controls.Clear(); 
            CheckBoxesPreparer();

            for (int i = 0; i < Mediator.TableList.Count; i++)
            {
                _checklist[i].Text = Mediator.TableList[i].TableName;
                _checklist[i].Name = Mediator.TableList[i].TableName;
                string ownerName = Mediator.TableList[i].UserName;
                if (ownerName.Length < 1)
                {
                    _checklist[i].Checked = false;                    
                }
                else
                {
                    ownerName = Manager.FirstCharToUpper(ownerName);
                    _checklist[i].Checked = true;
                    if (Environment.UserName.ToLower() != ownerName.ToLower())
                    {
                        _checklist[i].Enabled = false;
                        _checklist[i].BackColor = ColorTranslator.FromHtml("#ff4000");  // красный
                    }
                    else
                    {
                        _checklist[i].BackColor = ColorTranslator.FromHtml("#ffff66"); // желтый
                    }
                    string time = Manager.CalculateTime(Mediator.TableList[i].StartTime);
                    _checklist[i].Text += (" (using by " + ownerName + " " + time + ")");
                }
                this.Controls.Add(_checklist[i]);
            }
           // MessageBox.Show("Refresh(CheckBoxes form)");
        }

        void SetIconColor()
        {
            int recordCount = Mediator.EmptyList.Count + Mediator.BusyList.Count;

            if (Mediator.EmptyList.Count == 0)
            {
                this.Icon = Properties.Resources.IconRed;
                _appNotifyIcon.Icon = Properties.Resources.IconRed;
            }
            else if (Mediator.EmptyList.Count > 0 & Mediator.EmptyList.Count < recordCount)
            {
                SetYellow();
            }
            else
            {
                this.Icon = Properties.Resources.IconGreen;
                _appNotifyIcon.Icon = Properties.Resources.IconGreen;
            }
        }

        void SetYellow()
        {            
            switch (Mediator.EmptyList.Count)
            {
                case 1:
                    this.Icon = Properties.Resources.IconYellow_1;
                    _appNotifyIcon.Icon = Properties.Resources.IconYellow_1;
                    break;
                case 2:
                    this.Icon = Properties.Resources.IconYellow_2;
                    _appNotifyIcon.Icon = Properties.Resources.IconYellow_2;
                    break;
                case 3:
                    this.Icon = Properties.Resources.IconYellow_3;
                    _appNotifyIcon.Icon = Properties.Resources.IconYellow_3;
                    break;
                case 4:
                    this.Icon = Properties.Resources.IconYellow_4;
                    _appNotifyIcon.Icon = Properties.Resources.IconYellow_4;
                    break;
                case 5:
                    this.Icon = Properties.Resources.IconYellow_5;
                    _appNotifyIcon.Icon = Properties.Resources.IconYellow_5;
                    break;
                case 6:
                    this.Icon = Properties.Resources.IconYellow_6;
                    _appNotifyIcon.Icon = Properties.Resources.IconYellow_6;
                    break;
                case 7:
                    this.Icon = Properties.Resources.IconYellow_7;
                    _appNotifyIcon.Icon = Properties.Resources.IconYellow_7;
                    break;
                default:                    
                    _appNotifyIcon.Icon = Properties.Resources.IconYellow;
                    break;
            }
        }



    }

}
