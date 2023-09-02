using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Select.Common;

namespace Select.Model
{
    /// <summary>
    /// ボディのイメージ
    /// </summary>
    public class BodyImage : MonoBehaviour
    {
        /// <summary>向き（右向きが正面）</summary>
        private readonly IntReactiveProperty direction = new IntReactiveProperty();
        /// <summary>向き（右向きが正面）</summary>
        public IReactiveProperty<int> Direction => direction;

        private void Start()
        {
            var t = transform;
            var currentPosition = new Vector2ReactiveProperty();
            var prevPosition = new Vector2ReactiveProperty();
            currentPosition.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    if (currentPosition.Value.Equals(prevPosition.Value))
                        direction.Value = (int)EnumDirectionMode2D.Default;
                    else
                    {
                        // 右向きは正面、それ以外は左向き
                        direction.Value = prevPosition.Value.x < currentPosition.Value.x ? (int)EnumDirectionMode2D.Forward : (int)EnumDirectionMode2D.Back;
                        // 最後の位置を更新
                        prevPosition.Value = currentPosition.Value;
                    }
                });
            this.UpdateAsObservable()
                .Subscribe(_ => currentPosition.Value = (t as RectTransform).anchoredPosition);
        }
    }
}
