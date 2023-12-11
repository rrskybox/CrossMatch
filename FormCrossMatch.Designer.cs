
namespace CrossMatch
{
    partial class FormCatalogPlot
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.MapButton = new System.Windows.Forms.Button();
            this.StarChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.StarListTreeView = new System.Windows.Forms.TreeView();
            this.ListButton = new System.Windows.Forms.Button();
            this.StarListFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.NextButton = new System.Windows.Forms.Button();
            this.StepCheckBox = new System.Windows.Forms.CheckBox();
            this.RefTextBox = new System.Windows.Forms.TextBox();
            this.SkipMagCheckBox = new System.Windows.Forms.CheckBox();
            this.OnTopCheckBox = new System.Windows.Forms.CheckBox();
            this.AbortButton = new System.Windows.Forms.Button();
            this.CloseButton = new System.Windows.Forms.Button();
            this.HeaderChoiceBox = new System.Windows.Forms.ComboBox();
            this.UpdateCheckbox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SkipBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.StarChart)).BeginInit();
            this.SuspendLayout();
            // 
            // MapButton
            // 
            this.MapButton.Location = new System.Drawing.Point(12, 10);
            this.MapButton.Name = "MapButton";
            this.MapButton.Size = new System.Drawing.Size(45, 34);
            this.MapButton.TabIndex = 1;
            this.MapButton.Text = "Map";
            this.MapButton.UseVisualStyleBackColor = true;
            this.MapButton.Click += new System.EventHandler(this.FindButton_Click);
            // 
            // StarChart
            // 
            chartArea1.AxisX.IsStartedFromZero = false;
            chartArea1.AxisX.Title = "RA (arc sec)";
            chartArea1.AxisY.IsStartedFromZero = false;
            chartArea1.AxisY.Title = "Dec (arc sec)";
            chartArea1.Name = "ChartArea1";
            this.StarChart.ChartAreas.Add(chartArea1);
            this.StarChart.Location = new System.Drawing.Point(306, 12);
            this.StarChart.Name = "StarChart";
            series1.BackSecondaryColor = System.Drawing.Color.White;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            series1.MarkerBorderColor = System.Drawing.Color.Black;
            series1.MarkerColor = System.Drawing.Color.DeepSkyBlue;
            series1.MarkerImageTransparentColor = System.Drawing.Color.Transparent;
            series1.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Star4;
            series1.Name = "Series1";
            series1.YValuesPerPoint = 2;
            this.StarChart.Series.Add(series1);
            this.StarChart.Size = new System.Drawing.Size(417, 425);
            this.StarChart.TabIndex = 2;
            this.StarChart.Text = "chart1";
            // 
            // StarListTreeView
            // 
            this.StarListTreeView.Location = new System.Drawing.Point(14, 107);
            this.StarListTreeView.Name = "StarListTreeView";
            this.StarListTreeView.Size = new System.Drawing.Size(274, 299);
            this.StarListTreeView.TabIndex = 3;
            // 
            // ListButton
            // 
            this.ListButton.Location = new System.Drawing.Point(63, 10);
            this.ListButton.Name = "ListButton";
            this.ListButton.Size = new System.Drawing.Size(43, 34);
            this.ListButton.TabIndex = 4;
            this.ListButton.Text = "List";
            this.ListButton.UseVisualStyleBackColor = true;
            this.ListButton.Click += new System.EventHandler(this.ListButton_Click);
            // 
            // NextButton
            // 
            this.NextButton.BackColor = System.Drawing.Color.Gray;
            this.NextButton.Location = new System.Drawing.Point(245, 60);
            this.NextButton.Name = "NextButton";
            this.NextButton.Size = new System.Drawing.Size(43, 36);
            this.NextButton.TabIndex = 5;
            this.NextButton.Text = "Next";
            this.NextButton.UseVisualStyleBackColor = false;
            this.NextButton.Click += new System.EventHandler(this.StepButton_Click);
            // 
            // StepCheckBox
            // 
            this.StepCheckBox.AutoSize = true;
            this.StepCheckBox.ForeColor = System.Drawing.Color.White;
            this.StepCheckBox.Location = new System.Drawing.Point(124, 10);
            this.StepCheckBox.Name = "StepCheckBox";
            this.StepCheckBox.Size = new System.Drawing.Size(48, 17);
            this.StepCheckBox.TabIndex = 6;
            this.StepCheckBox.Text = "Step";
            this.StepCheckBox.UseVisualStyleBackColor = true;
            // 
            // RefTextBox
            // 
            this.RefTextBox.Location = new System.Drawing.Point(13, 68);
            this.RefTextBox.Name = "RefTextBox";
            this.RefTextBox.Size = new System.Drawing.Size(71, 20);
            this.RefTextBox.TabIndex = 7;
            this.RefTextBox.Text = "Gaia";
            // 
            // SkipMagCheckBox
            // 
            this.SkipMagCheckBox.AutoSize = true;
            this.SkipMagCheckBox.ForeColor = System.Drawing.Color.White;
            this.SkipMagCheckBox.Location = new System.Drawing.Point(184, 10);
            this.SkipMagCheckBox.Name = "SkipMagCheckBox";
            this.SkipMagCheckBox.Size = new System.Drawing.Size(114, 17);
            this.SkipMagCheckBox.TabIndex = 8;
            this.SkipMagCheckBox.Text = "Ignore Magnitudes";
            this.SkipMagCheckBox.UseVisualStyleBackColor = true;
            // 
            // OnTopCheckBox
            // 
            this.OnTopCheckBox.AutoSize = true;
            this.OnTopCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.OnTopCheckBox.ForeColor = System.Drawing.Color.White;
            this.OnTopCheckBox.Location = new System.Drawing.Point(60, 422);
            this.OnTopCheckBox.Name = "OnTopCheckBox";
            this.OnTopCheckBox.Size = new System.Drawing.Size(62, 17);
            this.OnTopCheckBox.TabIndex = 9;
            this.OnTopCheckBox.Text = "On Top";
            this.OnTopCheckBox.UseVisualStyleBackColor = true;
            this.OnTopCheckBox.CheckedChanged += new System.EventHandler(this.OnTopCheckBox_CheckedChanged);
            // 
            // AbortButton
            // 
            this.AbortButton.Location = new System.Drawing.Point(12, 412);
            this.AbortButton.Name = "AbortButton";
            this.AbortButton.Size = new System.Drawing.Size(43, 34);
            this.AbortButton.TabIndex = 10;
            this.AbortButton.Text = "Abort";
            this.AbortButton.UseVisualStyleBackColor = true;
            this.AbortButton.Click += new System.EventHandler(this.AbortButton_Click);
            // 
            // CloseButton
            // 
            this.CloseButton.Location = new System.Drawing.Point(245, 417);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(43, 34);
            this.CloseButton.TabIndex = 11;
            this.CloseButton.Text = "Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // HeaderChoiceBox
            // 
            this.HeaderChoiceBox.FormattingEnabled = true;
            this.HeaderChoiceBox.Items.AddRange(new object[] {
            "Generic",
            "IAU",
            "SDBX"});
            this.HeaderChoiceBox.Location = new System.Drawing.Point(107, 68);
            this.HeaderChoiceBox.Name = "HeaderChoiceBox";
            this.HeaderChoiceBox.Size = new System.Drawing.Size(71, 21);
            this.HeaderChoiceBox.TabIndex = 14;
            // 
            // UpdateCheckbox
            // 
            this.UpdateCheckbox.AutoSize = true;
            this.UpdateCheckbox.ForeColor = System.Drawing.Color.White;
            this.UpdateCheckbox.Location = new System.Drawing.Point(184, 28);
            this.UpdateCheckbox.Name = "UpdateCheckbox";
            this.UpdateCheckbox.Size = new System.Drawing.Size(104, 17);
            this.UpdateCheckbox.TabIndex = 15;
            this.UpdateCheckbox.Text = "Update RA/Dec";
            this.UpdateCheckbox.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(10, 91);
            this.label1.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Reference Star Listing";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(105, 52);
            this.label2.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "SDB Header";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(11, 50);
            this.label3.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "Reference To:";
            // 
            // SkipBox
            // 
            this.SkipBox.AutoSize = true;
            this.SkipBox.ForeColor = System.Drawing.Color.White;
            this.SkipBox.Location = new System.Drawing.Point(124, 28);
            this.SkipBox.Name = "SkipBox";
            this.SkipBox.Size = new System.Drawing.Size(47, 17);
            this.SkipBox.TabIndex = 19;
            this.SkipBox.Text = "Skip";
            this.SkipBox.UseVisualStyleBackColor = true;
            // 
            // FormCatalogPlot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Teal;
            this.ClientSize = new System.Drawing.Size(735, 463);
            this.Controls.Add(this.SkipBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.UpdateCheckbox);
            this.Controls.Add(this.HeaderChoiceBox);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.AbortButton);
            this.Controls.Add(this.OnTopCheckBox);
            this.Controls.Add(this.SkipMagCheckBox);
            this.Controls.Add(this.RefTextBox);
            this.Controls.Add(this.StepCheckBox);
            this.Controls.Add(this.NextButton);
            this.Controls.Add(this.ListButton);
            this.Controls.Add(this.StarListTreeView);
            this.Controls.Add(this.StarChart);
            this.Controls.Add(this.MapButton);
            this.ForeColor = System.Drawing.Color.Black;
            this.Name = "FormCatalogPlot";
            this.Text = "Catalog Plot";
            ((System.ComponentModel.ISupportInitialize)(this.StarChart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button MapButton;
        private System.Windows.Forms.DataVisualization.Charting.Chart StarChart;
        private System.Windows.Forms.TreeView StarListTreeView;
        private System.Windows.Forms.Button ListButton;
        private System.Windows.Forms.OpenFileDialog StarListFileDialog;
        private System.Windows.Forms.Button NextButton;
        private System.Windows.Forms.CheckBox StepCheckBox;
        private System.Windows.Forms.TextBox RefTextBox;
        private System.Windows.Forms.CheckBox SkipMagCheckBox;
        private System.Windows.Forms.CheckBox OnTopCheckBox;
        private System.Windows.Forms.Button AbortButton;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.ComboBox HeaderChoiceBox;
        private System.Windows.Forms.CheckBox UpdateCheckbox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox SkipBox;
    }
}

