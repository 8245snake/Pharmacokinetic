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
using Simulator.Dosing;
using static Simulator.Dosing.Medicine;

namespace SimulationApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            // いろいろ初期化
            InitializeComponent();
            chartSimulation.Series.Clear();
            chartSimulation.ChartAreas.Clear();

            // 描画
            DrawChartTest();

        }

        /// <summary>
        /// テスト関数
        /// </summary>
        private void DrawChartTest()
        {
            // 開始時刻設定
            var time = new DateTime(2021, 6, 12, 17, 20, 0);

            // プロポフォールのモデル作成（50kg）
            var f = new EleveldModelFactory(50, 1.5, 40, 2200, false, true);
            var model1 = f.Create("ﾌﾟﾛﾎﾟﾌｫｰﾙ_Eleveld(動脈)", EleveldModelFactory.BloodVessels.Arterial);
            var model2 = f.Create("ﾌﾟﾛﾎﾟﾌｫｰﾙ_Eleveld(静脈)", EleveldModelFactory.BloodVessels.Venous);
            var model3 = PharmacokineticModelFactory.CreatePropofol2(50);
            var model4 = PharmacokineticModelFactory.CreatePropofol2(50);
            var model5 = PharmacokineticModelFactory.CreatePropofol3(50);
            var factory = new PharmacokineticModelFactory(50, 1.5, 40, true);
            var model6 = factory.Create(1);
            var model7 = factory.Create(2);

            // チャートエリア作成
            SimulationChart chart = new SimulationChart(this.chartSimulation);
            chart.AddPlots(new SimulationChartPlots(model1.Name, SimulationChartPlots.ColorPattern.Red, model1));
            chart.AddPlots(new SimulationChartPlots(model2.Name, SimulationChartPlots.ColorPattern.Blue, model2));
            chart.AddPlots(new SimulationChartPlots(model3.Name, SimulationChartPlots.ColorPattern.Green, model3));
            chart.AddPlots(new SimulationChartPlots(model4.Name, SimulationChartPlots.ColorPattern.Orange, model4));
            chart.AddPlots(new SimulationChartPlots(model5.Name, SimulationChartPlots.ColorPattern.Yellow, model5));
            chart.AddPlots(new SimulationChartPlots(model5.Name, SimulationChartPlots.ColorPattern.Water, model6));
            chart.AddPlots(new SimulationChartPlots(model5.Name, SimulationChartPlots.ColorPattern.Pink, model7));

            // シミュレータ作成
            PharmacokineticSimulator sim = new PharmacokineticSimulator(time, 1,10);
            // 開始時刻に投与
            sim.BolusDose(time.AddMinutes(2), 100, WeightUnitEnum.mg, 30);
            // 描画
            chart.Draw(sim);

            // todo 対数グラフにする設定
            //chartSimulation.SuppressExceptions = true;
            //chartSimulation.ChartAreas.First().AxisY.IsLogarithmic = true;

        }
    }

    public class SimulationChart
    {
        public Chart Chart { get; set; }
        public ChartArea ChartArea { get; set; }

        private List<SimulationChartPlots> _simulationChartPlotsList = new List<SimulationChartPlots>();

        public SimulationChart(Chart chartCtrl)
        {
            var font = new System.Drawing.Font("Meiryo UI", 12, FontStyle.Regular, GraphicsUnit.Point);

            ChartArea area = new ChartArea();
            area.AxisX.TitleFont = font;
            area.AxisX.Title = "時刻";
            area.AxisY.TitleFont = font;
            area.AxisY.Title = "濃度(ng/ml)";
            area.AxisX.LabelStyle.Format = "HH:mm";
            area.AxisX.Interval = 1;
            area.AxisX.IntervalType = DateTimeIntervalType.Minutes;
            area.AxisX.IntervalOffsetType = DateTimeIntervalType.Minutes;
            area.AxisX.MajorGrid.LineColor = Color.White;
            area.AxisY.MajorGrid.LineColor = Color.White;
            area.BackColor = Color.Black;
            ChartArea = area;

            chartCtrl.ChartAreas.Add(ChartArea);
            chartCtrl.Legends[0].Font = font;
            chartCtrl.Legends[0].Position.Auto = false;
            chartCtrl.Legends[0].Position.X = 70;
            chartCtrl.Legends[0].Position.Y = 5;
            chartCtrl.Legends[0].Position.Height = 10;
            chartCtrl.Legends[0].Position.Width = 40;
            chartCtrl.Legends[0].IsTextAutoFit = true;
            Chart = chartCtrl;
            
        }

        public void AddPlots(SimulationChartPlots plots)
        {
            _simulationChartPlotsList.Add(plots);
        }

        public void Draw(PharmacokineticSimulator simulator, Medicine.WeightUnitEnum displayWeightUnit = Medicine.WeightUnitEnum.ug)
        {
            Chart.ChartAreas[0].AxisY.Title = $"濃度({displayWeightUnit.Name()}/ml)";

            foreach (var plots in _simulationChartPlotsList)
            {
                foreach (var result in simulator.Predict(plots.Model, displayWeightUnit))
                {
                    plots.SeriesC1.Points.Add(new DataPoint(result.PlotTime.ToOADate(), result.C1));
                    plots.SeriesCe.Points.Add(new DataPoint(result.PlotTime.ToOADate(), result.Ce));
                }

                Chart.Series.Add(plots.SeriesC1);
                Chart.Series.Add(plots.SeriesCe);
            }
        }

    }


    public class SimulationChartPlots
    {
        public enum ColorPattern
        {
            Red,
            Orange,
            Yellow,
            Green,
            Water,
            Blue,
            Purple,
            Pink
        }

        private enum ConcKind
        {
            C1,
            Ce
        }


        public Series SeriesC1 { get; set; }
        public Series SeriesC2 { get; set; }
        public Series SeriesC3 { get; set; }
        public Series SeriesCe { get; set; }
        public PharmacokineticModel Model { get; set; }

        public SimulationChartPlots(string title, ColorPattern pattern, PharmacokineticModel model)
        {
            Series CreateFunc(string suffix, ConcKind kind) =>
                new Series
                {
                    ChartType = SeriesChartType.Line,
                    LegendText = $"{title}{suffix}",
                    XValueType = ChartValueType.DateTime,
                    BorderWidth = 4,
                    Color = GetColor(pattern, kind)
                };


            SeriesC1 = CreateFunc("(血中濃度)", ConcKind.C1);
            SeriesC2 = CreateFunc("(C2)", ConcKind.Ce);
            SeriesC3 = CreateFunc("(C3)", ConcKind.Ce);
            SeriesCe = CreateFunc("(効果部位濃度)", ConcKind.Ce);
            Model = model;

        }

        /// <summary>
        /// 描画色を取得する
        /// </summary>
        /// <param name="pattern">パターンを<see cref="ColorPattern"/>の色から指定する。</param>
        /// <param name="kind"><see cref="pattern"/>のメインカラーかサブカラーか</param>
        /// <returns></returns>
        private Color GetColor(ColorPattern pattern, ConcKind kind)
        {
            switch (pattern)
            {
                case ColorPattern.Red:
                    switch (kind)
                    {
                        case ConcKind.C1:
                            return ColorTranslator.FromHtml("#FF0000");
                        case ConcKind.Ce:
                            return ColorTranslator.FromHtml("#FFBFBF");
                    }
                    break;
                case ColorPattern.Orange:
                    switch (kind)
                    {
                        case ConcKind.C1:
                            return ColorTranslator.FromHtml("#FF9900");
                        case ConcKind.Ce:
                            return ColorTranslator.FromHtml("#FFE6BF");
                    }
                    break;
                case ColorPattern.Yellow:
                    switch (kind)
                    {
                        case ConcKind.C1:
                            return ColorTranslator.FromHtml("#CCFF00");
                        case ConcKind.Ce:
                            return ColorTranslator.FromHtml("#F2FFBF");
                    }
                    break;
                case ColorPattern.Green:
                    switch (kind)
                    {
                        case ConcKind.C1:
                            return ColorTranslator.FromHtml("#00FF66");
                        case ConcKind.Ce:
                            return ColorTranslator.FromHtml("#BFFFD9");
                    }
                    break;
                case ColorPattern.Water:
                    switch (kind)
                    {
                        case ConcKind.C1:
                            return ColorTranslator.FromHtml("#00FFFF");
                        case ConcKind.Ce:
                            return ColorTranslator.FromHtml("#BFFFFF");
                    }
                    break;
                case ColorPattern.Blue:
                    switch (kind)
                    {
                        case ConcKind.C1:
                            return ColorTranslator.FromHtml("#0066FF");
                        case ConcKind.Ce:
                            return ColorTranslator.FromHtml("#0066FF");
                    }
                    break;
                case ColorPattern.Purple:
                    switch (kind)
                    {
                        case ConcKind.C1:
                            return ColorTranslator.FromHtml("#CC00FF");
                        case ConcKind.Ce:
                            return ColorTranslator.FromHtml("#F2BFFF");
                    }
                    break;
                case ColorPattern.Pink:
                    switch (kind)
                    {
                        case ConcKind.C1:
                            return ColorTranslator.FromHtml("#FF0099");
                        case ConcKind.Ce:
                            return ColorTranslator.FromHtml("#FFBFE5");
                    }
                    break;
            }
            return Color.White;
        }
    }

}
