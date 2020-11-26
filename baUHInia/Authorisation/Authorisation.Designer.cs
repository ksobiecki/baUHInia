﻿namespace baUHInia.Authorisation
{
    partial class Authorisation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Authorisation));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.isAdmin = new System.Windows.Forms.CheckBox();
            this.registerSecondPasswordBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.registerPasswordBox = new System.Windows.Forms.TextBox();
            this.registerLoginBox = new System.Windows.Forms.TextBox();
            this.registerButton = new System.Windows.Forms.Button();
            this.loginButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.loginPasswordBox = new System.Windows.Forms.TextBox();
            this.loginLoginBox = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.isAdmin);
            this.groupBox2.Controls.Add(this.registerSecondPasswordBox);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.registerPasswordBox);
            this.groupBox2.Controls.Add(this.registerLoginBox);
            this.groupBox2.Controls.Add(this.registerButton);
            this.groupBox2.Location = new System.Drawing.Point(12, 148);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(153, 193);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Rejestracja";
            // 
            // isAdmin
            // 
            this.isAdmin.AutoSize = true;
            this.isAdmin.Location = new System.Drawing.Point(6, 139);
            this.isAdmin.Name = "isAdmin";
            this.isAdmin.Size = new System.Drawing.Size(97, 17);
            this.isAdmin.TabIndex = 8;
            this.isAdmin.Text = "Władze miasta";
            this.isAdmin.UseVisualStyleBackColor = true;
            // 
            // registerSecondPasswordBox
            // 
            this.registerSecondPasswordBox.Location = new System.Drawing.Point(6, 112);
            this.registerSecondPasswordBox.Name = "registerSecondPasswordBox";
            this.registerSecondPasswordBox.Size = new System.Drawing.Size(138, 20);
            this.registerSecondPasswordBox.TabIndex = 6;
            this.registerSecondPasswordBox.UseSystemPasswordChar = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 96);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Powtórz hasło";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Hasło";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Login";
            // 
            // registerPasswordBox
            // 
            this.registerPasswordBox.Location = new System.Drawing.Point(6, 73);
            this.registerPasswordBox.Name = "registerPasswordBox";
            this.registerPasswordBox.Size = new System.Drawing.Size(138, 20);
            this.registerPasswordBox.TabIndex = 5;
            this.registerPasswordBox.UseSystemPasswordChar = true;
            this.registerPasswordBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.registerPasswordBox_MouseClick);
            // 
            // registerLoginBox
            // 
            this.registerLoginBox.Location = new System.Drawing.Point(6, 34);
            this.registerLoginBox.Name = "registerLoginBox";
            this.registerLoginBox.Size = new System.Drawing.Size(138, 20);
            this.registerLoginBox.TabIndex = 4;
            this.registerLoginBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.registerLoginBox_MouseClick);
            // 
            // registerButton
            // 
            this.registerButton.Location = new System.Drawing.Point(6, 162);
            this.registerButton.Name = "registerButton";
            this.registerButton.Size = new System.Drawing.Size(86, 23);
            this.registerButton.TabIndex = 7;
            this.registerButton.Text = "Zarejestruj się";
            this.registerButton.UseVisualStyleBackColor = true;
            this.registerButton.Click += new System.EventHandler(this.registerButton_Click);
            // 
            // loginButton
            // 
            this.loginButton.Location = new System.Drawing.Point(6, 101);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(86, 23);
            this.loginButton.TabIndex = 3;
            this.loginButton.Text = "Zaloguj się";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.loginPasswordBox);
            this.groupBox1.Controls.Add(this.loginLoginBox);
            this.groupBox1.Controls.Add(this.loginButton);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(153, 130);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Logowanie";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Hasło";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 19);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Login";
            // 
            // loginPasswordBox
            // 
            this.loginPasswordBox.Location = new System.Drawing.Point(6, 74);
            this.loginPasswordBox.Name = "loginPasswordBox";
            this.loginPasswordBox.Size = new System.Drawing.Size(138, 20);
            this.loginPasswordBox.TabIndex = 2;
            this.loginPasswordBox.UseSystemPasswordChar = true;
            // 
            // loginLoginBox
            // 
            this.loginLoginBox.Location = new System.Drawing.Point(6, 35);
            this.loginLoginBox.Name = "loginLoginBox";
            this.loginLoginBox.Size = new System.Drawing.Size(138, 20);
            this.loginLoginBox.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::baUHInia.Properties.Resources.icon;
            this.pictureBox1.Location = new System.Drawing.Point(171, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(372, 329);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            // 
            // Authorisation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 349);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Authorisation";
            this.Text = "baUHInia - Autoryzacja";
            this.Load += new System.EventHandler(this.Authorisation_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox registerPasswordBox;
        private System.Windows.Forms.TextBox registerLoginBox;
        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.TextBox registerSecondPasswordBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox loginPasswordBox;
        private System.Windows.Forms.TextBox loginLoginBox;
        private System.Windows.Forms.Button registerButton;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.CheckBox isAdmin;
    }
}