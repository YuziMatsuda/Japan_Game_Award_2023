using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// 暗闇
    /// ビュー
    /// </summary>
    public class DarkLightView : MonoBehaviour, IDarkLightView
    {
        /// <summary>暗闇のスプライト</summary>
        [SerializeField] private BodySpriteDarkLight bodySpriteDarkLight;

        public bool HoverTarget(Transform target)
        {
            return bodySpriteDarkLight.HoverTarget(target);
        }

        public int PlayLightDown()
        {
            return bodySpriteDarkLight.PlayLightDown();
        }

        private void Reset()
        {
            bodySpriteDarkLight = GetComponentInChildren<BodySpriteDarkLight>();
        }
    }

    /// <summary>
    /// 暗闇
    /// ビュー
    /// インターフェース
    /// </summary>
    public interface IDarkLightView
    {
        /// <summary>
        /// 暗闇レベルを一つ落とす
        /// </summary>
        /// <returns>現在のレベル</returns>
        public int PlayLightDown();
        /// <summary>
        /// ターゲットを追尾（一度のみ）
        /// </summary>
        /// <param name="target">追尾対象</param>
        /// <returns>成功／失敗</returns>
        public bool HoverTarget(Transform target);
    }
}
