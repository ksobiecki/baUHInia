namespace baUHInia.Simulation
{
    partial class ScoreTemperature
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
            this.ScoreTempTitle = new System.Windows.Forms.Label();
            this.ScoreTempMessage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ScoreTempTitle
            // 
            this.ScoreTempTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ScoreTempTitle.AutoSize = true;
            this.ScoreTempTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ScoreTempTitle.Location = new System.Drawing.Point(57, 9);
            this.ScoreTempTitle.Name = "ScoreTempTitle";
            this.ScoreTempTitle.Size = new System.Drawing.Size(267, 29);
            this.ScoreTempTitle.TabIndex = 0;
            this.ScoreTempTitle.Text = "Symulacja zakończona !";
            // 
            // ScoreTempMessage
            // 
            this.ScoreTempMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ScoreTempMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ScoreTempMessage.Location = new System.Drawing.Point(0, 51);
            this.ScoreTempMessage.Name = "ScoreTempMessage";
            this.ScoreTempMessage.Padding = new System.Windows.Forms.Padding(20, 0, 20, 5);
            this.ScoreTempMessage.Size = new System.Drawing.Size(391, 117);
            this.ScoreTempMessage.TabIndex = 1;
            this.ScoreTempMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ScoreTemperature
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(391, 168);
            this.Controls.Add(this.ScoreTempMessage);
            this.Controls.Add(this.ScoreTempTitle);
            this.Name = "ScoreTemperature";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ScoreTempTitle;
        private System.Windows.Forms.Label ScoreTempMessage;
    }
}