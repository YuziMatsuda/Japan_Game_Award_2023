using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Select.Common;

namespace Select.View
{
    /// <summary>
    /// ステージ選択のカーソルナビゲーション表示
    /// </summary>
    public class NavigationCursor : MonoBehaviour, INavigationCursor
    {
        /// <summary>ロゴカーソルのビュー配列</summary>
        [SerializeField] private LogoCursorView[] logoCursorViews;
        /// <summary>トランスフォーム</summary>
        private Transform _transform;

        public bool RedererCursorDirection(Navigation navigation)
        {
            return RedererCursorDirection(navigation, false);
        }

        public bool RedererCursorDirection(Navigation navigation, bool isIgnoreScaleImpact)
        {
            try
            {
                if (_transform == null)
                    _transform = transform;

                // スケールを戻す
                if (isIgnoreScaleImpact && !_transform.localScale.Equals(Vector3.one))
                {
                    _transform.localScale = Vector3.one;
                }

                if (!logoCursorViews[(int)EnumDirectionMode.Up].SetImageEnabled(navigation.selectOnUp != null))
                    throw new System.Exception("イメージのステータスを変更呼び出しの失敗");
                if (!logoCursorViews[(int)EnumDirectionMode.Right].SetImageEnabled(navigation.selectOnRight != null))
                    throw new System.Exception("イメージのステータスを変更呼び出しの失敗");
                if (!logoCursorViews[(int)EnumDirectionMode.Down].SetImageEnabled(navigation.selectOnDown != null))
                    throw new System.Exception("イメージのステータスを変更呼び出しの失敗");
                if (!logoCursorViews[(int)EnumDirectionMode.Left].SetImageEnabled(navigation.selectOnLeft != null))
                    throw new System.Exception("イメージのステータスを変更呼び出しの失敗");

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
            logoCursorViews = GetComponentsInChildren<LogoCursorView>();
        }
    }

    /// <summary>
    /// ステージ選択のカーソルナビゲーション表示
    /// 
    /// インターフェース
    /// </summary>
    public interface INavigationCursor
    {
        /// <summary>
        /// ナビゲーションの状態によってカーソル表示を変更
        /// </summary>
        /// <param name="navigation">ナビゲーション</param>
        /// <returns>成功／失敗</returns>
        public bool RedererCursorDirection(Navigation navigation);
        /// <summary>
        /// ナビゲーションの状態によってカーソル表示を変更
        /// </summary>
        /// <param name="navigation">ナビゲーション</param>
        /// <param name="isIgnoreScaleImpact">大きさの影響を受けない</param>
        /// <returns>成功／失敗</returns>
        public bool RedererCursorDirection(Navigation navigation, bool isIgnoreScaleImpact);
    }
}
