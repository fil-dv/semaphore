namespace Semaphore
{
    partial class Form_main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_main));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label_peyments = new System.Windows.Forms.Label();
            this.label_reg = new System.Windows.Forms.Label();
            this.button_use_table = new System.Windows.Forms.Button();
            this.button_dismiss_table = new System.Windows.Forms.Button();
            this.comboBox_empty = new System.Windows.Forms.ComboBox();
            this.comboBox_busy = new System.Windows.Forms.ComboBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.настройкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button_refresh = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Green;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(18, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Свободные:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Red;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(18, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "    Занятые:";
            // 
            // label_peyments
            // 
            this.label_peyments.AutoSize = true;
            this.label_peyments.Location = new System.Drawing.Point(240, 51);
            this.label_peyments.Name = "label_peyments";
            this.label_peyments.Size = new System.Drawing.Size(0, 13);
            this.label_peyments.TabIndex = 2;
            // 
            // label_reg
            // 
            this.label_reg.AutoSize = true;
            this.label_reg.Location = new System.Drawing.Point(240, 87);
            this.label_reg.Name = "label_reg";
            this.label_reg.Size = new System.Drawing.Size(0, 13);
            this.label_reg.TabIndex = 3;
            // 
            // button_use_table
            // 
            this.button_use_table.Enabled = false;
            this.button_use_table.Location = new System.Drawing.Point(443, 46);
            this.button_use_table.Name = "button_use_table";
            this.button_use_table.Size = new System.Drawing.Size(83, 23);
            this.button_use_table.TabIndex = 4;
            this.button_use_table.Text = "Занять";
            this.button_use_table.UseVisualStyleBackColor = true;
            this.button_use_table.Click += new System.EventHandler(this.button_use_table_Click);
            // 
            // button_dismiss_table
            // 
            this.button_dismiss_table.Enabled = false;
            this.button_dismiss_table.Location = new System.Drawing.Point(443, 77);
            this.button_dismiss_table.Name = "button_dismiss_table";
            this.button_dismiss_table.Size = new System.Drawing.Size(83, 23);
            this.button_dismiss_table.TabIndex = 5;
            this.button_dismiss_table.Text = "Освободить";
            this.button_dismiss_table.UseVisualStyleBackColor = true;
            this.button_dismiss_table.Click += new System.EventHandler(this.button_dismiss_table_Click);
            // 
            // comboBox_empty
            // 
            this.comboBox_empty.FormattingEnabled = true;
            this.comboBox_empty.Location = new System.Drawing.Point(104, 46);
            this.comboBox_empty.Name = "comboBox_empty";
            this.comboBox_empty.Size = new System.Drawing.Size(322, 21);
            this.comboBox_empty.TabIndex = 6;
            this.comboBox_empty.SelectedIndexChanged += new System.EventHandler(this.comboBox_empty_SelectedIndexChanged);
            // 
            // comboBox_busy
            // 
            this.comboBox_busy.FormattingEnabled = true;
            this.comboBox_busy.Location = new System.Drawing.Point(104, 79);
            this.comboBox_busy.Name = "comboBox_busy";
            this.comboBox_busy.Size = new System.Drawing.Size(322, 21);
            this.comboBox_busy.TabIndex = 7;
            this.comboBox_busy.SelectedIndexChanged += new System.EventHandler(this.comboBox_busy_SelectedIndexChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(547, 24);
            this.menuStrip1.TabIndex = 8;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.настройкиToolStripMenuItem});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // настройкиToolStripMenuItem
            // 
            this.настройкиToolStripMenuItem.Name = "настройкиToolStripMenuItem";
            this.настройкиToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.настройкиToolStripMenuItem.Text = "Настройки";
            this.настройкиToolStripMenuItem.Click += new System.EventHandler(this.настройкиToolStripMenuItem_Click);
            // 
            // button_refresh
            // 
            this.button_refresh.Location = new System.Drawing.Point(104, 115);
            this.button_refresh.Name = "button_refresh";
            this.button_refresh.Size = new System.Drawing.Size(322, 23);
            this.button_refresh.TabIndex = 9;
            this.button_refresh.Text = "Обновить";
            this.button_refresh.UseVisualStyleBackColor = true;
            this.button_refresh.Click += new System.EventHandler(this.button_refresh_Click);
            // 
            // Form_main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(547, 159);
            this.Controls.Add(this.button_refresh);
            this.Controls.Add(this.comboBox_busy);
            this.Controls.Add(this.comboBox_empty);
            this.Controls.Add(this.button_dismiss_table);
            this.Controls.Add(this.button_use_table);
            this.Controls.Add(this.label_reg);
            this.Controls.Add(this.label_peyments);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Form_main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Semaphore";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_main_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label_peyments;
        private System.Windows.Forms.Label label_reg;
        private System.Windows.Forms.Button button_use_table;
        private System.Windows.Forms.Button button_dismiss_table;
        private System.Windows.Forms.ComboBox comboBox_empty;
        private System.Windows.Forms.ComboBox comboBox_busy;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem настройкиToolStripMenuItem;
        private System.Windows.Forms.Button button_refresh;
    }
}

