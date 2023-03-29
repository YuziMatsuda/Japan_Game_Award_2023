using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Model;
using Main.Common;
using Main.View;

namespace Main.Presenter
{
    /// <summary>
    /// プレゼンタの共通処理
    /// </summary>
    public class MainPresenterCommon : IMainPresenterCommon
    {
        public bool SetDisableAllNodeCode(Transform[] transforms, bool onTriggerEnter2DDisabled)
        {
            try
            {
                if (transforms != null)
                {
                    foreach (var item in transforms)
                    {
                        if (item.GetComponent<StartNodeModel>() != null)
                        {
                            if (!item.GetComponent<StartNodeModel>().SetOnTriggerEnter2DDisabled(onTriggerEnter2DDisabled))
                                throw new System.Exception("OnTriggerEnter判定をセット呼び出しの失敗");
                        }
                        if (item.GetComponent<PivotModel>() != null)
                        {
                            if (!item.GetComponent<PivotModel>().SetOnTriggerEnter2DDisabled(onTriggerEnter2DDisabled))
                                throw new System.Exception("OnTriggerEnter判定をセット呼び出しの失敗");
                        }
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

        public bool IsInductiveMethod(GameObject transform)
        {
            return transform != null &&
                transform.GetComponent<PivotConfig>() != null &&
                transform.GetComponent<PivotConfig>().EnumAtomicMode.Equals(EnumAtomicMode.Molecules) &&
                transform.GetComponent<GoalNodeModel>() != null &&
                !transform.GetComponent<GoalNodeModel>().IsGetting.Value;
        }

        public bool ResetAllPostingState(Transform[] transforms)
        {
            try
            {
                if (transforms != null)
                {
                    foreach (var item in transforms)
                    {
                        if (item.GetComponent<StartNodeView>() != null)
                        {
                            if (!item.GetComponent<StartNodeModel>().SetIsPosting(false))
                                throw new System.Exception("信号発生アニメーション実行中フラグをセット呼び出しの失敗");
                            if (!item.GetComponent<StartNodeModel>().SetToListLength(-1))
                                throw new System.Exception("POST先のノードコードリスト数をセット呼び出しの失敗");
                        }
                        if (item.GetComponent<PivotView>() != null)
                        {
                            if (!item.GetComponent<PivotModel>().SetIsPosting(false))
                                throw new System.Exception("信号発生アニメーション実行中フラグをセット呼び出しの失敗");
                            if (!item.GetComponent<PivotModel>().SetToListLength(-1))
                                throw new System.Exception("POST先のノードコードリスト数をセット呼び出しの失敗");
                        }
                        if (item.GetComponent<GoalNodeView>() != null)
                        {
                            if (!item.GetComponent<GoalNodeModel>().SetIsPosting(false))
                                throw new System.Exception("信号発生アニメーション実行中フラグをセット呼び出しの失敗");
                        }
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
    }

    /// <summary>
    /// プレゼンタの共通処理
    /// インターフェース
    /// </summary>
    public interface IMainPresenterCommon
    {
        /// <summary>
        /// 帰納法かチェック
        /// </summary>
        /// <param name="transform">オブジェクト</param>
        /// <returns>帰納法モードが有効</returns>
        public bool IsInductiveMethod(GameObject transform);

        /// <summary>
        /// POSTのリセット
        /// スタートノード、コード、ゴールノードが対象
        /// </summary>
        /// <param name="transforms">スタートノード、コード、ゴールノードのトランスフォーム配列</param>
        /// <returns>成功／失敗</returns>
        public bool ResetAllPostingState(Transform[] transforms);

        /// <summary>
        /// ノードコードの衝突判定を無効にする
        /// </summary>
        /// <param name="transforms">スタートノード、コード、ゴールノードのトランスフォーム配列</param>
        /// <param name="onTriggerEnter2DDisabled">無効とするか</param>
        /// <returns>成功／失敗</returns>
        public bool SetDisableAllNodeCode(Transform[] transforms, bool onTriggerEnter2DDisabled);
    }
}
