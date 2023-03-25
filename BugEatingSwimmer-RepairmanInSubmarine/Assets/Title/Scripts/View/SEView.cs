using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Title.Common;

namespace Title.View
{
    /// <summary>
    /// ビュー
    /// SEタイトル、スライダー、設定値の表示用パネル
    /// </summary>
    public class SEView : CanvasFadeController
    {
        /// <summary>終了時間</summary>
        [SerializeField] private float duration = .1f;

        /// <summary>
        /// フェードのDOTweenアニメーション再生
        /// </summary>
        /// <param name="state">ステータス</param>
        /// <returns>成功／失敗</returns>
        public bool PlayFadeAnimation(EnumOptionContentState state)
        {
            try
            {
                canvasGroup.DOFade(endValue: state.Equals(EnumOptionContentState.Selected) ? 1f : fadeValue, duration)
                    .SetUpdate(true);

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }
    }
}
