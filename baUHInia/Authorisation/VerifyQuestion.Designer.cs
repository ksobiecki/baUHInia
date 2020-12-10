namespace baUHInia.Authorisation
{
    partial class VerifyQuestion
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.verifyAnswerBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.verifyQuestionBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.verifyQuestionBttn = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.verifyAnswerBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.verifyQuestionBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.verifyQuestionBttn);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(244, 134);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Pytanie pomocnicze";
            // 
            // verifyAnswerBox
            // 
            this.verifyAnswerBox.Location = new System.Drawing.Point(10, 81);
            this.verifyAnswerBox.Name = "verifyAnswerBox";
            this.verifyAnswerBox.Size = new System.Drawing.Size(228, 20);
            this.verifyAnswerBox.TabIndex = 4;
            this.verifyAnswerBox.Enter += new System.EventHandler(this.newQuestionTextClicked);
            this.verifyAnswerBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.form_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(224, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Wprowadź odpowiedź na pytanie pomocnicze";
            // 
            // verifyQuestionBox
            // 
            this.verifyQuestionBox.Enabled = false;
            this.verifyQuestionBox.Location = new System.Drawing.Point(10, 37);
            this.verifyQuestionBox.Name = "verifyQuestionBox";
            this.verifyQuestionBox.Size = new System.Drawing.Size(228, 20);
            this.verifyQuestionBox.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Twoje pytanie pomocnicze";
            // 
            // verifyQuestionBttn
            // 
            this.verifyQuestionBttn.Location = new System.Drawing.Point(10, 107);
            this.verifyQuestionBttn.Name = "verifyQuestionBttn";
            this.verifyQuestionBttn.Size = new System.Drawing.Size(75, 23);
            this.verifyQuestionBttn.TabIndex = 0;
            this.verifyQuestionBttn.Text = "Wyślij";
            this.verifyQuestionBttn.UseVisualStyleBackColor = true;
            // 
            // VerifyQuestion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(269, 159);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VerifyQuestion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Odpowiedz na pytanie pomocnicze";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button verifyQuestionBttn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox verifyAnswerBox;
        public System.Windows.Forms.TextBox verifyQuestionBox;
    }
}