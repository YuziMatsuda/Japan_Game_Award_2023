using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Common;
using DG.Tweening;
using Main.Model;
using System.Linq;

namespace Main.View
{
    /// <summary>
    /// コードをつつく場所を示すカーソル
    /// ビュー
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class AttackCodeCursorView : MonoBehaviour, IAttackCodeCursorView
    {
        /// <summary>終了時間</summary>
        [SerializeField] private float duration = .3f;
        /// <summary>キャンバスグループ</summary>
        [SerializeField] private CanvasGroup canvasGroup;
        /// <summary>カーソルアイコンモデル</summary>
        [SerializeField] private CursorIconModel[] cursorIconModels;
        /// <summary>位置</summary>
        [SerializeField] private Vector3[] positions;

        private void Reset()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            cursorIconModels = GetComponentsInChildren<CursorIconModel>();
        }

        public bool SetAlpha(EnumFadeState state)
        {
            try
            {
                canvasGroup.alpha = state.Equals(EnumFadeState.Open) ? 1f : 0f;
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public IEnumerator PlayFadeAnimation(System.IObserver<bool> observer, EnumFadeState state)
        {
            if ((state.Equals(EnumFadeState.Open) &&
                canvasGroup.alpha != 1f) ||
                (state.Equals(EnumFadeState.Close) &&
                canvasGroup.alpha != 0f))
            {
                canvasGroup.DOFade(endValue: state.Equals(EnumFadeState.Open) ? 1f : 0f, duration)
                    .SetUpdate(true)
                    .OnComplete(() => observer.OnNext(true));
            }
            else
                observer.OnNext(true);

            yield return null;
        }

        public bool SetSelectToChild(int index, EnumInteractIDSub enumInteractIDSub)
        {
            if (enumInteractIDSub.Equals(EnumInteractIDSub.None))
                return false;

            return cursorIconModels[0].transform.GetComponent<CursorIconView>()
                .SetSelect(positions[(int)enumInteractIDSub]);
        }

        public bool PlayRoundMoveAnimation(int index, EnumInteractIDSub enumInteractIDSub)
        {
            if (enumInteractIDSub.Equals(EnumInteractIDSub.None))
                return false;

            return cursorIconModels.Where(q => q.RuleShellfishConfig.InstanceOfReadedScenarioTutorialProperty.guideUIIdxes[(int)enumInteractIDSub] == index)
                    .Select(q => q.GetComponent<CursorIconView>())
                    .Distinct()
                    .ToArray()[0].PlayRoundMoveAnimation();
        }

        public bool RewindRoundMoveAnimation(int index, EnumInteractIDSub enumInteractIDSub)
        {
            if (enumInteractIDSub.Equals(EnumInteractIDSub.None))
                return false;

            return cursorIconModels[0].GetComponent<CursorIconView>()
                .RewindRoundMoveAnimation();
        }

        public bool RePlayRoundMoveAnimation(int index, EnumInteractIDSub enumInteractIDSub)
        {
            if (enumInteractIDSub.Equals(EnumInteractIDSub.None))
                return false;

            return cursorIconModels.Where(q => q.RuleShellfishConfig.InstanceOfReadedScenarioTutorialProperty.guideUIIdxes[(int)enumInteractIDSub] == index)
                    .Select(q => q.GetComponent<CursorIconView>())
                    .Distinct()
                    .ToArray()[0].RePlayRoundMoveAnimation();
        }
    }

    /// <summary>
    /// コードをつつく場所を示すカーソル
    /// ビュー
    /// インターフェース
    /// </summary>
    public interface IAttackCodeCursorView
    {
        /// <summary>
        /// アルファ値をセット
        /// </summary>
        /// <param name="state">ステータス</param>
        /// <returns>成功／失敗</returns>
        public bool SetAlpha(EnumFadeState state);
        /// <summary>
        /// フェードのDOTweenアニメーション再生
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <param name="state">ステータス</param>
        /// <returns>成功／失敗</returns>
        public IEnumerator PlayFadeAnimation(System.IObserver<bool> observer, EnumFadeState state);
        /// <summary>
        /// カーソル配置位置の変更
        /// </summary>
        /// <param name="position">移動先のポジション</param>
        /// <param name="index">インデックス</param>
        /// <param name="enumInteractIDSub">サブID</param>
        /// <returns>成功／失敗</returns>
        public bool SetSelectToChild(int index, EnumInteractIDSub enumInteractIDSub);
        /// <summary>
        /// カーソル往復移動アニメーション
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="enumInteractIDSub">サブID</param>
        /// <returns>成功／失敗</returns>
        public bool PlayRoundMoveAnimation(int index, EnumInteractIDSub enumInteractIDSub);
        /// <summary>
        /// カーソル往復移動アニメーション・一時停止
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool RewindRoundMoveAnimation(int index, EnumInteractIDSub enumInteractIDSub);
        /// <summary>
        /// カーソル往復移動アニメーション・再生
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool RePlayRoundMoveAnimation(int index, EnumInteractIDSub enumInteractIDSub);
    }
}
