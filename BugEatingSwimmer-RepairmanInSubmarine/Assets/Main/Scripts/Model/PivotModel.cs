using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;
using Main.Common;
using DG.Tweening;
using Main.View;

namespace Main.Model
{
    /// <summary>
    /// モデル
    /// 始点
    /// オブジェクトごとにコライダーは複数存在する想定のためRequireComponentでコライダー有無はチェックしない
    /// </summary>
    [RequireComponent(typeof(PivotConfig))]
    public class PivotModel : AbstractPivotModel, IPivotModel, IStartNodeModel, IGoalNodeModel
    {
        /// <summary>ターンアニメーション実行中</summary>
        private readonly BoolReactiveProperty _isTurning = new BoolReactiveProperty();
        /// <summary>ターンアニメーション実行中</summary>
        public IReactiveProperty<bool> IsTurning => _isTurning;
        /// <summary>方角モード</summary>
        private readonly IntReactiveProperty _enumDirectionMode = new IntReactiveProperty();
        /// <summary>方角モード</summary>
        public IReactiveProperty<int> EnumDirectionModeReact => _enumDirectionMode;
        /// <summary>方角モードのベクター配列</summary>
        [SerializeField] private Vector3[] vectorDirectionModes = { new Vector3(0, 0, 0f), new Vector3(0, 0, -90f), new Vector3(0, 0, 180f), new Vector3(0, 0, 90f) };
        /// <summary>回転方角モード</summary>
        [SerializeField] private EnumSpinDirectionMode isSpinDirectionMode = EnumSpinDirectionMode.Positive;
        /// <summary>アルゴリズムの共通処理</summary>
        private AlgorithmCommon _algorithmCommon = new AlgorithmCommon();
        /// <summary>コード挙動制御</summary>
        [SerializeField] private ShadowCodeCell shadowCodeCell;
        /// <summary>コード（明）挙動制御</summary>
        [SerializeField] private LightCodeCell lightCodeCell;

        protected override void Reset()
        {
            base.Reset();
            rayLayerMask = rayLayerMask | 1 << LayerMask.NameToLayer(ConstTagNames.TAG_NAME_GOALNODE);
            rayLayerMask = rayLayerMask | 1 << LayerMask.NameToLayer(ConstTagNames.TAG_NAME_STARTNODE);

            shadowCodeCell = transform.GetChild(0).GetChild(0).GetComponent<ShadowCodeCell>() != null ? transform.GetChild(0).GetChild(0).GetComponent<ShadowCodeCell>() : null;
            lightCodeCell = transform.GetChild(1).GetComponent<LightCodeCell>() != null ? transform.GetChild(1).GetComponent<LightCodeCell>() : null;
            if (GetComponent<PivotConfig>().EnumAtomicMode.Equals(EnumAtomicMode.Molecules))
            {
                var enumDirectionModeDefault = GetComponent<PivotConfig>().EnumDirectionModeDefault;
                shadowCodeCell.transform.localEulerAngles = vectorDirectionModes[(int)enumDirectionModeDefault];
                lightCodeCell.transform.localEulerAngles = vectorDirectionModes[(int)enumDirectionModeDefault];
            }
        }

