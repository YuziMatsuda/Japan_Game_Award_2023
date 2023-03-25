using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Common;
using UniRx;

namespace Main.Model
{
    /// <summary>
    /// モデル
    /// ゴールノード
    /// </summary>
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(PivotConfig))]
    public class GoalNodeModel : AbstractPivotModel, IPivotModel, IStartNodeModel, IGoalNodeModel
    {
        /// <summary>方角モードのベクター配列</summary>
        [SerializeField] private Vector3[] vectorDirectionModes = { new Vector3(0, 0, 0f), new Vector3(0, 0, -90f), new Vector3(0, 0, 180f), new Vector3(0, 0, 90f) };
        /// <summary>方角モード</summary>
        private readonly IntReactiveProperty _enumDirectionMode = new IntReactiveProperty();
        /// <summary>方角モード</summary>
        public IReactiveProperty<int> EnumDirectionModeReact => _enumDirectionMode;

        public bool GetSignal()
        {
            try
            {
                if (!_isPosting.Value)
                {
                    _isPosting.Value = true;
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
            throw new System.NotImplementedException();
        }

        public bool WaitSignalPost()
        {
            throw new System.NotImplementedException();
        }

        protected override void Reset()
        {
            base.Reset();
            if (GetComponent<PivotConfig>().EnumAtomicMode.Equals(EnumAtomicMode.Molecules))
                rayLayerMask = rayLayerMask | 1 << LayerMask.NameToLayer(ConstTagNames.TAG_NAME_ATOMS);
            var enumDirectionModeDefault = GetComponent<PivotConfig>().EnumDirectionModeDefault;
            transform.localEulerAngles = vectorDirectionModes[(int)enumDirectionModeDefault];
        }

        protected override void Start()
        {
            base.Start();
            _enumDirectionMode.Value = (int)GetComponent<PivotConfig>().EnumDirectionModeDefault;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, Vector2.up * rayDistance);
            Gizmos.DrawRay(transform.position, Vector2.right * rayDistance);
            Gizmos.DrawRay(transform.position, Vector2.down * rayDistance);
            Gizmos.DrawRay(transform.position, Vector2.left * rayDistance);
        }

        public bool SetIsGetting(bool isGettingValue)
        {
            try
            {
                if (_isGetting != null)
                    _isGetting.Value = isGettingValue;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool Getting()
        {
            try
            {
                var modes = new List<EnumDirectionMode>();
                if (((EnumDirectionMode)_enumDirectionMode.Value).Equals(EnumDirectionMode.Up))
                    modes.Add(EnumDirectionMode.Up);
                if (((EnumDirectionMode)_enumDirectionMode.Value).Equals(EnumDirectionMode.Right))
                    modes.Add(EnumDirectionMode.Right);
                if (((EnumDirectionMode)_enumDirectionMode.Value).Equals(EnumDirectionMode.Down))
                    modes.Add(EnumDirectionMode.Down);
                if (((EnumDirectionMode)_enumDirectionMode.Value).Equals(EnumDirectionMode.Left))
                    modes.Add(EnumDirectionMode.Left);
                _fromList = MainGameManager.Instance.AlgorithmOwner.GetSignalDestinations(modes.ToArray(), _transform, EnumCodeState.Normal, rayDistance, rayLayerMask);
                _fromListLength.Value = _fromList.Length;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool GetSignal(bool isGetProcessStart)
        {
            throw new System.NotImplementedException();
        }

        public bool SetFromListLength(int fromListLength)
        {
            try
            {
                if (_fromListLength != null)
                    _fromListLength.Value = fromListLength;

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
    /// ゴールノード
    /// インターフェース
    /// </summary>
    public interface IGoalNodeModel
    {
        /// <summary>
        /// 信号受信フラグをセット
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool SetIsGetting(bool isGettingValue);
        /// <summary>
        /// コード元を辿る
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool Getting();
        /// <summary>
        /// GET元のノードコードリスト数をセット
        /// </summary>
        /// <param name="fromListLength">GET元のノードコードリスト数</param>
        /// <returns>成功／失敗</returns>
        public bool SetFromListLength(int fromListLength);
    }
}
