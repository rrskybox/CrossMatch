
namespace GaiaReferral
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.MapButton = new System.Windows.Forms.Button();
            this.StarChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.StarListTreeView = new System.Windows.Forms.TreeView();
            this.ListButton = new System.Windows.Forms.Button();
            this.StarListFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.NextButton = new System.Windows.Forms.Button();
            this.StepCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.StarChart)).BeginInit();
            this.SuspendLayout();
            // 
            // MapButton
            // 
            this.MapButton.Location = new System.Drawing.Point(12, 21);
            this.MapButton.Name = "MapButton";
            this.MapButton.Size = new System.Drawing.Size(45, 36);
            this.MapButton.TabIndex = 1;
            this.MapButton.Text = "Map";
            this.MapButton.UseVisualStyleBackColor = true;
            this.MapButton.Click += new System.EventHandler(this.FindButton_Click);
            // 
            // StarChart
            // 
            chartArea2.AxisX.IsStartedFromZero = false;
            chartArea2.AxisX.Title = "RA (arc sec)";
            chartArea2.AxisY.IsStartedFromZero = false;
            chartArea2.AxisY.Title = "Dec (arc sec)";
            chartArea2.Name = "ChartArea1";
            this.StarChart.ChartAreas.Add(chartArea2);
            this.StarChart.Location = new System.Drawing.Point(249, 21);
            this.StarChart.Name = "StarChart";
            series2.BackSecondaryColor = System.Drawing.Color.White;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            series2.MarkerBorderColor = System.Drawing.Color.Black;
            series2.MarkerColor = System.Drawing.Color.DeepSkyBlue;
            series2.MarkerImageTransparentColor = System.Drawing.Color.Transparent;
            series2.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Star4;
            series2.Name = "Series1";
            series2.YValuesPerPoint = 2;
            this.StarChart.Series.Add(series2);
            this.StarChart.Size = new System.Drawing.Size(417, 416);
            this.StarChart.TabIndex = 2;
            this.StarChart.Text = "chart1";
            // 
            // StarListTreeView
            // 
            this.StarListTreeView.Location = new System.Drawing.Point(11, 71);
            this.StarListTreeView.Name = "StarListTreeView";
            this.StarListTreeView.Size = new System.Drawing.Size(224, 365);
            this.StarListTreeView.TabIndex = 3;
            // 
            // ListButton
            // 
            this.ListButton.Location = new System.Drawing.Point(63, 21);
            this.ListButton.Name = "ListButton";
            this.ListButton.Size = new System.Drawing.Size(43, 36);
            this.ListButton.TabIndex = 4;
            this.ListButton.Text = "List";
            this.ListButton.UseVisualStyleBackColor = true;
            this.ListButton.Click += new System.EventHandler(this.ListButton_Click);
            // 
            // NextButton
            // 
            this.NextButton.BackColor = System.Drawing.Color.Gray;
            this.NextButton.Location = new System.Drawing.Point(192, 21);
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
            this.StepCheckBox.Location = new System.Drawing.Point(138, 32);
            this.StepCheckBox.Name = "StepCheckBox";
            this.StepCheckBox.Size = new System.Drawing.Size(48, 17);
            this.StepCheckBox.TabIndex = 6;
            this.StepCheckBox.Text = "Step";
            this.StepCheckBox.UseVisualStyleBackColor = true;
            // 
            // FormCatalogPlot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Teal;
            this.ClientSize = new System.Drawing.Size(688, 450);
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
    }
}

