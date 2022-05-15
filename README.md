# 概要

３－コンパートメントモデルによる計算処理が利用可能なライブラリ、およびビューア。

![SimulationSample](https://user-images.githubusercontent.com/36323985/121774833-3a2aaf00-cbbf-11eb-9605-b7b2ec0fe899.PNG)

# サンプル

```C#

using Simulator;
using  static  Simulator.Dosing.Medicine;

private static void TestBolusPropofol()
{
    var time = new DateTime(2021, 6, 12, 12, 30, 0);

    PharmacokineticSimulator sim = new PharmacokineticSimulator()
    {
        DurationdMinutes = 10,
        StepSeconds = 60,
        CalculationStartTime = time
    };

    // 開始時に100μg投与
    sim.BolusDose(time, 100, WeightUnitEnum.ug);

    // プロポフォールのモデルでシミュレーション開始
    PharmacokineticModel model = PharmacokineticModelFactory.CreatePropofol(50);
    foreach (var result in sim.Predict(model))
    {
        Console.WriteLine(result.ToString());
    }
}   
     
```

# WEBアプリ版

以下で公開中

https://8245snake.github.io/Pharmacokinetic/
