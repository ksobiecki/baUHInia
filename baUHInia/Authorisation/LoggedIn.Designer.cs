namespace baUHInia.Authorisation
{
    partial class LoggedIn
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoggedIn));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.adminRadio = new System.Windows.Forms.RadioButton();
            this.userRadio = new System.Windows.Forms.RadioButton();
            this.loginTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.IDtextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.statsButton = new System.Windows.Forms.Button();
            this.boardButton = new System.Windows.Forms.Button();
            this.logoffButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.adminRadio);
            this.groupBox1.Controls.Add(this.userRadio);
            this.groupBox1.Controls.Add(this.loginTextBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.IDtextBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(214, 99);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // adminRadio
            // 
            this.adminRadio.AutoSize = true;
            this.adminRadio.Enabled = false;
            this.adminRadio.Location = new System.Drawing.Point(102, 72);
            this.adminRadio.Name = "adminRadio";
            this.adminRadio.Size = new System.Drawing.Size(97, 17);
            this.adminRadio.TabIndex = 5;
            this.adminRadio.TabStop = true;
            this.adminRadio.Text = "Władze Miasta";
            this.adminRadio.UseVisualStyleBackColor = true;
            // 
            // userRadio
            // 
            this.userRadio.AutoSize = true;
            this.userRadio.Enabled = false;
            this.userRadio.Location = new System.Drawing.Point(10, 72);
            this.userRadio.Name = "userRadio";
            this.userRadio.Size = new System.Drawing.Size(80, 17);
            this.userRadio.TabIndex = 4;
            this.userRadio.TabStop = true;
            this.userRadio.Text = "Użytkownik";
            this.userRadio.UseVisualStyleBackColor = true;
            // 
            // loginTextBox
            // 
            this.loginTextBox.Enabled = false;
            this.loginTextBox.Location = new System.Drawing.Point(102, 42);
            this.loginTextBox.Name = "loginTextBox";
            this.loginTextBox.Size = new System.Drawing.Size(100, 20);
            this.loginTextBox.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Login";
            // 
            // IDtextBox
            // 
            this.IDtextBox.Enabled = false;
            this.IDtextBox.Location = new System.Drawing.Point(102, 17);
            this.IDtextBox.Name = "IDtextBox";
            this.IDtextBox.Size = new System.Drawing.Size(100, 20);
            this.IDtextBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "ID Użytkownika";
            // 
            // statsButton
            // 
            this.statsButton.Location = new System.Drawing.Point(14, 118);
            this.statsButton.Name = "statsButton";
            this.statsButton.Size = new System.Drawing.Size(95, 23);
            this.statsButton.TabIndex = 1;
            this.statsButton.Text = "Statystyki";
            this.statsButton.UseVisualStyleBackColor = true;
            this.statsButton.Click += new System.EventHandler(this.statsButton_Click);
            // 
            // boardButton
            // 
            this.boardButton.Location = new System.Drawing.Point(115, 118);
            this.boardButton.Name = "boardButton";
            this.boardButton.Size = new System.Drawing.Size(112, 23);
            this.boardButton.TabIndex = 2;
            this.boardButton.Text = "Przejdź do planszy";
            this.boardButton.UseVisualStyleBackColor = true;
            this.boardButton.Click += new System.EventHandler(this.boardButton_Click);
            // 
            // logoffButton
            // 
            this.logoffButton.Location = new System.Drawing.Point(14, 148);
            this.logoffButton.Name = "logoffButton";
            this.logoffButton.Size = new System.Drawing.Size(213, 23);
            this.logoffButton.TabIndex = 3;
            this.logoffButton.Text = "Wyloguj się";
            this.logoffButton.UseVisualStyleBackColor = true;
            this.logoffButton.Click += new System.EventHandler(this.logoffButton_Click);
            // 
            // LoggedIn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(235, 182);
            this.Controls.Add(this.logoffButton);
            this.Controls.Add(this.boardButton);
            this.Controls.Add(this.statsButton);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoggedIn";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Zalogowano";
            this.Load += new System.EventHandler(this.LoggedIn_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton adminRadio;
        private System.Windows.Forms.RadioButton userRadio;
        private System.Windows.Forms.TextBox loginTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox IDtextBox;
        private System.Windows.Forms.Button statsButton;
        private System.Windows.Forms.Button boardButton;
        private System.Windows.Forms.Button logoffButton;
    }
}