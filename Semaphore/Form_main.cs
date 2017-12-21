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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Semaphore
{
    public partial class Form_main : Form
    {
        public NotifyIcon _appNotifyIcon = new NotifyIcon();
        public ContextMenu _appContextMenu = new ContextMenu();

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
                   // throw new NotImplementedException();                   
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Похоже нет доступа или отсутствует доступ к файлу " + AppSettings.PathToSynchronizerFile + "Автосинхронизация не будет работать...", "Exception", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //MessageBox.Show("Exception from Semaphore.Form_main.CheckIsSynchronizerExist()." + ex.Message);
            }            
        }

        void Init()
        {
            Manager.CreateConnect();
            InitContextMenu();
            RefreshData();
        }

        void InitContextMenu()
        {
            MenuItem exitItem = new MenuItem("Exit");
            exitItem.Click += ExitItem_Click;


            _appContextMenu.MenuItems.Add(exitItem);
        }

        private void ExitItem_Click(object sender, EventArgs e)
        {
            _appNotifyIcon.Visible = false;
            this.Close();
            Application.Exit();
        }

        void RefreshData()
        {
            try
            {
                Manager.InitData();
                comboBox_busy.Items.Clear();
                comboBox_empty.Items.Clear();
                FillCombo();
                SetIconColor();
            }
            catch (Exception)
            {
                //
                //
                //
                // MessageBox.Show("Exception from Semaphore.Form_main.RefreshData() " + ex.Message);
                //
                //
                //
            }
        }

        void SetIconColor()
        {
            if (Mediator.EmptyList.Count == 0)
            {
                this.Icon = Properties.Resources.IconRed;
                _appNotifyIcon.Icon = Properties.Resources.IconRed;
            }
            else
            {
                this.Icon = Properties.Resources.IconGreen;
                _appNotifyIcon.Icon = Properties.Resources.IconGreen;
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
            DbRecord record = Mediator.EmptyList.Where(x => x.TableName == tableName).First(); //new DbRecord();
            if (record.UserName.Length > 0)
            {
                res = false;
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
            RefreshData();
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
            // Specify what is done when a file is changed, created, or deleted.
            
            //MessageBox.Show("File: " + e.FullPath + " " + e.ChangeType);
            //_isSinchro = false;
            RefreshData();
        }

        private void Form_main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {               
                _appNotifyIcon.Visible = true;
                _appNotifyIcon.Text = "Semaphore";
                _appNotifyIcon.Visible = true;
                _appNotifyIcon.ContextMenu = _appContextMenu;

                _appNotifyIcon.DoubleClick += _appNotifyIcon_DoubleClick;
                this.Hide();
                e.Cancel = true;               
            }            
        }

        private void _appNotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }
       
    }
}
