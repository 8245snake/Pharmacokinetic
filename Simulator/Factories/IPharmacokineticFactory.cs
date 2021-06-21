
using Simulator.Models;

namespace Simulator.Factories
{
    public interface IPharmacokineticFactory
    {
        /// <summary>
        /// ファクトリに個人パラメータを設定する
        /// </summary>
        /// <param name="individual">個人パラメータ</param>
        void SetIndivisual(IndividualModel individual);

        /// <summary>
        /// 薬物動態モデルを作成する
        /// </summary>
        /// <param name="modelName">モデル名</param>
        /// <returns>薬物動態モデル</returns>
        PharmacokineticModel Create(string modelName);

    }
}