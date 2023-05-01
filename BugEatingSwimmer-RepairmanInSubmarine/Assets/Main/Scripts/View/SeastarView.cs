using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// ビュー
    /// ヒトデ
    /// </summary>
    public class SeastarView : MonoBehaviour, ISeastarView
    {
        /// <summary>ボディのスプライト</summary>
        [SerializeField] private BodySprite bodySprite;
        /// <summary>アサイン済みカラー</summary>
        [SerializeField] private Color assignedColor = Color.white;
        /// <summary>未アサインカラー</summary>
        [SerializeField] private Color unAssignColor;

        private void Reset()
        {
            bodySprite = transform.GetChild(0).GetComponent<BodySprite>();
        }

        public bool SetColorSpriteRenderer(Color color)
        {
            return bodySprite.SetColorSpriteRenderer(color);
        }

        public bool SetColorAssigned()
        {
            return bodySprite.SetColorSpriteRenderer(assignedColor);
        }

        public bool SetColorUnAssign()
        {
            return bodySprite.SetColorSpriteRenderer(unAssignColor);
        }
    }

    public interface ISeastarView
    {
        /// <summary>
        /// アサイン済みのカラー設定
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool SetColorAssigned();

        /// <summary>
        /// 未アサイン状態のカラー設定
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool SetColorUnAssign();
    }
}
