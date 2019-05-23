namespace GUI
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_keyboard = new System.Windows.Forms.TextBox();
            this.tb_mouse = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_url = new System.Windows.Forms.TextBox();
            this.btn_start = new System.Windows.Forms.Button();
            this.btn_save = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.btn_stop = new System.Windows.Forms.Button();
            this.btn_read = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 26);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "键盘";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 52);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "鼠标";
            // 
            // tb_keyboard
            // 
            this.tb_keyboard.Location = new System.Drawing.Point(70, 23);
            this.tb_keyboard.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tb_keyboard.Name = "tb_keyboard";
            this.tb_keyboard.ReadOnly = true;
            this.tb_keyboard.Size = new System.Drawing.Size(159, 21);
            this.tb_keyboard.TabIndex = 2;
            // 
            // tb_mouse
            // 
            this.tb_mouse.Location = new System.Drawing.Point(70, 48);
            this.tb_mouse.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tb_mouse.Name = "tb_mouse";
            this.tb_mouse.ReadOnly = true;
            this.tb_mouse.Size = new System.Drawing.Size(159, 21);
            this.tb_mouse.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tb_mouse);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tb_keyboard);
            this.groupBox1.Location = new System.Drawing.Point(44, 94);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Size = new System.Drawing.Size(243, 80);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "实时状态";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(44, 54);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "目标url";
            // 
            // tb_url
            // 
            this.tb_url.Location = new System.Drawing.Point(107, 52);
            this.tb_url.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tb_url.Name = "tb_url";
            this.tb_url.Size = new System.Drawing.Size(381, 21);
            this.tb_url.TabIndex = 6;
            this.tb_url.Text = "http://localhost:8086/announce";
            // 
            // btn_start
            // 
            this.btn_start.Location = new System.Drawing.Point(342, 113);
            this.btn_start.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn_start.Name = "btn_start";
            this.btn_start.Size = new System.Drawing.Size(56, 18);
            this.btn_start.TabIndex = 7;
            this.btn_start.Text = "开始";
            this.btn_start.UseVisualStyleBackColor = true;
            this.btn_start.Click += new System.EventHandler(this.button1_Click);
            // 
            // btn_save
            // 
            this.btn_save.Location = new System.Drawing.Point(431, 113);
            this.btn_save.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(56, 18);
            this.btn_save.TabIndex = 8;
            this.btn_save.Text = "保存记录";
            this.btn_save.UseVisualStyleBackColor = true;
            // 
            // btn_stop
            // 
            this.btn_stop.Location = new System.Drawing.Point(342, 146);
            this.btn_stop.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn_stop.Name = "btn_stop";
            this.btn_stop.Size = new System.Drawing.Size(56, 18);
            this.btn_stop.TabIndex = 9;
            this.btn_stop.Text = "停止";
            this.btn_stop.UseVisualStyleBackColor = true;
            this.btn_stop.Click += new System.EventHandler(this.btn_stop_Click);
            // 
            // btn_read
            // 
            this.btn_read.Location = new System.Drawing.Point(431, 146);
            this.btn_read.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn_read.Name = "btn_read";
            this.btn_read.Size = new System.Drawing.Size(56, 18);
            this.btn_read.TabIndex = 10;
            this.btn_read.Text = "读取记录";
            this.btn_read.UseVisualStyleBackColor = true;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(561, 222);
            this.Controls.Add(this.btn_read);
            this.Controls.Add(this.btn_stop);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.btn_start);
            this.Controls.Add(this.tb_url);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form1";
            this.Text = "键鼠操作监控程序";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox tb_keyboard;
        public System.Windows.Forms.TextBox tb_mouse;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb_url;
        private System.Windows.Forms.Button btn_start;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button btn_stop;
        private System.Windows.Forms.Button btn_read;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}

