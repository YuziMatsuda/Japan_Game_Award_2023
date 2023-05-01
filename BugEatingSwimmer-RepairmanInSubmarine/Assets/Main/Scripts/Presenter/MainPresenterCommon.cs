using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Model;
using Main.Common;
using Main.View;
using UniRx;
using Main.Template;
using System.Linq;

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

        public bool SetCounterBetweenAndFillAmount(SeastarGageView seastarGageView, IntReactiveProperty seastarGageCount)
        {
            return SetCounterBetweenAndFillAmount(seastarGageView, seastarGageCount, -1);
        }

        public bool SetCounterBetweenAndFillAmount(SeastarGageView seastarGageView, IntReactiveProperty seastarGageCount, int addCountValue)
        {
            try
            {
                if (seastarGageView != null)
                {
                    if (addCountValue == 1)
                        seastarGageCount.Value += addCountValue;
                    else if (addCountValue == 0)
                        seastarGageCount.Value = addCountValue;
                    else
                    {
                        // 更新なし
                    }
                    if (!seastarGageView.SetCounterBetween(seastarGageCount.Value))
                        throw new System.Exception("カウンターをセット呼び出しの失敗");
                    float seastarGageCountValue = (float)seastarGageCount.Value;
                    if (!seastarGageView.SetFillAmount(seastarGageCountValue))
                        throw new System.Exception("フィルターゲージの値をセット呼び出しの失敗");
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool PlayCounterBetweenAndFillAmountAnimation(SeastarGageView seastarGageView, IntReactiveProperty seastarGageCount)
        {
            return PlayCounterBetweenAndFillAmountAnimation(seastarGageView, seastarGageCount, -1);
        }

        public bool PlayCounterBetweenAndFillAmountAnimation(SeastarGageView seastarGageView, IntReactiveProperty seastarGageCount, int addCountValue)
        {
            try
            {
                if (seastarGageView != null)
                {
                    if (addCountValue == 1)
                        seastarGageCount.Value += addCountValue;
                    else if (addCountValue == 0)
                        seastarGageCount.Value = addCountValue;
                    else
                    {
                        // 更新なし
                    }
                    if (!seastarGageView.PlayCounterBetweenAnimation(seastarGageCount.Value))
                        throw new System.Exception("カウンターをセット呼び出しの失敗");
                    float seastarGageCountValue = (float)seastarGageCount.Value;
                    if (!seastarGageView.PlayFillAmountAnimation(seastarGageCountValue))
                        throw new System.Exception("フィルターゲージの値をセット呼び出しの失敗");
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SaveDatasCSVOfAreaOpenedAndITStateAndOfMission()
        {
            try
            {
                // ステージのクリア状態をチェック
                var temp = new MainTemplateResourcesAccessory();
                var mainSceneStagesState = temp.GetMainSceneStagesState(temp.LoadSaveDatasCSV(ConstResorcesNames.MAIN_SCENE_STAGES_STATE));
                // 各エリアのステージがクリア済みならエリアのステータスを更新する
                var areaOpenedAndITState = temp.GetAreaOpenedAndITState(temp.LoadSaveDatasCSV(ConstResorcesNames.AREA_OPENED_AND_IT_STATE));
                var areaUnitsDefault = temp.GetAreaUnits(temp.LoadSaveDatasCSV(ConstResorcesNames.AREA_UNITS));
                foreach (var unitID in areaUnitsDefault.Select(q => q[EnumAreaUnits.UnitID])
                    .Distinct())
                {
                    bool allCleared = false;
                    foreach (var stageID in areaUnitsDefault.Where(q => q[EnumAreaUnits.UnitID] == unitID)
                        .Select(q => q[EnumAreaUnits.StageID]))
                        allCleared = mainSceneStagesState[stageID][EnumMainSceneStagesState.State] == 2;
                    if (allCleared)
                    {
                        if (0 < areaOpenedAndITState.Where(q => int.Parse(q[EnumAreaOpenedAndITState.UnitID]) == unitID &&
                            int.Parse(q[EnumAreaOpenedAndITState.State]) < (int)EnumAreaOpenedAndITStateState.Cleared)
                            .Select(q => q)
                            .ToArray().Length)
                            areaOpenedAndITState.Where(q => int.Parse(q[EnumAreaOpenedAndITState.UnitID]) == unitID &&
                                int.Parse(q[EnumAreaOpenedAndITState.State]) < (int)EnumAreaOpenedAndITStateState.Cleared)
                                .Select(q => q)
                                .ToArray()[0][EnumAreaOpenedAndITState.State] = $"{(int)EnumAreaOpenedAndITStateState.Cleared}";
                        // 最終エリアでないなら次のエリアを1にする
                        var nextUnitID = unitID + 1;
                        if (0 < areaOpenedAndITState.Where(q => int.Parse(q[EnumAreaOpenedAndITState.UnitID]) == nextUnitID &&
                            int.Parse(q[EnumAreaOpenedAndITState.State]) < (int)EnumAreaOpenedAndITStateState.Select)
                            .Select(q => q)
                            .ToArray().Length)
                            areaOpenedAndITState.Where(q => int.Parse(q[EnumAreaOpenedAndITState.UnitID]) == nextUnitID &&
                                int.Parse(q[EnumAreaOpenedAndITState.State]) < (int)EnumAreaOpenedAndITStateState.Select)
                                .Select(q => q)
                                .ToArray()[0][EnumAreaOpenedAndITState.State] = $"{(int)EnumAreaOpenedAndITStateState.Select}";
                    }
                }
                if (!temp.SaveDatasCSVOfAreaOpenedAndITState(ConstResorcesNames.AREA_OPENED_AND_IT_STATE, areaOpenedAndITState))
                    throw new System.Exception("エリア解放・結合テスト済みデータをCSVデータへ保存呼び出しの失敗");

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
        /// <summary>
        /// ヒトデゲージのカウンターとフィルターカウンターを更新
        /// </summary>
        /// <param name="seastarGageView">ヒトデゲージのビュー</param>
        /// <param name="seastarGageCount">ヒトデカウンター</param>
        /// <returns>成功／失敗</returns>
        public bool SetCounterBetweenAndFillAmount(SeastarGageView seastarGageView, IntReactiveProperty seastarGageCount);
        /// <summary>
        /// ヒトデゲージのカウンターとフィルターカウンターを更新
        /// </summary>
        /// <param name="seastarGageView">ヒトデゲージのビュー</param>
        /// <param name="seastarGageCount">ヒトデカウンター</param>
        /// <param name="addCountValue">カウンター値（1は加算、0はリセット、-1は更新なし）</param>
        /// <returns>成功／失敗</returns>
        public bool SetCounterBetweenAndFillAmount(SeastarGageView seastarGageView, IntReactiveProperty seastarGageCount, int addCountValue);
        /// <summary>
        /// ヒトデゲージのカウンターとフィルターカウンターを更新アニメーション
        /// </summary>
        /// <param name="seastarGageView">ヒトデゲージのビュー</param>
        /// <param name="seastarGageCount">ヒトデカウンター</param>
        /// <returns>成功／失敗</returns>
        public bool PlayCounterBetweenAndFillAmountAnimation(SeastarGageView seastarGageView, IntReactiveProperty seastarGageCount);
        /// <summary>
        /// ヒトデゲージのカウンターとフィルターカウンターを更新アニメーション
        /// </summary>
        /// <param name="seastarGageView">ヒトデゲージのビュー</param>
        /// <param name="seastarGageCount">ヒトデカウンター</param>
        /// <param name="addCountValue">カウンター値（1は加算、0はリセット、-1は更新なし）</param>
        /// <returns>成功／失敗</returns>
        public bool PlayCounterBetweenAndFillAmountAnimation(SeastarGageView seastarGageView, IntReactiveProperty seastarGageCount, int addCountValue);
        /// <summary>
        /// エリア解放と実績一覧を更新
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool SaveDatasCSVOfAreaOpenedAndITStateAndOfMission();
    }
}
