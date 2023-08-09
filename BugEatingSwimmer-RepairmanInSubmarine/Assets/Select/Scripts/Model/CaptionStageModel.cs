using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UniRx;
using System.Linq;

namespace Select.Model
{
    /// <summary>
    /// モデル
    /// ステージキャプション
    /// </summary>
    public class CaptionStageModel : UIEventController, ILogoStageModel, ICaptionStageModel
    {
        /// <summary>ヒトデのモデル配列</summary>
        [SerializeField] private SeastarModel[] seastarModels;
        public SeastarModel[] SeastarModels => seastarModels;
        /// <summary>アサインされたか</summary>
        public IReactiveProperty<bool>[] IsAssigned => seastarModels.Select(q => q.IsAssigned)
            .ToArray();
        /// <summary>アサインされた数</summary>
        public int IsAssignedCount => seastarModels.Where(q => q.IsAssigned.Value)
            .Select(q => q.IsAssigned)
            .ToArray()
            .Length;
        /// <summary>アサインされたか（ローカルカウント）</summary>
        public IReactiveProperty<bool>[] IsAssignedLocal => seastarModels.Select(q => q.IsAssignedLocal).ToArray();
        /// <summary>ステージ番号</summary>
        private int _index = -1;
        /// <summary>番号置換の正規表現</summary>
        private readonly Regex _regex = new Regex("^.*_");
        /// <summary>ステージ番号</summary>
        public int Index
        {
            get
            {
                // ステージ番号セットの初期処理
                if (_index < 0)
                    _index = int.Parse(_regex.Replace(name, ""));
                if (_index < 0)
                    throw new System.Exception("置換失敗");
                return _index;
            }
        }

        private void Reset()
        {
            seastarModels = transform.GetChild(3).GetComponentsInChildren<SeastarModel>();
        }

        public bool SetButtonEnabled(bool enabled)
        {
            try
            {
                if (_button == null)
                    _button = GetComponent<Button>();
                _button.enabled = enabled;
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetEventTriggerEnabled(bool enabled)
        {
            try
            {
                if (_eventTrigger == null)
                    _eventTrigger = GetComponent<EventTrigger>();
                _eventTrigger.enabled = enabled;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetIsAssigned(bool assign, int index)
        {
            return seastarModels[index].SetIsAssigned(assign);
        }
    }

    /// <summary>
    /// モデル
    /// ステージキャプション
    /// インターフェース
    /// </summary>
    public interface ICaptionStageModel
    {
        /// <summary>
        /// アサイン情報をセット
        /// </summary>
        /// <param name="assign">アサイン</param>
        /// <param name="index">インデックス</param>
        /// <returns>成功／失敗</returns>
        public bool SetIsAssigned(bool assign, int index);
    }
}
