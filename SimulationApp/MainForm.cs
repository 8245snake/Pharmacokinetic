using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Simulator;
using static Simulator.Dosing.Medicine;

namespace SimulationApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            var time = new DateTime(2021, 6, 12, 12, 20, 0);

            // プロポフォールのモデルでシミュレーション開始
            var f = new EleveldModelFactory(50, 1.5, 40, 2200, false, true);
            var model1 = f.Create("ﾌﾟﾛﾎﾟﾌｫｰﾙ_動脈", EleveldModelFactory.BloodVessels.Arterial);
            var model2 = PharmacokineticModelFactory.CreatePropofol(50);
            PharmacokineticSimulator sim = new PharmacokineticSimulator(time, 1, 30);
            sim.BolusDose(time, 100, WeightUnitEnum.mg, 20);
            LoadGraph(chartSimulation, sim, model1, model2);
        }

        private void LoadGraph(Chart chart, PharmacokineticSimulator simulator,  PharmacokineticModel model1, PharmacokineticModel model2)
        {
            Series seriesC1Model1 = CreateSeries($"血中濃度({model1.Name})", Color.Red);
            Series seriesCeModel1 = CreateSeries($"効果部位濃度({model1.Name})", Color.Orange);

            foreach (var result in simulator.Predict(model1))
            {
                seriesC1Model1.Points.Add(new DataPoint(result.PlotTime.ToOADate(), result.C1));
                seriesCeModel1.Points.Add(new DataPoint(result.PlotTime.ToOADate(), result.Ce));
            }

            Series seriesC1Model2 = CreateSeries($"血中濃度({model2.Name})", Color.Blue);
            Series seriesCeModel2 = CreateSeries($"効果部位濃度({model2.Name})", Color.LightSkyBlue);

            foreach (var result in simulator.Predict(model2))
            {
                seriesC1Model2.Points.Add(new DataPoint(result.PlotTime.ToOADate(), result.C1));
                seriesCeModel2.Points.Add(new DataPoint(result.PlotTime.ToOADate(), result.Ce));
            }

            // chartarea
            ChartArea area1 = new ChartArea();
            area1.AxisX.Title = "時刻";
            area1.AxisY.Title = "濃度(ng/ml)";

            area1.AxisX.LabelStyle.Format = "HH:mm";
            area1.AxisX.Interval = 1;
            area1.AxisX.IntervalType = DateTimeIntervalType.Minutes;
            area1.AxisX.IntervalOffsetType = DateTimeIntervalType.Minutes;
            area1.AxisX.MajorGrid.LineColor = Color.White;
            area1.AxisY.MajorGrid.LineColor = Color.White;
            area1.BackColor = Color.Black;

            chart.Series.Clear();
            chart.ChartAreas.Clear();
            chart.ChartAreas.Add(area1);
            chart.Series.Add(seriesC1Model1);
            chart.Series.Add(seriesCeModel1);
            chart.Series.Add(seriesC1Model2);
            chart.Series.Add(seriesCeModel2);

            //chart.Legends[0].Title = "凡例";
            //chart.Legends[0].Position.Auto = false;
            //chart.Legends[0].Position.Width = 8.0F;
            //chart.Legends[0].Position.Height = 10.0F;
            //chart.Legends[0].Position.X = 0.0F;
            //chart.Legends[0].Position.Y = 0.0F;
        }

        private Series CreateSeries(string title, Color lineColor)
        {
            Series series = new Series
            {
                ChartType = SeriesChartType.Line,
                LegendText = title,
                XValueType = ChartValueType.DateTime,
                BorderWidth = 4,
                Color = lineColor
            };

            return series;

        }
    }
}
