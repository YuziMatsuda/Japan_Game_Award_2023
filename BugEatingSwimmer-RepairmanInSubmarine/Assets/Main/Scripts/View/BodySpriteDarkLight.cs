using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// 暗闇のスプライト
    /// </summary>
    public class BodySpriteDarkLight : BodySprite, IBodySpriteDarkLight, IBodySpriteMask
    {
        /// <summary>スプライトマスク</summary>
        [SerializeField] private BodySpriteMask bodySpriteMask;
        /// <summary>ジョーシーマスクのプレハブ</summary>
        [SerializeField] private Transform bodySpriteMaskOfJowsHIPrefab;
        /// <summary>トランスフォーム</summary>
        private Transform _transform;

        public bool HoverTarget(Transform target)
        {
            return ((IBodySpriteMask)bodySpriteMask).HoverTarget(target);
        }

        public int PlayLightDown()
        {
            return ((IBodySpriteMask)bodySpriteMask).PlayLightDown();
        }

        public Transform InstancesAndHoverTarget(Transform transform)
        {
            try
            {
                if (_transform == null)
                    _transform = this.transform;
                var mask = Instantiate(bodySpriteMaskOfJowsHIPrefab, _transform);
                if (mask.GetComponent<BodySpriteMask>() == null)
                    throw new System.Exception("コンポーネント未所持");
                else
                {
                    if (!mask.GetComponent<BodySpriteMask>().HoverTarget(transform))
                        throw new System.Exception("ターゲットを追尾（一度のみ）呼び出しの失敗");

                    return mask;
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }

        private void Reset()
        {
            bodySpriteMask = GetComponentInChildren<BodySpriteMask>();
        }

        public bool SetPositionToForward(bool isBackOfFrom)
        {
            throw new System.NotImplementedException();
        }
    }

    /// <summary>
    /// 暗闇のスプライト
    /// インターフェース
    /// </summary>
    public interface IBodySpriteDarkLight
    {
        /// <summary>
        /// マスクをインスタンスしてターゲットを追尾
        /// </summary>
        /// <param name="target">追尾対象</param>
        /// <returns>マスク</returns>
        public Transform InstancesAndHoverTarget(Transform target);
    }
}
