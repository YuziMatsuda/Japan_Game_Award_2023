using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;

namespace Main.Model
{
    /// <summary>
    /// サン（ゴ）ショウコードの破片
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class CoralPartsModel : MonoBehaviour
    {
        /// <summary>移動速度</summary>
        [SerializeField] private float speed = 5f;
        /// <summary>ベロシティ</summary>
        [SerializeField] private Vector3 velocity;
        /// <summary>破片が止まる時間</summary>
        [SerializeField] private float duration = .25f;

        private void Start()
        {
            var rigidbody = GetComponent<Rigidbody2D>();
            var speedReact = new FloatReactiveProperty(speed);
            DOVirtual.DelayedCall(duration, () =>
            {
                Destroy(gameObject);
            });
            this.FixedUpdateAsObservable()
                .Subscribe(_ =>
                {
                    if (0f < speedReact.Value)
                        rigidbody.AddForce(velocity * speedReact.Value * Time.fixedDeltaTime, ForceMode2D.Impulse);
                });
        }
    }
}
