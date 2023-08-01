using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Common;
using System.Linq;
using UniRx;
using DG.Tweening;

namespace Main.Model
{
    /// <summary>
    /// ジョーシー
    /// モデル
    /// </summary>
    [RequireComponent(typeof(LoinclothConfig))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class JawsHiModel : MonoBehaviour, IJawsHiModel, ISearchCollider, IDangerCollider
    {
        /// <summary>設定</summary>
        [SerializeField] private LoinclothConfig loinclothConfig;
        /// <summary>設定</summary>
        public LoinclothConfig LoinclothConfig => loinclothConfig;
        /// <summary>ターゲット</summary>
        private Transform[] _targets;
        /// <summary>アニメーション</summary>
        private Tweener _sequence;
        /// <summary>トランスフォーム</summary>
        private Transform _transform;
        /// <summary>アニメーション終了時間</summary>
        [SerializeField] private float[] durations = { 9f };
        /// <summary>サーチコライダー</summary>
        [SerializeField] private SearchCollider searchCollider;
        /// <summary>接触位置</summary>
        public IReactiveProperty<Vector3> HitPosition => searchCollider.HitPosition;
        /// <summary>死亡判定のコライダー</summary>
        [SerializeField] private DangerCollider dangerCollider;
        /// <summary>接触するか</summary>
        public IReactiveProperty<bool> IsHit => dangerCollider.IsHit;
        /// <summary>向き変更を無効とするか</summary>
        private readonly BoolReactiveProperty _lookAtDisabled = new BoolReactiveProperty();
        /// <summary>後ろを向いているか</summary>
        private readonly BoolReactiveProperty _isBack = new BoolReactiveProperty();
        /// <summary>後ろを向いているか</summary>
        public IReactiveProperty<bool> IsBack => _isBack;

        private void Reset()
        {
            loinclothConfig = GetComponent<LoinclothConfig>();
            searchCollider = GetComponentInChildren<SearchCollider>();
            dangerCollider = GetComponentInChildren<DangerCollider>();
        }

        private void Start()
        {
            // 同じ部署に所属している社員をターゲットへ設定
            var objs = GameObject.FindGameObjectsWithTag(ConstTagNames.TAG_NAME_LOINCLOTH);
            if (objs != null &&
                0 < objs.Length)
            {
                List<Transform> targetList = new List<Transform>();
                var ary = objs.Where(q => q.GetComponent<LoinclothModel>() != null &&
                    q.GetComponent<LoinclothModel>().LoinclothConfig.EnumDepartmentCode.Equals(loinclothConfig.EnumDepartmentCode))
                    .OrderBy(q => q.GetComponent<LoinclothModel>().Index)
                    .Select(q => q.transform)
                    .ToArray();
                // ターゲットのみだとルートが1周とならないため最初のポイントを最後に追加する
                targetList.AddRange(ary);
                _targets = targetList.ToArray();
            }

            if (!PlayTrackingMoveAnimation())
                Debug.LogError("追跡移動するアニメーションを再生呼び出しの失敗");

            if (_transform)
                _transform = transform;
            Vector3ReactiveProperty toDirectionLast = new Vector3ReactiveProperty(Vector3.right);
            _transform.ObserveEveryValueChanged(x => x.position)
                .Subscribe(x =>
                {
                    if (!_lookAtDisabled.Value)
                    {
                        if (x.x < toDirectionLast.Value.x)
                        {
                            _transform.rotation = Quaternion.FromToRotation(Vector2.right, Vector3.right);
                            _isBack.Value = true;
                        }
                        else if (toDirectionLast.Value.x < x.x)
                        {
                            _transform.rotation = Quaternion.FromToRotation(Vector2.right, Vector3.left);
                            _isBack.Value = false;
                        }
                        toDirectionLast.Value = x;
                    }
                });
        }

        public bool SetHitState(bool isEnabled)
        {
            return ((ISearchCollider)searchCollider).SetHitState(isEnabled);
        }

        public bool StopTrackingMoveAnimation()
        {
            try
            {
                if (_sequence != null)
                    _sequence.Pause();

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool PlayTrackingMoveAnimation()
        {
            try
            {
                if (_transform == null)
                    _transform = transform;
                if (_sequence == null)
                    _sequence = _transform.DOPath(_targets.Select(q => q.transform.position)
                        .ToArray(), durations[0], PathType.Linear, PathMode.Sidescroller2D)
                        .SetOptions(true)
                        .SetLoops(-1, LoopType.Restart);
                else if (!_sequence.IsPlaying())
                    _sequence.Play();
                else
                {
                    // 再生中
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetLookAtDisabled(bool isDisabled)
        {
            try
            {
                _lookAtDisabled.Value = isDisabled;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetCollider2DEnabled(bool isEnabled)
        {
            return ((IDangerCollider)dangerCollider).SetCollider2DEnabled(isEnabled);
        }

        public bool SetIsCollisionBan(bool isEnabled)
        {
            return ((IDangerCollider)dangerCollider).SetIsCollisionBan(isEnabled);
        }
    }


    /// <summary>
    /// ジョーシー
    /// モデル
    /// インターフェース
    /// </summary>
    public interface IJawsHiModel
    {
        /// <summary>
        /// 追跡移動するアニメーションを再生
        /// </summary>
        /// <returns>コルーチン</returns>
        public bool PlayTrackingMoveAnimation();
        /// <summary>
        /// 追跡移動を止める
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool StopTrackingMoveAnimation();
        /// <summary>
        /// 向き変更を無効状態をセット
        /// </summary>
        /// <param name="isDisabled">無効状態か</param>
        /// <returns>成功／失敗</returns>
        public bool SetLookAtDisabled(bool isDisabled);
    }
}
