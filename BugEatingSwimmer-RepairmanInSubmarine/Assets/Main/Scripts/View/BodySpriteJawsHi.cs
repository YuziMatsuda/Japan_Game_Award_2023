using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Main.Model;
using Main.Common;

namespace Main.View
{
    /// <summary>
    /// ジョーシー
    /// スプライト
    /// </summary>
    public class BodySpriteJawsHi : BodySprite, IBodySpriteJawsHi, IBodySpriteWow
    {
        /// <summary>！！のスプライト</summary>
        [SerializeField] private BodySpriteWow bodySpriteWow;
        /// <summary>表示</summary>
        [SerializeField] private Color visibledColorOfChild = new Color(1f, 1f, 1f, 1f);
        /// <summary>非表示</summary>
        [SerializeField] private Color disabledColorOfChild = new Color(1f, 1f, 1f, 0f);
        /// <summary>設定</summary>
        [SerializeField] private LoinclothConfig loinclothConfig;
        /// <summary>スプライト</summary>
        [SerializeField] private SpriteRenderer spriteRenderer;

        public IEnumerator PlayWowAnimation(System.IObserver<bool> observer)
        {
            Observable.FromCoroutine<bool>(observer => bodySpriteWow.PlayWowAnimation(observer))
                .Subscribe(_ => observer.OnNext(true))
                .AddTo(gameObject);

            yield return null;
        }

        public bool SetColorSpriteRendererWow(bool isEnabled)
        {
            return bodySpriteWow.SetColorSpriteRenderer(isEnabled ? visibledColorOfChild : disabledColorOfChild);
        }

        public bool SetScale(Vector3 scale)
        {
            return ((IBodySpriteWow)bodySpriteWow).SetScale(scale);
        }

        public bool SetSpriteIndex(EnumEnemySpriteIndex enumEnemySpriteIndex)
        {
            try
            {
                spriteRenderer.sprite = loinclothConfig.JawsHiDetail.sprites[(int)enumEnemySpriteIndex];

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
            bodySpriteWow = GetComponentInChildren<BodySpriteWow>();
            loinclothConfig = GetComponent<LoinclothConfig>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    /// <summary>
    /// ジョーシー
    /// スプライト
    /// インターフェース
    /// </summary>
    public interface IBodySpriteJawsHi
    {
        /// <summary>
        /// カラーを設定
        /// </summary>
        /// <param name="isEnabled">表示／非表示</param>
        /// <returns>成功／失敗</returns>
        public bool SetColorSpriteRendererWow(bool isEnabled);
        /// <summary>
        /// スプライト配列の番号をセット
        /// </summary>
        /// <param name="enumEnemySpriteIndex">スプライト配列の番号</param>
        /// <returns>成功／失敗</returns>
        public bool SetSpriteIndex(EnumEnemySpriteIndex enumEnemySpriteIndex);
    }
}
