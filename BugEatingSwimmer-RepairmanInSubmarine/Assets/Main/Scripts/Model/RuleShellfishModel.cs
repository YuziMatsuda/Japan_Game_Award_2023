using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Main.Common;
using System.Linq;

namespace Main.Model
{
    /// <summary>
    /// ルール貝
    /// モデル
    /// </summary>
    [RequireComponent(typeof(CircleCollider2D))]
    public class RuleShellfishModel : MonoBehaviour, IRuleShellfishModel
    {
        /// <summary>接触対象のオブジェクトタグ</summary>
        [SerializeField] protected string[] tags = { ConstTagNames.TAG_NAME_TOUCH_TRIGGER };
        /// <summary>接触状態か</summary>
        private readonly BoolReactiveProperty _isInRange = new BoolReactiveProperty();
        /// <summary>接触状態か</summary>
        public IReactiveProperty<bool> IsInRange => _isInRange;
        /// <summary>一度のみヒントを表示させる（デフォルト：無効）</summary>
        [SerializeField] private bool isOnlyOnceHint = false;
        /// <summary>一度のみヒントを表示させる（デフォルト：無効）</summary>
        public bool IsOnlyOnceHint => isOnlyOnceHint;
        /// <summary>設定</summary>
        [SerializeField] private RuleShellfishConfig ruleShellfishConfig;
        /// <summary>設定</summary>
        public RuleShellfishConfig RuleShellfishConfig => ruleShellfishConfig;
        /// <summary>接触回数</summary>
        private int _onInRangedCount;
        /// <summary>接触回数</summary>
        public int OnInRangedCount => _onInRangedCount;

        private void Reset()
        {
            ruleShellfishConfig = GetComponent<RuleShellfishConfig>();
        }

        private void Start()
        {
            if (ruleShellfishConfig.IsRetake)
                this.OnTriggerExit2DAsObservable()
                    .Subscribe(collision =>
                    {
                        if (0 < tags.Where(q => collision.CompareTag(q)).Select(q => q).ToArray().Length)
                        {
                            _isInRange.Value = false;
                            _onInRangedCount++;
                        }
                    });
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (0 < tags.Where(q => collision.CompareTag(q)).Select(q => q).ToArray().Length)
            {
                _isInRange.Value = true;
            }
        }

        public bool SetColliderState(bool active)
        {
            try
            {
                GetComponent<CircleCollider2D>().enabled = active;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }
    }

    /// <summary>
    /// ルール貝
    /// モデル
    /// インターフェース
    /// </summary>
    public interface IRuleShellfishModel
    {
        /// <summary>
        /// コライダーの状態をセット
        /// </summary>
        /// <param name="active">有効／無効</param>
        /// <returns>成功／失敗</returns>
        public bool SetColliderState(bool active);
    }
}
