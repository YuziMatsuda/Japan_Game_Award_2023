using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// ビュー
    /// エビダンス
    /// </summary>
    public class ShrimpDanceView : AbstractGimmickView, IShrimpDanceView, IBodySpriteShrimpDance
    {
        protected override void Reset()
        {
            base.Reset();
            bodySprite = transform.GetChild(0).GetComponent<BodySpriteShrimpDance>();
        }

        public bool SetColorAssigned()
        {
            return bodySprite.SetColorSpriteRenderer(assignedColor);
        }

        public bool SetColorUnAssign()
        {
            return bodySprite.SetColorSpriteRenderer(unAssignColor);
        }

        public bool PlayDanceAnimation()
        {
            return ((BodySpriteShrimpDance)bodySprite).PlayDanceAnimation();
        }

        public bool StopDanceAnimation()
        {
            return ((BodySpriteShrimpDance)bodySprite).StopDanceAnimation();
        }
    }

    /// <summary>
    /// ビュー
    /// エビダンス
    /// インターフェース
    /// </summary>
    public interface IShrimpDanceView
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
