using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Select.Common;

namespace Select.View
{
    /// <summary>
    /// ビュー
    /// ロゴカーソル
    /// </summary>
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(LogocursorConfig))]
    public class LogoCursorView : MonoBehaviour, ILogoCursorView
    {
        /// <summary>イメージ</summary>
        [SerializeField] private Image image;
        /// <summary>ロゴカーソル設定</summary>
        [SerializeField] private LogocursorConfig logocursorConfig;
        /// <summary>トランスフォーム</summary>
        private Transform _transform;

        private void Reset()
        {
            image = GetComponent<Image>();
            logocursorConfig = GetComponent<LogocursorConfig>();
        }

        public bool SetImageEnabled(bool isEnabled)
        {
            try
            {
                image.enabled = isEnabled;
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
            try
            {
                if (_transform == null)
                    _transform = transform;
                (_transform as RectTransform).anchoredPosition = logocursorConfig.Positions[(int)enumCursorDistance];

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
    /// ビュー
    /// ロゴカーソル
    /// インターフェース
    /// </summary>
    public interface ILogoCursorView
    {
        /// <summary>
        /// イメージのステータスを変更
        /// </summary>
        /// <param name="isEnabled">有効／無効</param>
        /// <returns>成功／失敗</returns>
        public bool SetImageEnabled(bool isEnabled);
        /// <summary>
        /// カーソル長さをセット
        /// </summary>
        /// <param name="enumCursorDistance">カーソルの長さ</param>
        /// <returns>成功／失敗</returns>
        public bool SetCursorDistance(EnumCursorDistance enumCursorDistance);
    }
}
