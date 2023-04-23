using Area.Template;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Select.Model;
using System.Linq;
using Area.View;

namespace Area.Common
{
    /// <summary>
    /// プレゼンタ
    /// セレクトシーン
    /// </summary>
    public class AreaPresenterCommon : IAreaPresenterCommon
    {
        public Dictionary<EnumAreaOpenedAndITState, string>[] LoadSaveDatasCSVAndGetAreaOpenedAndITState()
        {
            var tResourcesAccessory = new AreaTemplateResourcesAccessory();
            var quasiAssignFormResources = tResourcesAccessory.LoadSaveDatasCSV(ConstResorcesNames.AREA_OPENED_AND_IT_STATE);
            return tResourcesAccessory.GetAreaOpenedAndITState(quasiAssignFormResources);
        }

        public Dictionary<EnumAreaUnits, int>[] LoadSaveDatasCSVAndGetAreaUnits()
        {
            var tResourcesAccessory = new AreaTemplateResourcesAccessory();
            var quasiAssignFormResources = tResourcesAccessory.LoadSaveDatasCSV(ConstResorcesNames.AREA_UNITS);
            return tResourcesAccessory.GetAreaUnits(quasiAssignFormResources);
        }

        public Dictionary<EnumMainSceneStagesState, int>[] LoadSaveDatasCSVAndGetMainSceneStagesState()
        {
            var tResourcesAccessory = new AreaTemplateResourcesAccessory();
            var quasiAssignFormResources = tResourcesAccessory.LoadSaveDatasCSV(ConstResorcesNames.MAIN_SCENE_STAGES_STATE);
            return tResourcesAccessory.GetMainSceneStagesState(quasiAssignFormResources);
        }

        public Dictionary<EnumQuasiAssignmentForm, string>[] LoadSaveDatasCSVAndGetQuasiAssignmentForm()
        {
            var tResourcesAccessory = new AreaTemplateResourcesAccessory();
            var quasiAssignFormResources = tResourcesAccessory.LoadSaveDatasCSV(ConstResorcesNames.QUASI_ASSIGNMENT_FORM);
            return tResourcesAccessory.GetQuasiAssignmentForm(quasiAssignFormResources);
        }

        //public bool SetCounterBetweenAndFillAmountAllGage(SeastarGageView[] seastarGageViews)
        //{
        //    try
        //    {
        //        foreach (var item in seastarGageViews.Where(q => q != null))
        //        {
        //            if (!item.PlayCounterBetweenAnimation(SelectGameManager.Instance.GimmickOwner.GetAssinedCounter()))
        //                throw new System.Exception("カウンターをセット呼び出しの失敗");
        //            float seastarGageCountValue = (float)SelectGameManager.Instance.GimmickOwner.GetAssinedCounter();
        //            if (!item.PlayFillAmountAnimation(seastarGageCountValue))
        //                throw new System.Exception("フィルターゲージの値をセット呼び出しの失敗");
        //        }

        //        return true;
        //    }
        //    catch (System.Exception e)
        //    {
        //        Debug.LogError(e);
        //        return false;
        //    }
        //}

        public bool SetIsAssignedAllCaption(CaptionStageModel[] captionStageModels)
        {
            try
            {
                foreach (var item in LoadSaveDatasCSVAndGetMainSceneStagesState()
                    .Select((p, i) => new { Content = p, Index = i })
                    .Where(q => 0 < q.Index))
                {
                    // ステージ番号を取り出す
                    var sceneId = item.Index;
                    foreach (var itemChilds in captionStageModels.Where(q => q != null &&
                            q.Index == sceneId &&
                            q.SeastarModels != null &&
                            0 < q.SeastarModels.Length)
                        .Select(q => q.SeastarModels))
                    {
                        // ステージ番号ごとにSeastar情報の配列を取得
                        foreach (var itemChild in itemChilds)
                        {
                            // ステージ番号ごとのキャプション配列の中でアサイン情報を反映
                            itemChild.SetIsAssigned(LoadSaveDatasCSVAndGetQuasiAssignmentForm()
                                .Where(q => q[EnumQuasiAssignmentForm.MainSceneStagesModulesStateIndex].Equals($"{sceneId}") &&
                                    q[EnumQuasiAssignmentForm.SeastarID].Equals($"{itemChild.SeastarConfig.EnumSeastarID}"))
                                .Select(q => q)
                                .ToArray()[0][EnumQuasiAssignmentForm.Assigned].Equals(ConstGeneric.DIGITFORM_TRUE));
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
    /// プレゼンタ
    /// セレクトシーン
    /// インターフェース
    /// </summary>
    public interface IAreaPresenterCommon
    {
        /// <summary>
        /// 準委任帳票の取得
        /// </summary>
        /// <returns>格納オブジェクト配列</returns>
        public Dictionary<EnumQuasiAssignmentForm, string>[] LoadSaveDatasCSVAndGetQuasiAssignmentForm();
        /// <summary>
        /// ステージクリア済みデータをオブジェクトへ一時セット
        /// </summary>
        /// <returns>格納オブジェクト配列</returns>
        public Dictionary<EnumMainSceneStagesState, int>[] LoadSaveDatasCSVAndGetMainSceneStagesState();
        /// <summary>
        /// 全ステージキャプションのアサイン情報をセット
        /// </summary>
        /// <param name="captionStageModels">ステージキャプション配列</param>
        /// <returns>成功／失敗</returns>
        public bool SetIsAssignedAllCaption(CaptionStageModel[] captionStageModels);
        ///// <summary>
        ///// 全ヒトデゲージのカウンターとフィルターをセット
        ///// </summary>
        ///// <param name="seastarGageViews">ヒトデゲージのビュー配列</param>
        ///// <returns>成功／失敗</returns>
        //public bool SetCounterBetweenAndFillAmountAllGage(SeastarGageView[] seastarGageViews);
        /// <summary>
        /// エリアユニットファイルへ一時セット
        /// </summary>
        /// <returns>格納オブジェクト配列</returns>
        public Dictionary<EnumAreaUnits, int>[] LoadSaveDatasCSVAndGetAreaUnits();
        /// <summary>
        /// エリア解放・結合テスト済みデータをオブジェクトへ一時セット
        /// </summary>
        /// <returns>格納オブジェクト配列</returns>
        public Dictionary<EnumAreaOpenedAndITState, string>[] LoadSaveDatasCSVAndGetAreaOpenedAndITState();
    }
}
