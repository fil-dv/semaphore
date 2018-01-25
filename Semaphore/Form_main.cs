using Semaphore.Infrastructure;
using Semaphore.Infrastructure.Manager;
using Semaphore.Infrastructure.Settings;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Semaphore
{
    public partial class Form_main : Form
    {
        NotifyIcon _appNotifyIcon = new NotifyIcon();
        List<CheckBox> _checklist = new List<CheckBox>();
        DateTime _lastUpdateTime = DateTime.Now;
        bool _isProgrammaticallyUpdate = false;
        ContextMenu _appContextMenu = new ContextMenu();

        public Form_main()
        {
            InitializeComponent();
            Manager.CreateConnect();
            InitContextMenu();
            RefreshData(true);
            CreateHandlers();
            this.Width = 450;
            this.Height = 86 + (Mediator.TableList.Count * 50);
            CheckIsSynchronizerExist();
            CreateFileWatcher(AppSettings.PathToSynchronizerFolder);
            InitNotifyIcon();
            this.FormClosing += Form_main_FormClosing;            
        }

        private void Form_main_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (e.CloseReason == CloseReason.UserClosing)
                {
                    InitNotifyIcon();
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception from Semaphore.Form_main.Form_main_FormClosing() " + ex.Message);
            }
        }

        void InitContextMenu()
        {
            try
            {
                MenuItem exitItem = new MenuItem("Выход");
                exitItem.Click += ExitItem_Click;
                _appContextMenu.MenuItems.Add(exitItem);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception from Semaphore.Form_main.InitContextMenu() " + ex.Message);
            }
        }

        private void ExitItem_Click(object sender, EventArgs e)
        {
            try
            {
                _appNotifyIcon.Visible = false;
                this.Close();
                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception from Semaphore.Form_main.ExitItem_Click() " + ex.Message);
            }            
        }

        void CreateHandlers()
        {
            try
            {
                foreach (var item in _checklist)
                {
                    item.CheckStateChanged += Item_CheckStateChanged;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception from Semaphore.Form_main.CreateHandlers() " + ex.Message);
            }            
        }

        private void Item_CheckStateChanged(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show("Exception from Semaphore.Form_main.Item_CheckStateChanged() " + ex.Message);
            }            
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
            catch (Exception ex)
            {
                MessageBox.Show("Похоже нет доступа или отсутствует доступ к файлу " + AppSettings.PathToSynchronizerFile + "Автосинхронизация не будет работать..." + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("Exception from Semaphore.Form_main.RefreshData() " + ex.Message);
            }
        }

        void FillCheckBoxList()
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show("Exception from Semaphore.Form_main.FillCheckBoxList() " + ex.Message);
            }            
        }

        void CheckBoxesPreparer()
        {
            try
            {
                foreach (var item in _checklist)
                {
                    item.Text = "";
                    item.Enabled = true;
                    item.BackColor = ColorTranslator.FromHtml("#b3ff99");  // зеленый
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception from Semaphore.Form_main.CheckBoxesPreparer() " + ex.Message);
            }            
        }

        void RefreshCheckBoxes()
        {
            try
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception from Semaphore.Form_main.RefreshCheckBoxes() " + ex.Message);
            }            
        }

        void SetIconColor()
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show("Exception from Semaphore.Form_main.SetIconColor() " + ex.Message);
            }            
        }

        void SetYellow()
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show("Exception from Semaphore.Form_main.SetYellow() " + ex.Message);
            }            
        }

        public void CreateFileWatcher(string path)
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show("Exception from Semaphore.Form_main.CreateFileWatcher() " + ex.Message);
            }            
        }


        private void OnChanged(object source, FileSystemEventArgs e)
        {
            try
            {
                if (InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        RefreshData();
                    }));
                }
                else
                {
                    RefreshData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception from Semaphore.Form_main.OnChanged() " + ex.Message);
            }            
        }

        void InitNotifyIcon()
        {
            try
            {
                _appNotifyIcon.Visible = true;
                string text = Manager.TipBuilder();
                _appNotifyIcon.BalloonTipTitle = text;
                _appNotifyIcon.Visible = true;
                _appNotifyIcon.ContextMenu = _appContextMenu;
                _appNotifyIcon.Click += _appNotifyIcon_Click;
                _appNotifyIcon.DoubleClick += _appNotifyIcon_DoubleClick;
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception from Semaphore.Form_main.InitNotifyIcon() " + ex.Message);
            }            
        }

        private void _appNotifyIcon_Click(object sender, EventArgs e)
        {
            try
            {
                ShowForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception from Semaphore.Form_main._appNotifyIcon_Click() " + ex.Message);
            }            
        }

        private void _appNotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                ShowForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception from Semaphore.Form_main._appNotifyIcon_DoubleClick() " + ex.Message);
            }            
        }

        void ShowForm()
        {
            try
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception from Semaphore.Form_main.ShowForm() " + ex.Message);
            }            
        }
    }
}
