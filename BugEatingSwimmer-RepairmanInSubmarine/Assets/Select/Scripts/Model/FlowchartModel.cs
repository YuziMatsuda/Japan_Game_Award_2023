using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Select.Common;
using Select.Template;
using System.Linq;

namespace Select.Model
{
    /// <summary>
    /// Fungusのフローチャート
    /// ADVの台詞でコールバックさせる
    /// モデル
    /// </summary>
    public class FlowchartModel : MonoBehaviour, IFlowchartModel
    {
        /// <summary>シナリオ番号</summary>
        private readonly IntReactiveProperty _readedScenarioNo = new IntReactiveProperty();
        /// <summary>シナリオ番号</summary>
        public IReactiveProperty<int> ReadedScenarioNo => _readedScenarioNo;
        /// <summary>シナリオブロック名称配列</summary>
        [SerializeField] private string[] blockNames;

        public string GetBlockName(int index)
        {
            return blockNames[index];
        }

        /// <summary>
        /// シナリオ開かれた
        /// </summary>
        public void OnOpenedScenario()
        {
            _readedScenarioNo.Value = 0;
        }

        /// <summary>
        /// シナリオ読み込まれた
        /// </summary>
        public void OnReadedScenario()
        {
            _readedScenarioNo.Value = 1;
        }
    }

    /// <summary>
    /// Fungusのフローチャート
    /// ADVの台詞でコールバックさせる
    /// モデル
    /// インターフェース
    /// </summary>
    public interface IFlowchartModel
    {
        /// <summary>
        /// ミッションIDから対象のブロック名を取得
        /// </summary>
        /// <param name="index">シナリオインデックス</param>
        /// <returns>ブロック名</returns>
        public string GetBlockName(int index);
    }
}
