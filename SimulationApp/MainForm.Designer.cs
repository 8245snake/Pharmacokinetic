namespace SimulationApp
{
    partial class MainForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chartSimulation = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.combMedicine = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.chartSimulation)).BeginInit();
            this.SuspendLayout();
            // 
            // chartSimulation
            // 
            this.chartSimulation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chartSimulation.BorderlineColor = System.Drawing.Color.Black;
            chartArea4.Name = "ChartArea1";
            this.chartSimulation.ChartAreas.Add(chartArea4);
            legend4.Name = "Legend1";
            this.chartSimulation.Legends.Add(legend4);
            this.chartSimulation.Location = new System.Drawing.Point(12, 67);
            this.chartSimulation.Name = "chartSimulation";
            series4.ChartArea = "ChartArea1";
            series4.Legend = "Legend1";
            series4.Name = "Series1";
            this.chartSimulation.Series.Add(series4);
            this.chartSimulation.Size = new System.Drawing.Size(776, 371);
            this.chartSimulation.TabIndex = 0;
            this.chartSimulation.Text = "chart1";
            // 
            // combMedicine
            // 
            this.combMedicine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combMedicine.FormattingEnabled = true;
            this.combMedicine.Location = new System.Drawing.Point(12, 28);
            this.combMedicine.Name = "combMedicine";
            this.combMedicine.Size = new System.Drawing.Size(121, 20);
            this.combMedicine.TabIndex = 1;
            this.combMedicine.SelectedIndexChanged += new System.EventHandler(this.combMedicine_SelectedIndexChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.combMedicine);
            this.Controls.Add(this.chartSimulation);
            this.Name = "MainForm";
            this.Text = "薬物動態シミュレーション";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chartSimulation)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chartSimulation;
        private System.Windows.Forms.ComboBox combMedicine;
    }
}

