
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
            this.MapColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UserColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ScoreColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mapSelectBox = new System.Windows.Forms.ComboBox();
            this.userSelectBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.statsView)).BeginInit();
            this.SuspendLayout();
            // 
            // statsView
            // 
            this.statsView.AllowUserToAddRows = false;
            this.statsView.AllowUserToDeleteRows = false;
            this.statsView.AllowUserToResizeColumns = false;
            this.statsView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.statsView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MapColumn,
            this.UserColumn,
            this.ScoreColumn});
            this.statsView.Location = new System.Drawing.Point(50, 163);
            this.statsView.Name = "statsView";
            this.statsView.ReadOnly = true;
            this.statsView.RowHeadersWidth = 51;
            this.statsView.RowTemplate.Height = 24;
            this.statsView.Size = new System.Drawing.Size(700, 232);
            this.statsView.TabIndex = 0;
            // 
            // MapColumn
            // 
            this.MapColumn.HeaderText = "Map Name";
            this.MapColumn.MinimumWidth = 100;
            this.MapColumn.Name = "MapColumn";
            this.MapColumn.ReadOnly = true;
            this.MapColumn.Width = 227;
            // 
            // UserColumn
            // 
            this.UserColumn.HeaderText = "Username";
            this.UserColumn.MinimumWidth = 100;
            this.UserColumn.Name = "UserColumn";
            this.UserColumn.ReadOnly = true;
            this.UserColumn.Width = 220;
            // 
            // ScoreColumn
            // 
            this.ScoreColumn.HeaderText = "Score";
            this.ScoreColumn.MinimumWidth = 100;
            this.ScoreColumn.Name = "ScoreColumn";
            this.ScoreColumn.ReadOnly = true;
            this.ScoreColumn.Width = 200;
            // 
            // mapSelectBox
            // 
            this.mapSelectBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.mapSelectBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.mapSelectBox.FormattingEnabled = true;
            this.mapSelectBox.Location = new System.Drawing.Point(250, 34);
            this.mapSelectBox.Name = "mapSelectBox";
            this.mapSelectBox.Size = new System.Drawing.Size(300, 24);
            this.mapSelectBox.TabIndex = 1;
            this.mapSelectBox.SelectedIndexChanged += new System.EventHandler(this.mapSelectBox_SelectedIndexChanged);
            // 
            // userSelectBox
            // 
            this.userSelectBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.userSelectBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.userSelectBox.FormattingEnabled = true;
            this.userSelectBox.Location = new System.Drawing.Point(250, 79);
            this.userSelectBox.Name = "userSelectBox";
            this.userSelectBox.Size = new System.Drawing.Size(300, 24);
            this.userSelectBox.TabIndex = 2;
            this.userSelectBox.SelectedIndexChanged += new System.EventHandler(this.userSelectBox_SelectedIndexChanged);
            // 
            // Statistics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(802, 453);
            this.Controls.Add(this.userSelectBox);
            this.Controls.Add(this.mapSelectBox);
            this.Controls.Add(this.statsView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Statistics";
            this.Text = "Statistics";
            this.Load += new System.EventHandler(this.Statistics_Load);
            ((System.ComponentModel.ISupportInitialize)(this.statsView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView statsView;
        private System.Windows.Forms.ComboBox mapSelectBox;
        private System.Windows.Forms.ComboBox userSelectBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn MapColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ScoreColumn;
    }
}