        protected override void Start()
        {
            base.Start();
            _enumDirectionMode.Value = (int)GetComponent<PivotConfig>().EnumDirectionModeDefault;

            if (GetComponent<PivotConfig>().EnumAtomicMode.Equals(EnumAtomicMode.Molecules) &&
                lightCodeCell != null)
            {
                if (!lightCodeCell.SetAlphaOff())
                    Debug.LogError("アルファ値をセット呼び出しの失敗");
                if (!lightCodeCell.InitializeLight((EnumDirectionMode)_enumDirectionMode.Value))
                    Debug.LogError("アルファ値をセット呼び出しの失敗");
            }
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (GetComponent<PivotConfig>().EnumAtomicMode.Equals(EnumAtomicMode.Molecules) &&
                0 < tags.Where(q => collision.CompareTag(q)).Select(q => q).ToArray().Length &&
                shadowCodeCell != null &&
                lightCodeCell != null &&
                !_isTurning.Value)
            {
                _isTurning.Value = true;
                var turnValue = GetTurnValue(_transform, collision.ClosestPoint(_transform.position), isSpinDirectionMode);
                if (turnValue == 0)
                    Debug.LogError("ターン加算値の取得呼び出しの失敗");
                _enumDirectionMode.Value = (int)_algorithmCommon.GetAjustedEnumDirectionMode((EnumDirectionMode)_enumDirectionMode.Value, turnValue);
                // 回転アニメーション
                if (!lightCodeCell.SetAlphaOff())
                    Debug.LogError("アルファ値をセット呼び出しの失敗");
                Observable.FromCoroutine<bool>(observer => shadowCodeCell.PlaySpinAnimation(observer, vectorDirectionModes[_enumDirectionMode.Value]))
                    .Subscribe(_ =>
                    {
                        if (!lightCodeCell.SetSpinDirection(vectorDirectionModes[_enumDirectionMode.Value]))
                            Debug.LogError("回転方角セット呼び出しの失敗");
                        Observable.FromCoroutine<bool>(observer => lightCodeCell.PlayLightAnimation(observer, (EnumDirectionMode)_enumDirectionMode.Value))
                            .Subscribe(_ =>
                            {
                                _isTurning.Value = false;
                            })
                            .AddTo(gameObject);
                    })
                    .AddTo(gameObject);
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
        /// ターン加算値の取得
        /// </summary>
        /// <param name="transform">トランスフォーム</param>
        /// <param name="target">ターゲットの座標</param>
        /// <returns>ターン加算値</returns>
        private int GetTurnValue(Transform transform, Vector2 target, EnumSpinDirectionMode enumSpinDirectionMode)
        {
            try
            {
                switch (enumSpinDirectionMode)
                {
                    case EnumSpinDirectionMode.Auto:
                        if (transform.position.y == target.y ||
                               transform.position.x == target.x)
                            throw new System.Exception("同一座標位置は例外エラー");

                        if (transform.position.x < target.x)
                        {
                            // 上かつ、右からの衝突は反時計回り
                            // 下かつ、右からの衝突は時計回り
                            return transform.position.y < target.y ? -1 : 1;
                        }
                        else
                        {
                            // 上かつ、左からの衝突は時計回り
                            // 下かつ、左からの衝突は反時計回り
                            return transform.position.y < target.y ? 1 : -1;
                        }
                    case EnumSpinDirectionMode.Positive:
                        return 1;
                    case EnumSpinDirectionMode.Negative:
                        return -1;
                    default:
                        throw new System.Exception("例外エラー");
                }

            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return 0;
            }
        }

        public bool GetSignal()
        {
            return GetSignal(false);
        }

        public bool GetSignal(bool isGetProcessStart)
        {
            try
            {
                if (!isGetProcessStart)
                {
                    // 演繹法ルート
                    if (!_isPosting.Value)
                    {
                        _isPosting.Value = true;
                        var modes = GetModes((EnumDirectionMode)_enumDirectionMode.Value);
                        _toList = MainGameManager.Instance.AlgorithmOwner.GetSignalDestinations(modes.ToArray(), _transform, EnumCodeState.Normal, rayDistance, rayLayerMask);
                        _toListLength.Value = _toList.Length;
                    }
                }
                else
                {
                    // 帰納法ルート
                    if (!_isGetting.Value)
                    {
                        _isGetting.Value = true;
                        var algorithmOwner = MainGameManager.Instance.AlgorithmOwner;
                        var prevNodeCode = algorithmOwner.GetHistorySignalsGetedLasted(_transform);
                        var otherIgnoreDirection = -1;
                        if (prevNodeCode.GetComponent<GoalNodeModel>() != null)
                        {
                            otherIgnoreDirection = prevNodeCode.GetComponent<GoalNodeModel>().EnumDirectionModeReact.Value;
                        }
                        else if (prevNodeCode.GetComponent<PivotModel>() != null)
                        {
                            otherIgnoreDirection = prevNodeCode.GetComponent<PivotModel>().EnumDirectionModeReact.Value;
                        }
                        else
                            throw new System.Exception("ゴールノードまたはコードの情報無し");

                        var modes = GetModes((EnumDirectionMode)_enumDirectionMode.Value, new EnumDirectionMode[1] { _algorithmCommon.GetAjustedEnumDirectionMode((EnumDirectionMode)otherIgnoreDirection, 2) });
                        var fromListReverse = algorithmOwner.GetSignalDestinationsReverse(modes.ToArray(), _transform, EnumCodeState.Normal, rayDistance, rayLayerMask);
                        var fromList = new List<Transform>();
                        if (GetComponent<PivotConfig>().EnumAtomicMode.Equals(EnumAtomicMode.Molecules) &&
                            !_algorithmCommon.GetAjustedEnumDirectionMode((EnumDirectionMode)otherIgnoreDirection, 2).Equals((EnumDirectionMode)_enumDirectionMode.Value))
                        {
                            fromList.AddRange(algorithmOwner.GetSignalDestinations(GetModes((EnumDirectionMode)_enumDirectionMode.Value).ToArray(), _transform, EnumCodeState.Normal, rayDistance, rayLayerMask));
                        }
                        var merge = new List<Transform>();
                        if (0 < fromListReverse.Length)
                            merge.AddRange(fromListReverse);
                        if (0 < fromList.Count)
                            merge.AddRange(fromList);
                        _fromList = merge.ToArray();
                        _fromListLength.Value = _fromList.Length;
                    }
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// ノードコードを辿る方向を取得
        /// </summary>
        /// <param name="enumDirectionMode">方向モード</param>
        /// <returns>対象方向モード配列</returns>
        private List<EnumDirectionMode> GetModes(EnumDirectionMode enumDirectionMode)
        {
            return GetModes(enumDirectionMode, null);
        }

        /// <summary>
        /// ノードコードを辿る方向を取得
        /// </summary>
        /// <param name="enumDirectionMode">方向モード</param>
        /// <param name="ignoreEnumDirectionModes">例外対象の方向モード配列</param>
        /// <returns>対象方向モード配列</returns>
        private List<EnumDirectionMode> GetModes(EnumDirectionMode enumDirectionMode, EnumDirectionMode[] ignoreEnumDirectionModes)
        {
            var modes = new List<EnumDirectionMode>();
            if (ignoreEnumDirectionModes == null)
            {
                if (enumDirectionMode.Equals(EnumDirectionMode.Up))
                    modes.Add(EnumDirectionMode.Up);
                if (enumDirectionMode.Equals(EnumDirectionMode.Right))
                    modes.Add(EnumDirectionMode.Right);
                if (enumDirectionMode.Equals(EnumDirectionMode.Down))
                    modes.Add(EnumDirectionMode.Down);
                if (enumDirectionMode.Equals(EnumDirectionMode.Left))
                    modes.Add(EnumDirectionMode.Left);
            }
            else
            {
                if (ignoreEnumDirectionModes.Where(q => q.Equals(EnumDirectionMode.Up)).Select(q => q).ToArray().Length < 1)
                    modes.Add(EnumDirectionMode.Up);
                if (ignoreEnumDirectionModes.Where(q => q.Equals(EnumDirectionMode.Right)).Select(q => q).ToArray().Length < 1)
                    modes.Add(EnumDirectionMode.Right);
                if (ignoreEnumDirectionModes.Where(q => q.Equals(EnumDirectionMode.Down)).Select(q => q).ToArray().Length < 1)
                    modes.Add(EnumDirectionMode.Down);
                if (ignoreEnumDirectionModes.Where(q => q.Equals(EnumDirectionMode.Left)).Select(q => q).ToArray().Length < 1)
                    modes.Add(EnumDirectionMode.Left);
            }

            return modes;
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
            throw new System.NotImplementedException();
        }
    }

    /// <summary>
    /// モデル
    /// 始点
    /// インターフェース
    /// </summary>
    public interface IPivotModel
    {
        /// <summary>
        /// シグナル受信
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool GetSignal();
        /// <summary>
        /// シグナル受信
        /// </summary>
        /// <param name="isGetProcessStart">帰納法フラグ</param>
        /// <returns>成功／失敗</returns>
        public bool GetSignal(bool isGetProcessStart);
    }
}
