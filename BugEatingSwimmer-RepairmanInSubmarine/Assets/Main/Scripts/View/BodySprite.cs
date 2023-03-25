using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// ボディのスプライト
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class BodySprite : MonoBehaviour, IBodySprite
    {
        public bool SetColorSpriteRenderer(Color color)
        {
            try
            {
                GetComponent<SpriteRenderer>().color = color;

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
    /// ボディのスプライト
    /// インターフェース
    /// </summary>
    public interface IBodySprite
    {
        /// <summary>
        /// カラーを設定
        /// </summary>
        /// <param name="color">カラー情報</param>
        /// <returns>成功／失敗</returns>
        public bool SetColorSpriteRenderer(Color color);
    }
}
