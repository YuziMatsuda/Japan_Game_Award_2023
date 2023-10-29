using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;
using Main.Common;
using DG.Tweening;
using Main.View;
using Main.Audio;

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
        /// <summary>ターン失敗アニメーション実行中</summary>
        private readonly BoolReactiveProperty _isTurningFaild = new BoolReactiveProperty();
        /// <summary>ターン失敗アニメーション実行中</summary>
        public IReactiveProperty<bool> IsTurningFaild => _isTurningFaild;
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
        /// <summary>感情コードを通過</summary>
        private readonly BoolReactiveProperty _isPathEmotions = new BoolReactiveProperty();
        /// <summary>感情コードを通過</summary>
        public IReactiveProperty<bool> IsPathEmotions => _isPathEmotions;
        /// <summary>設定</summary>
        [SerializeField] private PivotConfig pivotConfig;
        /// <summary>回転アニメーションを一時ロック管理フラグ</summary>
        private bool _isLockedPlaySpinAnimations;
        /// <summary>回転アニメーションの待機時間</summary>
        [SerializeField] private float delayTimeOfLockedPlaySpinAnimations = .35f;

        protected override void Reset()
        {
            base.Reset();
            rayLayerMask = rayLayerMask | 1 << LayerMask.NameToLayer(ConstTagNames.TAG_NAME_ATOMS);
            rayLayerMask = rayLayerMask | 1 << LayerMask.NameToLayer(ConstTagNames.TAG_NAME_GOALNODE);
            rayLayerMask = rayLayerMask | 1 << LayerMask.NameToLayer(ConstTagNames.TAG_NAME_STARTNODE);

            shadowCodeCell = transform.GetChild(0).GetChild(0).GetComponent<ShadowCodeCell>() != null ? transform.GetChild(0).GetChild(0).GetComponent<ShadowCodeCell>() : null;
            lightCodeCell = transform.GetChild(1).GetComponent<LightCodeCell>() != null ? transform.GetChild(1).GetComponent<LightCodeCell>() : null;
            pivotConfig = GetComponent<PivotConfig>();
            if (pivotConfig.EnumAtomicMode.Equals(EnumAtomicMode.Molecules))
            {
                var enumDirectionModeDefault = pivotConfig.EnumDirectionModeDefault;
                shadowCodeCell.transform.localEulerAngles = vectorDirectionModes[(int)enumDirectionModeDefault];
                lightCodeCell.transform.localEulerAngles = vectorDirectionModes[(int)enumDirectionModeDefault];
            }
        }

        protected override void Start()
        {
            base.Start();
            _enumDirectionMode.Value = (int)pivotConfig.EnumDirectionModeDefault;

            if (pivotConfig.EnumAtomicMode.Equals(EnumAtomicMode.Molecules) &&
                lightCodeCell != null)
            {
                if (!lightCodeCell.SetAlphaOff())
                    Debug.LogError("アルファ値をセット呼び出しの失敗");
                if (!lightCodeCell.InitializeLight((EnumDirectionMode)_enumDirectionMode.Value))
                    Debug.LogError("アルファ値をセット呼び出しの失敗");
            }
            // サン（ゴ）ショウコードであるかの状態をセット
            // ピボット側では動的な値として扱う
            if (pivotConfig.ReadonlyCodeMode)
                if (!shadowCodeCell.SetDefaultDirection())
                    Debug.LogError("デフォルト角度をセット呼び出しの失敗");
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_onTriggerEnter2DDisabled &&
                pivotConfig.EnumAtomicMode.Equals(EnumAtomicMode.Molecules) &&
                0 < tags.Where(q => collision.CompareTag(q)).Select(q => q).ToArray().Length &&
                shadowCodeCell != null &&
                lightCodeCell != null)
            {
                // プレイヤーがパワー状態か
                var trigger = collision.GetComponent<AttackTrigger>();
                if (pivotConfig.ReadonlyCodeMode &&
                    trigger != null &&
                    trigger.IsPower.Value &&
                    trigger.IsPressAndHoldAndReleased.Value)
                {
                    if (!pivotConfig.SetReadonlyCodeMode(false))
                        Debug.LogError("サン（ゴ）ショウコードであるかをセット呼び出しの失敗");
                    // パワー解除時にサン（ゴ）ショウコードの破片がとぶ
                    foreach (var item in pivotConfig.CoralParts)
                        Instantiate(item, _transform.position, Quaternion.identity, _transform);
                    if (!lightCodeCell.SetSprite(EnumPivotDynamic.PivotLight))
                        Debug.LogError("スプライトをセット呼び出しの失敗");
                    if (!shadowCodeCell.SetSprite(EnumPivotDynamic.PivotLight))
                        Debug.LogError("スプライトをセット呼び出しの失敗");
                    // プレイヤーのパワー状態を解除
                    if (!trigger.SetIsPower(false))
                        Debug.LogError("パワー状態をセット呼び出しの失敗");
                }

                if (!pivotConfig.ReadonlyCodeMode)
                {
                    if (!_isLockedPlaySpinAnimations)
                    {
                        _isLockedPlaySpinAnimations = true;
                        DOVirtual.DelayedCall(delayTimeOfLockedPlaySpinAnimations, () => _isLockedPlaySpinAnimations = false);
                        // コード回転のSE
                        MainGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_code_normal);
                        PlaySpinAnimations(collision, _isTurning.Value,
                            _isTurning, _transform, isSpinDirectionMode, _enumDirectionMode, _algorithmCommon,
                            lightCodeCell, shadowCodeCell, vectorDirectionModes, pivotConfig,
                            gameObject);
                    }
                }
                else
                {
                    if (!_isTurningFaild.Value)
                    {
                        // サン（ゴ）ショウコードとしての振る舞い
                        // 回転アニメーション
                        if (!lightCodeCell.SetAlphaOff())
                            Debug.LogError("アルファ値をセット呼び出しの失敗");
                        _isTurningFaild.Value = true;
                        Observable.FromCoroutine<bool>(observer => shadowCodeCell.PlayLockSpinAnimation(observer))
                            .Subscribe(_ =>
                            {
                                if (!lightCodeCell.InitializeLight((EnumDirectionMode)_enumDirectionMode.Value))
                                    Debug.LogError("アルファ値をセット呼び出しの失敗");
                                _isTurningFaild.Value = false;
                            })
                            .AddTo(gameObject);
                    }
                }
            }
            else if (0 < notLetPassTags.Where(q => collision.CompareTag(q)).Select(q => q).ToArray().Length &&
                lightCodeCell != null)
            {
                // 向いている先がエラーならエラー演出
                if (IsErrorDirection(pivotConfig.ErrorDirections, (EnumDirectionMode)_enumDirectionMode.Value))
                {
                    // エラーSE
                    MainGameManager.Instance.AudioOwner.PlaySFX(Audio.ClipToPlay.se_code_error);
                    Observable.FromCoroutine<bool>(observer => lightCodeCell.PlayErrorLightFlashAnimation(observer))
                        .Subscribe(_ => { })
                        .AddTo(gameObject);
                }
                // 感情コードの判定
                if (pivotConfig.EmotionsCodeMode &&
                    !_isPathEmotions.Value)
                {
                    // エラーSE
                    //Debug.LogWarning("感情コード");
                    _isPathEmotions.Value = true;
                }
            }
        }

        /// <summary>
        /// 回転アニメーションの再生
        /// 関連するコード全て
        /// </summary>
        /// <param name="collision">2Dコリジョン</param>
        /// <param name="restartMode">リスタートモード</param>
        /// <param name="isTurning">ターンアニメーション実行中</param>
        /// <param name="transform">トランスフォーム</param>
        /// <param name="isSpinDirectionMode">回転方向モード</param>
        /// <param name="enumDirectionMode">方角モード</param>
        /// <param name="algorithmCommon">アルゴリズムの共通処理</param>
        /// <param name="lightCodeCell">コード（明）挙動制御</param>
        /// <param name="shadowCodeCell">コード挙動制御</param>
        /// <param name="vectorDirectionModes">方角モードのベクター配列</param>
        /// <param name="pivotConfig">設定</param>
        /// <param name="gameObject">ゲームオブジェクト</param>
        private void PlaySpinAnimations(Collider2D collision, bool restartMode,
            BoolReactiveProperty isTurning, Transform transform, EnumSpinDirectionMode isSpinDirectionMode, IntReactiveProperty enumDirectionMode, AlgorithmCommon algorithmCommon,
            LightCodeCell lightCodeCell, ShadowCodeCell shadowCodeCell, Vector3[] vectorDirectionModes, PivotConfig pivotConfig,
            GameObject gameObject)
        {
            // 再生中の場合は一度状態をリセット
            if (restartMode &&
                isTurning.Value)
                isTurning.Value = false;
            // 通常コードの振る舞い
            isTurning.Value = true;
            var turnValue = GetTurnValue(transform, collision.ClosestPoint(transform.position), isSpinDirectionMode);
            if (turnValue == 0)
                Debug.LogError("ターン加算値の取得呼び出しの失敗");
            enumDirectionMode.Value = (int)algorithmCommon.GetAjustedEnumDirectionMode((EnumDirectionMode)enumDirectionMode.Value, turnValue);
            // 回転アニメーション
            if (!lightCodeCell.SetAlphaOff())
                Debug.LogError("アルファ値をセット呼び出しの失敗");
            Observable.FromCoroutine<bool>(observer => shadowCodeCell.PlaySpinAnimation(observer, vectorDirectionModes[enumDirectionMode.Value], restartMode))
                .Subscribe(_ =>
                {
                    if (!lightCodeCell.SetSpinDirection(vectorDirectionModes[enumDirectionMode.Value]))
                        Debug.LogError("回転方角セット呼び出しの失敗");
                    Observable.FromCoroutine<bool>(observer => lightCodeCell.PlayLightAnimation(observer, (EnumDirectionMode)enumDirectionMode.Value, restartMode))
                        .Subscribe(_ => PlaySpinAnimationsFinal(isTurning, pivotConfig, shadowCodeCell),
                            () => PlaySpinAnimationsFinal(isTurning, pivotConfig, shadowCodeCell))
                        .AddTo(gameObject);
                })
                .AddTo(gameObject);
        }

        /// <summary>
        /// 回転アニメーションの再生
        /// 関連するコード全て
        /// 最終処理
        /// </summary>
        /// <param name="isTurning">ターンアニメーション実行中</param>
        /// <param name="pivotConfig">設定</param>
        /// <param name="shadowCodeCell">コード挙動制御</param>
        private void PlaySpinAnimationsFinal(BoolReactiveProperty isTurning, PivotConfig pivotConfig, ShadowCodeCell shadowCodeCell)
        {
            isTurning.Value = false;
            if (pivotConfig.EnumInteractID.Equals(EnumInteractID.IN0000))
            {
                if (!pivotConfig.SetEnumInteractID(EnumInteractID.IN0001))
                    Debug.LogError("インタラクトIDをセット呼び出しの失敗");
                if (!pivotConfig.SetReadonlyCodeMode(true))
                    Debug.LogError("サン（ゴ）ショウコードであるかをセット呼び出しの失敗");
                if (pivotConfig.ReadonlyCodeMode)
                    if (!shadowCodeCell.SetDefaultDirection())
                        Debug.LogError("デフォルト角度をセット呼び出しの失敗");
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
                        if (pivotConfig.EnumAtomicMode.Equals(EnumAtomicMode.Molecules) &&
                            !_algorithmCommon.GetAjustedEnumDirectionMode((EnumDirectionMode)otherIgnoreDirection, 2).Equals((EnumDirectionMode)_enumDirectionMode.Value))
                        {
                            var modes = GetModes((EnumDirectionMode)_enumDirectionMode.Value, pivotConfig.ErrorDirections);
                            _toList = algorithmOwner.GetSignalDestinations(modes.ToArray(), _transform, EnumCodeState.Normal, rayDistance, rayLayerMask);
                            _toListLength.Value = _toList.Length;
                        }
                        else
                        {
                            _toListLength.Value = 0;
                        }
                    }
                    else
                    {
                        _toListLength.Value = 0;
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
                        if (pivotConfig.EnumAtomicMode.Equals(EnumAtomicMode.Molecules) &&
                            !_algorithmCommon.GetAjustedEnumDirectionMode((EnumDirectionMode)otherIgnoreDirection, 2).Equals((EnumDirectionMode)_enumDirectionMode.Value))
                        {
                            // 自コード向きも考慮する
                            fromList.AddRange(algorithmOwner.GetSignalDestinations(GetModes((EnumDirectionMode)_enumDirectionMode.Value, pivotConfig.ErrorDirections).ToArray(), _transform, EnumCodeState.Normal, rayDistance, rayLayerMask));
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

        public bool SetIsPathEmotions(bool enabled)
        {
            try
            {
                _isPathEmotions.Value = enabled;

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
        /// <summary>
        /// 感情コード通過状態をセット
        /// </summary>
        /// <param name="enabled">有効／無効</param>
        /// <returns>成功／失敗</returns>
        public bool SetIsPathEmotions(bool enabled);
    }
}
