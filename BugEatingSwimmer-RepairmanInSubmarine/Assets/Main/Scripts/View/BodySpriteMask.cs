using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;

namespace Main.View
{
    /// <summary>
    /// スプライトマスク
    /// </summary>
    [RequireComponent(typeof(SpriteMask))]
    public class BodySpriteMask : MonoBehaviour, IBodySpriteMask
    {
        ///// <summary>スプライトマスク</summary>
        //[SerializeField] private SpriteMask spriteMask;
        /// <summary>設定</summary>
        [SerializeField] private DarkLightConfig darkLightConfig;
        /// <summary>アニメーション終了時間</summary>
        [SerializeField] private float[] durations = { .5f };
        /// <summary>暗闇レベル</summary>
        private int _lightDownLevel = -1;
        /// <summary>追尾中</summary>
        private bool _hovering;
        /// <summary>トランスフォーム</summary>
        private Transform _transform;

        public bool HoverTarget(Transform target)
        {
            try
            {
                if (_transform == null)
                    _transform = transform;
                if (!_hovering)
                {
                    _hovering = true;
                    this.UpdateAsObservable()
                        .Subscribe(_ => _transform.position = target.position);
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public int PlayLightDown()
        {
            try
            {
                if (_transform == null)
                    _transform = transform;
                if (darkLightConfig.Scales != null &&
                    0 < darkLightConfig.Scales.Length)
                {
                    if (_lightDownLevel++ < darkLightConfig.Scales.Length)
                    {
                        _transform.DOScale(darkLightConfig.Scales[_lightDownLevel], durations[0])
                            .SetUpdate(true);
                    }

                    return _lightDownLevel;
                }

                return 0;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return -1;
            }
        }

        private void Reset()
        {
            darkLightConfig = GetComponent<DarkLightConfig>();
        }
    }

    /// <summary>
    /// スプライトマスク
    /// インターフェース
    /// </summary>
    public interface IBodySpriteMask
    {
        /// <summary>
        /// 暗闇レベルを一つ落とす
        /// </summary>
        /// <returns>現在のレベル</returns>
        public int PlayLightDown();
        /// <summary>
        /// ターゲットを追尾（一度のみ）
        /// </summary>
        /// <param name="target">追尾対象</param>
        /// <returns>成功／失敗</returns>
        public bool HoverTarget(Transform target);
    }
}
