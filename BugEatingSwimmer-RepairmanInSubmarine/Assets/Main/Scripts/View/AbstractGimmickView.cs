using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// ビュー
    /// ギミック「ヒトデ」「エビダンス」使用
    /// </summary>
    public class AbstractGimmickView : MonoBehaviour
    {
        /// <summary>ボディのスプライト</summary>
        [SerializeField] protected BodySprite bodySprite;
        /// <summary>アサイン済みカラー</summary>
        [SerializeField] protected Color assignedColor = Color.white;
        /// <summary>未アサインカラー</summary>
        [SerializeField] protected Color unAssignColor;

        protected virtual void Reset()
        {
            bodySprite = transform.GetChild(0).GetComponent<BodySprite>();
        }
    }
}
