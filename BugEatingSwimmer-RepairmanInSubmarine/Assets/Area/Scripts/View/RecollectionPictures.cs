using Area.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Area.View
{
    /// <summary>
    /// 回想シーンのイメージを表示
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class RecollectionPictures : MonoBehaviour, IRecollectionPictures
    {
        /// <summary>設定</summary>
        [SerializeField] private RecollectionPicturesConfig recollectionPicturesConfig;
        /// <summary>イメージ</summary>
        [SerializeField] private Image image;

        public bool SetSprite(EnumRecollectionPicture index)
        {
            try
            {
                image.sprite = recollectionPicturesConfig.Sprites[(int)index];

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
            recollectionPicturesConfig = GetComponent<RecollectionPicturesConfig>();
            image = GetComponent<Image>();
        }

    }

    /// <summary>
    /// 回想シーンのイメージを表示
    /// インターフェース
    /// </summary>
    public interface IRecollectionPictures
    {
        /// <summary>
        /// スプライトをセット
        /// </summary>
        /// <param name="index">カット番号</param>
        /// <returns>成功／失敗</returns>
        public bool SetSprite(EnumRecollectionPicture index);
    }
}
