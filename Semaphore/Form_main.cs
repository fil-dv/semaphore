using Semaphore.Infrastructure;
using Semaphore.Infrastructure.Data;
using Semaphore.Infrastructure.Init;
using Semaphore.Infrastructure.Manager;
using Semaphore.Infrastructure.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Semaphore
{
    public partial class Form_main : Form
    {
        public NotifyIcon _appNotifyIcon = new NotifyIcon();
        public ContextMenu _appContextMenu = new ContextMenu();
        Form_CheckBox _checkBoxForm;
        AlterForm _gridForm;

        public Form_main()
        {
            InitializeComponent();
            Init();
            CheckIsSynchronizerExist();
            CreateFileWatcher(AppSettings.PathToSynchronizerFolder);
        }

        void CheckIsSynchronizerExist()
        {
            try
            {
                if (!Directory.Exists(AppSettings.PathToSynchronizerFolder))
                {
                    Directory.CreateDirectory(AppSettings.PathToSynchronizerFolder);
                }

                if (!File.Exists(AppSettings.PathToSynchronizerFile))
                {
                    File.Create(AppSettings.PathToSynchronizerFolder).Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Похоже нет доступа или отсутствует доступ к файлу " + AppSettings.PathToSynchronizerFile + "Автосинхронизация не будет работать...", "Exception", MessageBoxButtons.OK, MessageBoxIcon.Information);                
            }
        }

        void Init()
        {
            Manager.CreateConnect();
            InitContextMenu();
            RefreshData(true);
        }

        void InitContextMenu()
        {
            MenuItem checkBoxFormItem = new MenuItem("CheckBox");
            MenuItem gridFormItem = new MenuItem("Grid");
            MenuItem exitItem = new MenuItem("Выход");
            

            exitItem.Click += ExitItem_Click;
            gridFormItem.Click += GridFormItem_Click;
            checkBoxFormItem.Click += CheckBoxFormItem_Click;

            _appContextMenu.MenuItems.Add(checkBoxFormItem);
            //_appContextMenu.MenuItems.Add(gridFormItem);
            _appContextMenu.MenuItems.Add(exitItem);
        }

        private void CheckBoxFormItem_Click(object sender, EventArgs e)
        {
            if (this.Visible == true)
            {
                this.Hide();
            }
            //if (_gridForm != null)
            //{
            //    _gridForm.Close();
            //}
            if (_checkBoxForm == null)
            {
                _checkBoxForm = new Form_CheckBox();
                _checkBoxForm.ShowDialog();
            }
            else
                if (_checkBoxForm.Visible == false)
                {
                    _checkBoxForm.ShowDialog();
                }            
        }

        private void GridFormItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            if (_checkBoxForm != null)
            {
                _checkBoxForm.Close();
            }

            if (_gridForm == null)
            {
                _gridForm = new AlterForm();
            }            
            _gridForm.ShowDialog();
        }

        private void ExitItem_Click(object sender, EventArgs e)
        {
            _appNotifyIcon.Visible = false;
            this.Close();
            Application.Exit();
        }

        void RefreshData(bool isInit = false)
        {
            try
            {
                Manager.InitData();
                comboBox_busy.Items.Clear();
                comboBox_empty.Items.Clear();
                FillCombo();
                SetIconColor();
                _appNotifyIcon.Text = Manager.TipBuilder();

                if (!isInit)
                {
                    //PlaySound();
                    _appNotifyIcon.ShowBalloonTip(1000, "", Manager.FileReader(AppSettings.PathToSynchronizerFile), ToolTipIcon.Info);
                }
                //MessageBox.Show("Refresh(main form)");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception from Semaphore.Form_main.RefreshData() " + ex.Message);
            }
        }

        //void PlaySound()
        //{
        //    SoundPlayer snd = new SoundPlayer(Properties.Resources.vk);
        //    snd.Play();
        //}

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

            string tableName = comboBox_empty.SelectedItem.ToString();

            if (CheckIsTableRealFreeNow(tableName))
            {
                Manager.SetTableIsUsed(tableName);
            }
            else
            {
                MessageBox.Show("Похоже таблица уже занята, обновите данные.", "Oops", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            //RefreshData();
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
            //RefreshData();
        }

        bool CheckIsTableRealFreeNow(string tableName)
        {
            bool res = true;
            //DbRecord record = Mediator.EmptyList.Where(x => x.TableName == tableName).First(); //new DbRecord();
            DbRecord record = new DbRecord();
            List<DbRecord> records = Mediator.EmptyList.Where(x => x.TableName == tableName).ToList();
            if (records.Count > 0)
            {
                record = records[0];
                if (record.UserName.Length > 0)
                {
                    res = false;
                }
            }

            return res;
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
            RefreshData(true);
        }

        public void CreateFileWatcher(string path)
        {
            // Create a new FileSystemWatcher and set its properties.
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = path;
            /* Watch for changes in LastAccess and LastWrite times, and 
               the renaming of files or directories. */
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            //watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
            //   | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            // Only watch text files.
            //watcher.Filter = "*.txt";
            // Add event handlers.
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            //watcher.Created += new FileSystemEventHandler(OnChanged);
            //watcher.Deleted += new FileSystemEventHandler(OnChanged);
            //watcher.Renamed += new RenamedEventHandler(OnRenamed);
            // Begin watching.
            watcher.EnableRaisingEvents = true;
        }


        private void OnChanged(object source, FileSystemEventArgs e)
        {            
            if (InvokeRequired)
            {
                this.Invoke(new Action(() => {
                    RefreshData();
                    if (_checkBoxForm != null) _checkBoxForm.RefreshData();
                }));
            }
            else
            {
                RefreshData();
                if (_checkBoxForm != null) _checkBoxForm.RefreshData();
            }
        }
        

        private void Form_main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {               
                _appNotifyIcon.Visible = true;
                string text = Manager.TipBuilder();
                _appNotifyIcon.BalloonTipTitle = text;               
                _appNotifyIcon.Visible = true;
                _appNotifyIcon.ContextMenu = _appContextMenu;
                _appNotifyIcon.Click += _appNotifyIcon_Click;
                _appNotifyIcon.DoubleClick += _appNotifyIcon_DoubleClick;
                this.Hide();
                e.Cancel = true;               
            }            
        }

        //string TipBuilder()
        //{
        //    string res = "Свободны: \n";
        //    for(int i = 0; i < Mediator.EmptyList.Count; ++i) 
        //    {
        //        if (Mediator.EmptyList[i].TableName == "IMPORT_UPDATE_COMISIYA")
        //        {
        //            res += "IUC";
        //        }
        //        else if (Mediator.EmptyList[i].TableName == "IMPORT_CLNT_EXAMPLE")
        //        {
        //            res += "ICE";
        //        }
        //        else
        //        {
        //            res += Mediator.EmptyList[i].TableName;
        //        }
                
        //        if (i < Mediator.EmptyList.Count - 1)
        //        {
        //            res += ", \n";
        //        }
        //    }
        //    //res += "\nЗаняты:";
        //    //for (int i = 0; i < Mediator.BusyList.Count; ++i)
        //    //{
        //    //    res += Mediator.BusyList[i].TableName;
        //    //    if (i < Mediator.BusyList.Count - 1)
        //    //    {
        //    //        res += ", \n";
        //    //    }
        //    //}

        //    return res;
        //}

        private void _appNotifyIcon_Click(object sender, EventArgs e)
        {
            ShowForm();
        }

        private void _appNotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            ShowForm();
        }

        void ShowForm()
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }


    }
}
