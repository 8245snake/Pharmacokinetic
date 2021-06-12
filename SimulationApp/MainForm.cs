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

            var time = new DateTime(2021, 6, 12, 12, 30, 0);

            // プロポフォールのモデルでシミュレーション開始
            var f = new EleveldModelFactory(50, 1.5, 40, 2200, true, false);
            var model1 = f.Create("プロポフォール_動脈", EleveldModelFactory.BloodVessels.Arterial);
            var model2 = f.Create("プロポフォール_静脈", EleveldModelFactory.BloodVessels.Venous);

            PharmacokineticSimulator sim = createSimulator(time, 1, 20);
            sim.BolusDose(time.AddMinutes(1), 100, WeightUnitEnum.ug);
            //sim.ContinuousDose(time, time.AddMinutes(1), 6000, WeightUnitEnum.mg, TimeUnitEnum.hour);
            LoadGraph(chartSimulation, sim, model1, model2);
        }

        private void LoadGraph(Chart chart, PharmacokineticSimulator simulator,  PharmacokineticModel model1, PharmacokineticModel model2)
        {
            Series seriesC1Arterial = CreateSeries("血中濃度(動脈)", Color.Red);
            Series seriesCeArterial = CreateSeries("効果部位濃度(動脈)", Color.Orange);

            foreach (var result in simulator.Predict(model1))
            {
                seriesC1Arterial.Points.Add(new DataPoint(result.PlotTime.ToOADate(), result.C1));
                seriesCeArterial.Points.Add(new DataPoint(result.PlotTime.ToOADate(), result.Ce));
            }

            Series seriesC1Venous = CreateSeries("血中濃度(静脈)", Color.Blue);
            Series seriesCeVenous = CreateSeries("効果部位濃度(静脈)", Color.LightSkyBlue);

            foreach (var result in simulator.Predict(model2))
            {
                seriesC1Venous.Points.Add(new DataPoint(result.PlotTime.ToOADate(), result.C1));
                seriesCeVenous.Points.Add(new DataPoint(result.PlotTime.ToOADate(), result.Ce));
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
            chart.Series.Add(seriesC1Arterial);
            chart.Series.Add(seriesCeArterial);
            chart.Series.Add(seriesC1Venous);
            chart.Series.Add(seriesCeVenous);

            chart.Legends[0].Title = "凡例";
            chart.Legends[0].Position.Auto = false;
            chart.Legends[0].Position.Width = 8.0F;
            chart.Legends[0].Position.Height = 10.0F;
            chart.Legends[0].Position.X = 0.0F;
            chart.Legends[0].Position.Y = 0.0F;
        }

        private PharmacokineticSimulator createSimulator(DateTime start, int step, int duration)
        {
            PharmacokineticSimulator sim = new PharmacokineticSimulator()
            {
                DurationdMinutes = duration,
                StepSeconds = step,
                CalculationStartTime = start
            };

            return sim;
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
