using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Common;
using Main.Model;
using DG.Tweening;

namespace Main.View
{
    /// <summary>
    /// コシギンチャク
    /// スプライト
    /// </summary>
    public class BodySpriteLoincloth : BodySprite, IBodySpriteLoincloth
    {
        /// <summary>設定</summary>
        [SerializeField] private LoinclothConfig loinclothConfig;
        /// <summary>スプライト</summary>
        [SerializeField] private SpriteRenderer spriteRenderer;
        /// <summary>トランスフォーム</summary>
        private Transform _transform;
        /// <summary>アニメーションの終了時間</summary>
        [SerializeField] private float[] durations = { .25f };
        /// <summary>振動時最大移動座標緒</summary>
        [SerializeField] private Vector3 punchPosition = new Vector3(.25f, 0f, 0f);
        /// <summary>振動数</summary>
        [SerializeField] private int vibrato = 5;
        /// <summary>振動する範囲</summary>
        [SerializeField] private float elasticity = 0f;

        public bool PlaySwing()
        {
            try
            {
                if (_transform == null)
                    _transform = transform;
                DOTween.Sequence()
                    .Append(DOTween.To(() => (int)EnumEnemySpriteIndex.Normal, x => spriteRenderer.sprite = loinclothConfig.LoinclothDetail.sprites[x], (int)EnumEnemySpriteIndex.Attack, fadeDuration))
                    .Join(_transform.DOPunchPosition(punchPosition, durations[0], vibrato, elasticity))
                    .SetLoops(-1, LoopType.Yoyo)
                    ;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        private void Reset()
        {
            loinclothConfig = GetComponent<LoinclothConfig>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            fadeDuration = 1.2f;
        }
    }

    /// <summary>
    /// コシギンチャク
    /// スプライト
    /// インターフェース
    /// </summary>
    public interface IBodySpriteLoincloth
    {
        /// <summary>
        /// 揺らすアニメーションを再生
        /// </summary>
        /// <returns></returns>
        public bool PlaySwing();
    }
}
