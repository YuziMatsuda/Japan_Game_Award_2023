using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// プレイヤーのボディのスプライト
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class BodySpritePlayer : BodySprite, IBodySpritePlayer
    {
        /// <summary>アニメータ</summary>
        [SerializeField] private Animator animator;
        /// <summary>ターンパラメータ</summary>
        private static readonly string ANIMATOR_PARAMETERS_ONTURN = "OnTurn";

        public bool PlayTurnAnimation()
        {
            try
            {
                animator.SetTrigger(ANIMATOR_PARAMETERS_ONTURN);

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
            animator = GetComponent<Animator>();
        }
    }

    /// <summary>
    /// プレイヤーのボディのスプライト
    /// インターフェース
    /// </summary>
    public interface IBodySpritePlayer
    {
        /// <summary>
        /// ターン用のアニメーション再生
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool PlayTurnAnimation();
    }
}
