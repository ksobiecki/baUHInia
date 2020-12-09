namespace baUHInia.Authorisation
{
    partial class AddQuestion
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
            this.sendQuestionBttn = new System.Windows.Forms.Button();
            this.answerBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.questionBox = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.sendQuestionBttn);
            this.groupBox1.Controls.Add(this.answerBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.questionBox);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(250, 138);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Pytanie pomocnicze";
            // 
            // sendQuestionBttn
            // 
            this.sendQuestionBttn.Location = new System.Drawing.Point(7, 107);
            this.sendQuestionBttn.Name = "sendQuestionBttn";
            this.sendQuestionBttn.Size = new System.Drawing.Size(75, 23);
            this.sendQuestionBttn.TabIndex = 4;
            this.sendQuestionBttn.Text = "Wyślij";
            this.sendQuestionBttn.UseVisualStyleBackColor = true;
            // 
            // answerBox
            // 
            this.answerBox.Location = new System.Drawing.Point(7, 80);
            this.answerBox.Name = "answerBox";
            this.answerBox.Size = new System.Drawing.Size(232, 20);
            this.answerBox.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(224, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Wprowadź odpowiedź na pytanie pomocnicze";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(155, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Wprowadź pytanie pomocnicze";
            // 
            // questionBox
            // 
            this.questionBox.Location = new System.Drawing.Point(6, 36);
            this.questionBox.Name = "questionBox";
            this.questionBox.Size = new System.Drawing.Size(233, 20);
            this.questionBox.TabIndex = 0;
            // 
            // AddQuestion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(269, 159);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddQuestion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dodaj pytanie pomocnicze";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button sendQuestionBttn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox answerBox;
        public System.Windows.Forms.TextBox questionBox;
    }
}