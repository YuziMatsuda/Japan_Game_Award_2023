using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Area.View
{
    /// <summary>
    /// ヒトデ総配属人数
    /// </summary>
    [RequireComponent(typeof(Text))]
    public class SeastarCount : MonoBehaviour, ISeastarCount
    {
        /// <summary>人数表示</summary>
        [SerializeField] private Text counter;
        /// <summary>タイトル</summary>
        [SerializeField] private string title = "取得数：";

        public bool SetCounterText(int countValue)
        {
            try
            {
                if (countValue < 0)
                    throw new System.Exception("数の取得失敗");

                var nThPlace = countValue < 10 ? "0" : "";
                counter.text = $"{title}{nThPlace}{countValue}";

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
            counter = GetComponent<Text>();
        }
    }

    /// <summary>
    /// ヒトデ総配属人数
    /// インターフェース
    /// </summary>
    public interface ISeastarCount
    {
        /// <summary>
        /// ヒトデ総配属人数をセット
        /// </summary>
        /// <param name="countValue">人数</param>
        /// <returns>成功／失敗</returns>
        public bool SetCounterText(int countValue);
    }
}
