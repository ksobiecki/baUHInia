namespace baUHInia.Authorisation
{
    partial class NewPassword
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
            this.newPasswordBox = new System.Windows.Forms.TextBox();
            this.changePasswdBttn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // newPasswordBox
            // 
            this.newPasswordBox.Location = new System.Drawing.Point(13, 12);
            this.newPasswordBox.Name = "newPasswordBox";
            this.newPasswordBox.Size = new System.Drawing.Size(160, 20);
            this.newPasswordBox.TabIndex = 1;
            this.newPasswordBox.Enter += new System.EventHandler(this.newPasswordTextClicked);
            this.newPasswordBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.form_KeyDown);
            // 
            // changePasswdBttn
            // 
            this.changePasswdBttn.Location = new System.Drawing.Point(13, 38);
            this.changePasswdBttn.Name = "changePasswdBttn";
            this.changePasswdBttn.Size = new System.Drawing.Size(160, 23);
            this.changePasswdBttn.TabIndex = 2;
            this.changePasswdBttn.Text = "Zmień hasło";
            this.changePasswdBttn.UseVisualStyleBackColor = true;
            // 
            // NewPassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(185, 72);
            this.ControlBox = false;
            this.Controls.Add(this.changePasswdBttn);
            this.Controls.Add(this.newPasswordBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "NewPassword";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Podaj nowe hasło";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button changePasswdBttn;
        public System.Windows.Forms.TextBox newPasswordBox;
    }
}