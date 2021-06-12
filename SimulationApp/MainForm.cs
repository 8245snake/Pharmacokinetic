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
            PharmacokineticModel model = PharmacokineticModelFactory.CreatePropofol(50);
            PharmacokineticSimulator sim = createSimulator(time, 6, 20);
            // ボーラスだと一気に上がるので持続を短時間で入れるようにしてみた
            sim.ContinuousDose(time, time.AddSeconds(30), 200, WeightUnitEnum.ug, TimeUnitEnum.minute);
            LoadGraph(chartSimulation, sim, model);
        }

        private void LoadGraph(Chart chart, PharmacokineticSimulator simulator,  PharmacokineticModel model)
        {
            Series seriesC1 = CreateSeries("血中濃度", Color.Red);
            Series seriesCe = CreateSeries("効果部位濃度", Color.Blue);

            foreach (var result in simulator.Predict(model))
            {
                seriesC1.Points.Add(new DataPoint(result.PlotTime.ToOADate(), result.C1));
                seriesCe.Points.Add(new DataPoint(result.PlotTime.ToOADate(), result.Ce));
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
            chart.Series.Add(seriesC1);
            chart.Series.Add(seriesCe);

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
