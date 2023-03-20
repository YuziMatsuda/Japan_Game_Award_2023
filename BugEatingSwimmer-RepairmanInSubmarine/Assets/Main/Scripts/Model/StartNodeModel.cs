using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;
using DG.Tweening;
using Main.Common;

namespace Main.Model
{
    /// <summary>
    /// モデル
    /// スタートノード
    /// </summary>
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(PivotConfig))]
    public class StartNodeModel : AbstractPivotModel, IStartNodeModel, IPivotModel
    {
        /// <summary>信号発生インターバル</summary>
        [SerializeField] private float postIntervalSeconds = 5f;
        /// <summary>方角モード</summary>
        private readonly IntReactiveProperty _enumDirectionMode = new IntReactiveProperty();
        /// <summary>方角モード</summary>
        public IReactiveProperty<int> EnumDirectionModeReact => _enumDirectionMode;
        /// <summary>方角モードのベクター配列</summary>
        [SerializeField] private Vector3[] vectorDirectionModes = { new Vector3(0, 0, 0f), new Vector3(0, 0, -90f), new Vector3(0, 0, 180f), new Vector3(0, 0, 90f) };

        protected override void Reset()
        {
            base.Reset();
            var enumDirectionModeDefault = GetComponent<PivotConfig>().EnumDirectionModeDefault;
            transform.localEulerAngles = vectorDirectionModes[(int)enumDirectionModeDefault];
        }

        protected override void Start()
        {
            base.Start();
            _enumDirectionMode.Value = (int)GetComponent<PivotConfig>().EnumDirectionModeDefault;
        }

        /// <summary>
        /// 信号発生まで待つ
        /// </summary>
        /// <param name="postIntervalSeconds">信号発生インターバル</param>
        /// <returns>コルーチン</returns>
        private IEnumerator WaitSignalPost(float postIntervalSeconds)
        {
            yield return new WaitForSeconds(postIntervalSeconds);
            if (!Posting(_isPosting, _toListLength))
                Debug.LogError("送信呼び出しの失敗");
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (0 < tags.Where(q => collision.CompareTag(q)).Select(q => q).ToArray().Length)
            {
                if (!Posting(_isPosting, _toListLength))
                    Debug.LogError("送信呼び出しの失敗");
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, Vector2.up * rayDistance);
            Gizmos.DrawRay(transform.position, Vector2.right * rayDistance);
            Gizmos.DrawRay(transform.position, Vector2.down * rayDistance);
            Gizmos.DrawRay(transform.position, Vector2.left * rayDistance);
        }

        /// <summary>
        /// 送信
        /// </summary>
        /// <param name="isPosting">信号発生アニメーション実行中</param>
        /// <param name="toListLength">POST先のノードコードリスト項目数</param>
        /// <returns>成功／失敗</returns>
        private bool Posting(BoolReactiveProperty isPosting, IntReactiveProperty toListLength)
        {
            try
            {
                if (!isPosting.Value)
                {
                    isPosting.Value = true;
                    var modes = new List<EnumDirectionMode>();
                    if (((EnumDirectionMode)_enumDirectionMode.Value).Equals(EnumDirectionMode.Up))
                        modes.Add(EnumDirectionMode.Up);
                    if (((EnumDirectionMode)_enumDirectionMode.Value).Equals(EnumDirectionMode.Right))
                        modes.Add(EnumDirectionMode.Right);
                    if (((EnumDirectionMode)_enumDirectionMode.Value).Equals(EnumDirectionMode.Down))
                        modes.Add(EnumDirectionMode.Down);
                    if (((EnumDirectionMode)_enumDirectionMode.Value).Equals(EnumDirectionMode.Left))
                        modes.Add(EnumDirectionMode.Left);
                    _toList = MainGameManager.Instance.AlgorithmOwner.GetSignalDestinations(modes.ToArray(), _transform, EnumCodeState.Normal, rayDistance, rayLayerMask);
                    toListLength.Value = _toList.Length;
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetIsPosting(bool isPostingValue)
        {
            try
            {
                if (_isPosting != null)
                    _isPosting.Value = isPostingValue;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetToListLength(int toListLength)
        {
            try
            {
                if (_toListLength != null)
                    _toListLength.Value = toListLength;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool WaitSignalPost()
        {
            try
            {
                StartCoroutine(WaitSignalPost(postIntervalSeconds));

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool GetSignal()
        {
            throw new System.NotImplementedException();
        }

        public bool GetSignal(bool isGetProcessStart)
        {
            try
            {
                if (!_isGetting.Value)
                {
                    _isGetting.Value = true;
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }
    }

    /// <summary>
    /// モデル
    /// スタートノード
    /// インターフェース
    /// </summary>
    public interface IStartNodeModel
    {
        /// <summary>
        /// 信号送信アニメーション実行中フラグをセット
        /// </summary>
        /// <param name="isPostingValue">信号発生アニメーション実行中</param>
        /// <returns>成功／失敗</returns>
        public bool SetIsPosting(bool isPostingValue);
        /// <summary>
        /// POST先のノードコードリスト数をセット
        /// </summary>
        /// <param name="toListLength">POST先のノードコードリスト数</param>
        /// <returns>成功／失敗</returns>
        public bool SetToListLength(int toListLength);
        /// <summary>
        /// 信号発生まで待つ
        /// </summary>
        /// <returns>コルーチン</returns>
        public bool WaitSignalPost();
    }
}
