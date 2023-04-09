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
        /// <summary>衝突判定の状態が無効k</summary>
        private bool _onTriggerEnter2DDisabled;
        /// <summary>通さない接触対象オブジェクトタグ</summary>
        [SerializeField] private string[] notLetPassTags = { ConstTagNames.TAG_NAME_DUSTCONNECTSIGNAL };
        /// <summary>読み取り専用フラグ</summary>
        private bool _readonlyCodeMode;

        protected override void Reset()
        {
            base.Reset();
            rayLayerMask = rayLayerMask | 1 << LayerMask.NameToLayer(ConstTagNames.TAG_NAME_ATOMS);
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
            // サン（ゴ）ショウコードであるかの状態をセット
            // ピボット側では動的な値として扱う
            _readonlyCodeMode = GetComponent<PivotConfig>().ReadonlyCodeMode;
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_onTriggerEnter2DDisabled &&
                GetComponent<PivotConfig>().EnumAtomicMode.Equals(EnumAtomicMode.Molecules) &&
                0 < tags.Where(q => collision.CompareTag(q)).Select(q => q).ToArray().Length &&
                shadowCodeCell != null &&
                lightCodeCell != null &&
                !_isTurning.Value)
            {
                // プレイヤーがパワー状態か
                var trigger = collision.GetComponent<AttackTrigger>();
                if (_readonlyCodeMode &&
                    trigger != null &&
                    trigger.IsPower.Value)
                {
                    _readonlyCodeMode = false;
                    // T.B.D パワー解除時に演出をつける？
                    if (!lightCodeCell.SetSprite(EnumPivotDynamic.PivotLight))
                        Debug.LogError("スプライトをセット呼び出しの失敗");
                }

                if (!_readonlyCodeMode)
                {
                    // 通常コードの振る舞い
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
                else
                {
                    // サン（ゴ）ショウコードとしての振る舞い
                    // 回転アニメーション
                    if (!lightCodeCell.SetAlphaOff())
                        Debug.LogError("アルファ値をセット呼び出しの失敗");
                    Observable.FromCoroutine<bool>(observer => shadowCodeCell.PlayLockSpinAnimation(observer))
                        .Subscribe(_ =>
                        {
                            if (!lightCodeCell.InitializeLight((EnumDirectionMode)_enumDirectionMode.Value))
                                Debug.LogError("アルファ値をセット呼び出しの失敗");
                        })
                        .AddTo(gameObject);
                }
            }
            else if (0 < notLetPassTags.Where(q => collision.CompareTag(q)).Select(q => q).ToArray().Length &&
                lightCodeCell != null)
            {
                // 向いている先がエラーならエラー演出
                if (IsErrorDirection(GetComponent<PivotConfig>().ErrorDirections, (EnumDirectionMode)_enumDirectionMode.Value))
                    Observable.FromCoroutine<bool>(observer => lightCodeCell.PlayErrorLightFlashAnimation(observer))
                        .Subscribe(_ => { })
                        .AddTo(gameObject);
            }
        }

        /// <summary>
        /// 今のコードの向きとエラーの向きを比較
        /// </summary>
        /// <param name="errorDirections">エラー方向モードの配列</param>
        /// <param name="enumDirectionModeValue">方角モード</param>
        /// <returns>エラーコード</returns>
        private bool IsErrorDirection(EnumDirectionMode[] errorDirections, EnumDirectionMode enumDirectionModeValue)
        {
            foreach (var item in errorDirections)
                if (item.Equals(enumDirectionModeValue))
                    return true;

            return false;
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

        /// <summary>
        /// 無効角度の取得
        /// </summary>
        /// <param name="prevNodeCode">一つ前のノード</param>
        /// <returns>無効角度（例外は-1）</returns>
        private int GetOtherIgnoreDirection(Transform prevNodeCode)
        {
            return GetOtherIgnoreDirection(prevNodeCode, false);
        }

        /// <summary>
        /// 無効角度の取得
        /// </summary>
        /// <param name="prevNodeCode">一つ前のノード</param>
        /// <param name="isGettingRoot">帰納法であるか</param>
        /// <returns>無効角度（例外は-1）</returns>
        private int GetOtherIgnoreDirection(Transform prevNodeCode, bool isGettingRoot)
        {
            try
            {
                var otherIgnoreDirection = 0;
                if (!isGettingRoot)
                {
                    // 演繹法
                    if (prevNodeCode.GetComponent<StartNodeModel>() != null)
                    {
                        otherIgnoreDirection = prevNodeCode.GetComponent<StartNodeModel>().EnumDirectionModeReact.Value;
                    }
                    else if (prevNodeCode.GetComponent<PivotModel>() != null)
                    {
                        otherIgnoreDirection = prevNodeCode.GetComponent<PivotModel>().EnumDirectionModeReact.Value;
                    }
                    else
                        throw new System.Exception("ゴールノードまたはコードの情報無し");
                }
                else
                {
                    // 帰納法
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
                }

                return otherIgnoreDirection;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return -1;
            }
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

                        var algorithmOwner = MainGameManager.Instance.AlgorithmOwner;
                        var prevNodeCode = algorithmOwner.GetHistorySignalsPostedLasted(_transform);
                        var otherIgnoreDirection = GetOtherIgnoreDirection(prevNodeCode);
                        if (otherIgnoreDirection < 0)
                            throw new System.Exception("無効角度の取得呼び出しの失敗");
                        if (GetComponent<PivotConfig>().EnumAtomicMode.Equals(EnumAtomicMode.Molecules) &&
                            !_algorithmCommon.GetAjustedEnumDirectionMode((EnumDirectionMode)otherIgnoreDirection, 2).Equals((EnumDirectionMode)_enumDirectionMode.Value))
                        {
                            var modes = GetModes((EnumDirectionMode)_enumDirectionMode.Value, GetComponent<PivotConfig>().ErrorDirections);
                            _toList = algorithmOwner.GetSignalDestinations(modes.ToArray(), _transform, EnumCodeState.Normal, rayDistance, rayLayerMask);
                            _toListLength.Value = _toList.Length;
                        }
                        else
                        {
                            _toListLength.Value = 0;
                        }
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
                        var otherIgnoreDirection = GetOtherIgnoreDirection(prevNodeCode, true)/*-1*/;
                        if (otherIgnoreDirection < 0)
                            throw new System.Exception("無効角度の取得呼び出しの失敗");

                        var fromList = new List<Transform>();
                        // 分子の場合かつ、自コードの向きと一つ前のコードの向きが正反対でない
                        if (GetComponent<PivotConfig>().EnumAtomicMode.Equals(EnumAtomicMode.Molecules) &&
                            !_algorithmCommon.GetAjustedEnumDirectionMode((EnumDirectionMode)otherIgnoreDirection, 2).Equals((EnumDirectionMode)_enumDirectionMode.Value))
                        {
                            // 自コード向きも考慮する
                            fromList.AddRange(algorithmOwner.GetSignalDestinations(GetModes((EnumDirectionMode)_enumDirectionMode.Value, GetComponent<PivotConfig>().ErrorDirections).ToArray(), _transform, EnumCodeState.Normal, rayDistance, rayLayerMask));
                        }
                        var merge = new List<Transform>();
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
        /// <param name="errorDirections">エラー方向モードの配列</param>
        /// <returns>対象方向モード配列</returns>
        private List<EnumDirectionMode> GetModes(EnumDirectionMode enumDirectionMode, EnumDirectionMode[] errorDirections)
        {
            return GetModes(enumDirectionMode, errorDirections, null);
        }

        /// <summary>
        /// ノードコードを辿る方向を取得
        /// </summary>
        /// <param name="enumDirectionMode">方向モード</param>
        /// <param name="errorDirections">エラー方向モードの配列</param>
        /// <param name="ignoreEnumDirectionModes">例外対象の方向モード配列</param>
        /// <returns>対象方向モード配列</returns>
        private List<EnumDirectionMode> GetModes(EnumDirectionMode enumDirectionMode, EnumDirectionMode[] errorDirections, EnumDirectionMode[] ignoreEnumDirectionModes)
        {
            var modes = new List<EnumDirectionMode>();
            if (ignoreEnumDirectionModes == null)
            {
                if (enumDirectionMode.Equals(EnumDirectionMode.Up) &&
                    !IsErrorDirection(errorDirections, enumDirectionMode))
                    modes.Add(EnumDirectionMode.Up);
                if (enumDirectionMode.Equals(EnumDirectionMode.Right) &&
                    !IsErrorDirection(errorDirections, enumDirectionMode))
                    modes.Add(EnumDirectionMode.Right);
                if (enumDirectionMode.Equals(EnumDirectionMode.Down) &&
                    !IsErrorDirection(errorDirections, enumDirectionMode))
                    modes.Add(EnumDirectionMode.Down);
                if (enumDirectionMode.Equals(EnumDirectionMode.Left) &&
                    !IsErrorDirection(errorDirections, enumDirectionMode))
                    modes.Add(EnumDirectionMode.Left);
            }
            else
            {
                if (ignoreEnumDirectionModes.Where(q => q.Equals(EnumDirectionMode.Up))
                    .Select(q => q)
                    .ToArray()
                    .Length < 1 &&
                    !IsErrorDirection(errorDirections, EnumDirectionMode.Up))
                {
                    modes.Add(EnumDirectionMode.Up);
                }
                if (ignoreEnumDirectionModes.Where(q => q.Equals(EnumDirectionMode.Right))
                    .Select(q => q)
                    .ToArray()
                    .Length < 1 &&
                    !IsErrorDirection(errorDirections, EnumDirectionMode.Right))
                {
                    // エラーコードのチェックした後に対象外チェック
                    modes.Add(EnumDirectionMode.Right);
                }
                if (ignoreEnumDirectionModes.Where(q => q.Equals(EnumDirectionMode.Down))
                    .Select(q => q)
                    .ToArray()
                    .Length < 1 &&
                    !IsErrorDirection(errorDirections, EnumDirectionMode.Down))
                {
                    modes.Add(EnumDirectionMode.Down);
                }
                if (ignoreEnumDirectionModes.Where(q => q.Equals(EnumDirectionMode.Left))
                    .Select(q => q)
                    .ToArray()
                    .Length < 1 &&
                    !IsErrorDirection(errorDirections, EnumDirectionMode.Left))
                {
                    modes.Add(EnumDirectionMode.Left);
                }
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

        public bool SetOnTriggerEnter2DDisabled(bool onTriggerEnter2DDisabled)
        {
            try
            {
                _onTriggerEnter2DDisabled = onTriggerEnter2DDisabled;

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
        /// <summary>
        /// OnTriggerEnter判定をセット
        /// </summary>
        /// <param name="onTriggerEnter2DDisabled">無効とするか</param>
        /// <returns>成功／失敗</returns>
        public bool SetOnTriggerEnter2DDisabled(bool onTriggerEnter2DDisabled);
    }
}
