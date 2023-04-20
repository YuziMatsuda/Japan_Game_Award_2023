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
    public class NavigationCursor : MonoBehaviour, INavigationCursor, ILogoCursorView
    {
        /// <summary>ロゴカーソルのビュー配列</summary>
        [SerializeField] private LogoCursorView[] logoCursorViews;
        /// <summary>トランスフォーム</summary>
        private Transform _transform;


        public bool RedererCursorDirectionAndDistance(Navigation navigation, EnumCursorDistance enumCursorDistance)
        {
            try
            {
                if (_transform == null)
                    _transform = transform;
                if (!logoCursorViews[(int)EnumDirectionMode.Up].SetImageEnabled(navigation.selectOnUp != null))
                    throw new System.Exception("イメージのステータスを変更呼び出しの失敗");
                if (!logoCursorViews[(int)EnumDirectionMode.Right].SetImageEnabled(navigation.selectOnRight != null))
                    throw new System.Exception("イメージのステータスを変更呼び出しの失敗");
                if (!logoCursorViews[(int)EnumDirectionMode.Down].SetImageEnabled(navigation.selectOnDown != null))
                    throw new System.Exception("イメージのステータスを変更呼び出しの失敗");
                if (!logoCursorViews[(int)EnumDirectionMode.Left].SetImageEnabled(navigation.selectOnLeft != null))
                    throw new System.Exception("イメージのステータスを変更呼び出しの失敗");
                foreach (var item in logoCursorViews)
                    if (!item.SetCursorDistance(enumCursorDistance))
                        throw new System.Exception("カーソル長さをセット呼び出しの失敗");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetCursorDistance(EnumCursorDistance enumCursorDistance)
        {
            throw new System.NotImplementedException();
        }

        public bool SetImageEnabled(bool isEnabled)
        {
            try
            {
                foreach (var item in logoCursorViews)
                    if (!item.SetImageEnabled(isEnabled))
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
        /// <param name="enumCursorDistance">カーソルの長さ</param>
        /// <returns>成功／失敗</returns>
        public bool RedererCursorDirectionAndDistance(Navigation navigation, EnumCursorDistance enumCursorDistance);
    }
}
