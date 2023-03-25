using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;

namespace Select.View
{
    /// <summary>
    /// ビュー
    /// プレイヤー
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class PlayerView : SelectStageFrameViewParent, ISelectStageFrameView
    {
        /// <summary>一つ前の位置</summary>
        private Vector3 _prevPosition;
        /// <summary>一つ前のターゲット</summary>
        private Transform _prevTarget;
        /// <summary>一つ前のターゲット（デフォルト）</summary>
        [SerializeField] private Transform defaultTarget;
        /// <summary>移動元オブジェクトリスト</summary>
        [SerializeField] private Transform[] froms;
        /// <summary>移動先オブジェクトリスト</summary>
        [SerializeField] private Transform[] tos;
        /// <summary>移動先の位置補正</summary>
        [SerializeField] private Vector3[] toPositionCrrection;
        /// <summary>アニメーション再生</summary>
        private readonly BoolReactiveProperty _isPlaying = new BoolReactiveProperty();
        /// <summary>アニメーション再生</summary>
        public IReactiveProperty<bool> IsPlaying => _isPlaying;
        /// <summary>実行中に操作禁止にする対象</summary>
        private Transform _hookContent;
        /// <summary>実行中に操作禁止にする対象</summary>
        public Transform HookContent => _hookContent;

        protected override void Start()
        {
            base.Start();
            _prevPosition = transform.position;
            _prevTarget = defaultTarget;
        }

        public bool MoveSelectPlayer(Vector3 targetPosition, Transform currentTarget)
        {
            try
            {
                if (!_isPlaying.Value)
                {
                    _isPlaying.Value = true;
                    _hookContent = currentTarget;
                    if (_transform == null)
                        _transform = transform;

                    var scale = _transform.localScale;
                    scale.x = _prevPosition.x <= targetPosition.x ? 1f : -1f;
                    _transform.localScale = scale;
                    // 一つ前のターゲット位置と次のターゲット位置を調整（コードのみ）
                    if (froms.Length == tos.Length &&
                        tos.Length == toPositionCrrection.Length)
                    {
                        for (var i = 0; i < froms.Length; i++)
                        {
                            if (froms[i].Equals(_prevTarget) &&
                                tos[i].Equals(currentTarget))
                            {
                                targetPosition += toPositionCrrection[i];
                            }
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"格納数の不一致 from:[{froms.Length}]_to:[{tos.Length}]_toPositionCrrection:[{toPositionCrrection.Length}]");
                    }
                    _transform.DOMove(targetPosition, duration)
                        .OnComplete(() => _isPlaying.Value = false);
                    _prevPosition = targetPosition;
                    _prevTarget = currentTarget;
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool MoveSelectStageFrame(Vector3 targetPosition, Vector2 sizeDelta)
        {
            throw new System.NotImplementedException();
        }

        public bool SetColorAlpha(float alpha)
        {
            try
            {
                if (_image == null)
                    _image = GetComponent<Image>();
                var color = _image.color;
                color.a = alpha;
                _image.color = color;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SelectPlayer(Vector3 targetPosition, Transform currentTarget)
        {
            try
            {
                if (_transform == null)
                    _transform = transform;

                // T.B.D 一つ前のターゲット位置と次のターゲット位置を調整（コードのみ）
                if (froms.Length == tos.Length &&
                    tos.Length == toPositionCrrection.Length)
                {
                    for (var i = 0; i < froms.Length; i++)
                    {
                        if (froms[i].Equals(_prevTarget) &&
                            tos[i].Equals(currentTarget))
                        {
                            targetPosition += toPositionCrrection[i];
                        }
                    }
                }
                else
                {
                    Debug.LogWarning($"格納数の不一致 from:[{froms.Length}]_to:[{tos.Length}]_toPositionCrrection:[{toPositionCrrection.Length}]");
                }
                _transform.localPosition = targetPosition;
                _prevPosition = targetPosition;
                _prevTarget = currentTarget;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }
    }
}
