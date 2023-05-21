using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// ヒトデゲージの背景
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class SeastartGageBackground : MonoBehaviour, ISeastartGageBackground
    {
        /// <summary>ヒトデゲージの設定</summary>
        [SerializeField] private SeastarGageConfig seastarGageConfig;
        /// <summary>スプライト</summary>
        [SerializeField] private SpriteRenderer spriteRenderer;

        public bool SetSprite(int index)
        {
            try
            {
                spriteRenderer.sprite = seastarGageConfig.Sprites[index];

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
            seastarGageConfig = GetComponent<SeastarGageConfig>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    /// <summary>
    /// ヒトデゲージの背景
    /// </summary>
    public interface ISeastartGageBackground
    {
        /// <summary>
        /// スプライトをセット
        /// </summary>
        /// <param name="index">配列番号</param>
        /// <returns>成功／失敗</returns>
        public bool SetSprite(int index);
    }
}
