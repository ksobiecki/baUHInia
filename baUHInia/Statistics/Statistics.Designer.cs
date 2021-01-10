
namespace baUHInia.Statistics
{
    partial class Statistics
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
            this.statsView = new System.Windows.Forms.DataGridView();
            this.mapSelectBox = new System.Windows.Forms.ComboBox();
            this.userSelectBox = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.MapColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UserColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ScoreColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.statsView)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statsView
            // 
            this.statsView.AllowUserToAddRows = false;
            this.statsView.AllowUserToDeleteRows = false;
            this.statsView.AllowUserToResizeColumns = false;
            this.statsView.BackgroundColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.statsView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.statsView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MapColumn,
            this.UserColumn,
            this.ScoreColumn});
            this.statsView.Location = new System.Drawing.Point(50, 163);
            this.statsView.MultiSelect = false;
            this.statsView.Name = "statsView";
            this.statsView.ReadOnly = true;
            this.statsView.RowHeadersWidth = 51;
            this.statsView.RowTemplate.Height = 24;
            this.statsView.Size = new System.Drawing.Size(700, 232);
            this.statsView.TabIndex = 0;
            // 
            // mapSelectBox
            // 
            this.mapSelectBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.mapSelectBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.mapSelectBox.BackColor = System.Drawing.SystemColors.Window;
            this.mapSelectBox.CausesValidation = false;
            this.mapSelectBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.mapSelectBox.FormattingEnabled = true;
            this.mapSelectBox.Location = new System.Drawing.Point(45, 49);
            this.mapSelectBox.Name = "mapSelectBox";
            this.mapSelectBox.Size = new System.Drawing.Size(300, 24);
            this.mapSelectBox.TabIndex = 1;
            this.mapSelectBox.SelectedIndexChanged += new System.EventHandler(this.mapSelectBox_SelectedIndexChanged);
            // 
            // userSelectBox
            // 
            this.userSelectBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.userSelectBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.userSelectBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.userSelectBox.FormattingEnabled = true;
            this.userSelectBox.Location = new System.Drawing.Point(351, 49);
            this.userSelectBox.Name = "userSelectBox";
            this.userSelectBox.Size = new System.Drawing.Size(300, 24);
            this.userSelectBox.TabIndex = 2;
            this.userSelectBox.SelectedIndexChanged += new System.EventHandler(this.userSelectBox_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.mapSelectBox);
            this.groupBox1.Controls.Add(this.userSelectBox);
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox1.Location = new System.Drawing.Point(50, 28);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(700, 100);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(463, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Użytkownicy";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(176, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Mapy";
            // 
            // MapColumn
            // 
            this.MapColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.MapColumn.HeaderText = "    Nazwa Mapy";
            this.MapColumn.MinimumWidth = 100;
            this.MapColumn.Name = "MapColumn";
            this.MapColumn.ReadOnly = true;
            // 
            // UserColumn
            // 
            this.UserColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.UserColumn.HeaderText = "    Nazwa Użytkownika";
            this.UserColumn.MinimumWidth = 100;
            this.UserColumn.Name = "UserColumn";
            this.UserColumn.ReadOnly = true;
            // 
            // ScoreColumn
            // 
            this.ScoreColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ScoreColumn.HeaderText = "    Wynik";
            this.ScoreColumn.MinimumWidth = 100;
            this.ScoreColumn.Name = "ScoreColumn";
            this.ScoreColumn.ReadOnly = true;
            // 
            // Statistics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(802, 453);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.statsView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Statistics";
            this.Text = "Statystyki";
            this.Load += new System.EventHandler(this.Statistics_Load);
            ((System.ComponentModel.ISupportInitialize)(this.statsView)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView statsView;
        private System.Windows.Forms.ComboBox mapSelectBox;
        private System.Windows.Forms.ComboBox userSelectBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewTextBoxColumn MapColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ScoreColumn;
    }
}