using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Select.Common;
using DG.Tweening;

namespace Select.View
{
    /// <summary>
    /// ボディのテキスト
    /// </summary>
    [RequireComponent(typeof(Text))]
    public class BodyText : MonoBehaviour, IBodyText
    {
        /// <summary>テキスト</summary>
        [SerializeField] protected Text text;
        /// <summary>アニメーション終了時間</summary>
        [SerializeField] protected float[] durations = { .85f };

        public IEnumerator PlayFadeColorText(System.IObserver<bool> observer, EnumFadeState state)
        {
            text.DOFade(endValue: state.Equals(EnumFadeState.Close) ? 0f : 1f, durations[0])
                .OnComplete(() => observer.OnNext(true));

            yield return null;
        }

        public bool SetColorText(Color color)
        {
            try
            {
                text.color = color;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetTextEnabled(bool isEnabled)
        {
            try
            {
                text.enabled = isEnabled;
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
            text = GetComponent<Text>();
        }
    }

    /// <summary>
    /// ボディのテキスト
    /// インターフェース
    /// </summary>
    public interface IBodyText
    {
        /// <summary>
        /// テキストのステータスを変更
        /// </summary>
        /// <param name="isEnabled">有効／無効</param>
        /// <returns>成功／失敗</returns>
        public bool SetTextEnabled(bool isEnabled);
        /// <summary>
        /// テキストのカラーを変更
        /// </summary>
        /// <param name="isEnabled">有効／無効</param>
        /// <returns>成功／失敗</returns>
        public bool SetColorText(Color color);
        /// <summary>
        /// テキストのカラーを変更するフェードアニメーションを再生
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <param name="state">フェード状態</param>
        /// <returns>コルーチン</returns>
        public IEnumerator PlayFadeColorText(System.IObserver<bool> observer, EnumFadeState state);
    }
}
