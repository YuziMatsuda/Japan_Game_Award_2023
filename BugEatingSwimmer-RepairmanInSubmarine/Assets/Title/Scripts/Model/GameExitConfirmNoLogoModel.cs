using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Title.Model
{
    /// <summary>
    /// モデル
    /// ゲームを終了しますか？Noロゴ
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class GameExitConfirmNoLogoModel : UIEventController
    {
        /// <summary>ボタン</summary>
        private Button _button;

        /// <summary>
        /// ボタンのステータスを変更
        /// </summary>
        /// <param name="enabled">有効／無効</param>
        /// <returns>成功／失敗</returns>
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
    }
}
