
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
            this.IAUCheckBox = new System.Windows.Forms.CheckBox();
            this.SdbxCheckBox = new System.Windows.Forms.CheckBox();
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
            this.StarChart.Location = new System.Drawing.Point(249, 12);
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
            this.StarListTreeView.Location = new System.Drawing.Point(11, 92);
            this.StarListTreeView.Name = "StarListTreeView";
            this.StarListTreeView.Size = new System.Drawing.Size(224, 314);
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
            this.NextButton.Location = new System.Drawing.Point(192, 10);
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
            this.StepCheckBox.Location = new System.Drawing.Point(192, 52);
            this.StepCheckBox.Name = "StepCheckBox";
            this.StepCheckBox.Size = new System.Drawing.Size(48, 17);
            this.StepCheckBox.TabIndex = 6;
            this.StepCheckBox.Text = "Step";
            this.StepCheckBox.UseVisualStyleBackColor = true;
            // 
            // RefTextBox
            // 
            this.RefTextBox.Location = new System.Drawing.Point(115, 12);
            this.RefTextBox.Name = "RefTextBox";
            this.RefTextBox.Size = new System.Drawing.Size(71, 20);
            this.RefTextBox.TabIndex = 7;
            this.RefTextBox.Text = "Gaia";
            // 
            // SkipMagCheckBox
            // 
            this.SkipMagCheckBox.AutoSize = true;
            this.SkipMagCheckBox.ForeColor = System.Drawing.Color.White;
            this.SkipMagCheckBox.Location = new System.Drawing.Point(115, 69);
            this.SkipMagCheckBox.Name = "SkipMagCheckBox";
            this.SkipMagCheckBox.Size = new System.Drawing.Size(114, 17);
            this.SkipMagCheckBox.TabIndex = 8;
            this.SkipMagCheckBox.Text = "Ignore Magnitudes";
            this.SkipMagCheckBox.UseVisualStyleBackColor = true;
            // 
            // OnTopCheckBox
            // 
            this.OnTopCheckBox.AutoSize = true;
            this.OnTopCheckBox.ForeColor = System.Drawing.Color.White;
            this.OnTopCheckBox.Location = new System.Drawing.Point(30, 60);
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
            this.CloseButton.Location = new System.Drawing.Point(192, 412);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(43, 34);
            this.CloseButton.TabIndex = 11;
            this.CloseButton.Text = "Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // IAUCheckBox
            // 
            this.IAUCheckBox.AutoSize = true;
            this.IAUCheckBox.ForeColor = System.Drawing.Color.White;
            this.IAUCheckBox.Location = new System.Drawing.Point(115, 35);
            this.IAUCheckBox.Name = "IAUCheckBox";
            this.IAUCheckBox.Size = new System.Drawing.Size(44, 17);
            this.IAUCheckBox.TabIndex = 12;
            this.IAUCheckBox.Text = "IAU";
            this.IAUCheckBox.UseVisualStyleBackColor = true;
            this.IAUCheckBox.CheckedChanged += new System.EventHandler(this.IAUCheckBox_CheckedChanged);
            // 
            // SdbxCheckBox
            // 
            this.SdbxCheckBox.AutoSize = true;
            this.SdbxCheckBox.ForeColor = System.Drawing.Color.White;
            this.SdbxCheckBox.Location = new System.Drawing.Point(115, 51);
            this.SdbxCheckBox.Name = "SdbxCheckBox";
            this.SdbxCheckBox.Size = new System.Drawing.Size(55, 17);
            this.SdbxCheckBox.TabIndex = 13;
            this.SdbxCheckBox.Text = "SDBX";
            this.SdbxCheckBox.UseVisualStyleBackColor = true;
            this.SdbxCheckBox.CheckedChanged += new System.EventHandler(this.SdbxCheckBox_CheckedChanged);
            // 
            // FormCatalogPlot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Teal;
            this.ClientSize = new System.Drawing.Size(680, 455);
            this.Controls.Add(this.SdbxCheckBox);
            this.Controls.Add(this.IAUCheckBox);
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
            this.TopMost = true;
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
        private System.Windows.Forms.CheckBox IAUCheckBox;
        private System.Windows.Forms.CheckBox SdbxCheckBox;
    }
}

