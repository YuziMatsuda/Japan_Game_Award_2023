using Select.Template;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Select.Model;
using System.Linq;
using Select.View;

namespace Select.Common
{
    /// <summary>
    /// プレゼンタ
    /// セレクトシーン
    /// </summary>
    public class SelectPresenterCommon : ISelectPresenterCommon
    {
        public int GetContentsCountInPage()
        {
            try
            {
                var temp = new SelectTemplateResourcesAccessory();
                // 現在選択されているステージ番号を取得
                var systemCommonCash = SelectGameManager.Instance.SceneOwner.GetSystemCommonCash();
                var areaUnits = temp.GetAreaUnits(temp.LoadSaveDatasCSV(ConstResorcesNames.AREA_UNITS));
                // ステージIDを元にエリアIDを取得
                return areaUnits.Where(q => q[EnumAreaUnits.StageID] == systemCommonCash[EnumSystemCommonCash.SceneId])
                    .Select(q => q[EnumAreaUnits.UnitID])
                    .Distinct()
                    .ToArray()[0];
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return -1;
            }
        }

        public Dictionary<EnumAreaOpenedAndITState, string>[] LoadSaveDatasCSVAndGetAreaOpenedAndITState()
        {
            var tResourcesAccessory = new SelectTemplateResourcesAccessory();
            var quasiAssignFormResources = tResourcesAccessory.LoadSaveDatasCSV(ConstResorcesNames.AREA_OPENED_AND_IT_STATE);
            return tResourcesAccessory.GetAreaOpenedAndITState(quasiAssignFormResources);
        }

        public Dictionary<EnumAreaUnits, int>[] LoadSaveDatasCSVAndGetAreaUnits()
        {
            var tResourcesAccessory = new SelectTemplateResourcesAccessory();
            var quasiAssignFormResources = tResourcesAccessory.LoadSaveDatasCSV(ConstResorcesNames.AREA_UNITS);
            return tResourcesAccessory.GetAreaUnits(quasiAssignFormResources);
        }

        public Dictionary<EnumMainSceneStagesState, int>[] LoadSaveDatasCSVAndGetMainSceneStagesState()
        {
            var tResourcesAccessory = new SelectTemplateResourcesAccessory();
            var quasiAssignFormResources = tResourcesAccessory.LoadSaveDatasCSV(ConstResorcesNames.MAIN_SCENE_STAGES_STATE);
            return tResourcesAccessory.GetMainSceneStagesState(quasiAssignFormResources);
        }

        public Dictionary<EnumQuasiAssignmentForm, string>[] LoadSaveDatasCSVAndGetQuasiAssignmentForm()
        {
            var tResourcesAccessory = new SelectTemplateResourcesAccessory();
            var quasiAssignFormResources = tResourcesAccessory.LoadSaveDatasCSV(ConstResorcesNames.QUASI_ASSIGNMENT_FORM);
            return tResourcesAccessory.GetQuasiAssignmentForm(quasiAssignFormResources);
        }

        public bool SetCounterBetweenAndFillAmountAllGage(SeastarGageView[] seastarGageViews)
        {
            try
            {
                foreach (var item in seastarGageViews.Where(q => q != null))
                {
                    var counter = item.SeastarGageConfig.EnumUnitID.Equals(EnumUnitID.Head) ?
                        SelectGameManager.Instance.GimmickOwner.GetAssinedCounter((int)EnumUnitID.Head) :
                        SelectGameManager.Instance.GimmickOwner.GetAssinedCounter();
                    if (!item.PlayCounterBetweenAnimation(counter))
                        throw new System.Exception("カウンターをセット呼び出しの失敗");
                    float seastarGageCountValue = (float)counter;
                    if (!item.PlayFillAmountAnimation(seastarGageCountValue))
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
    public interface ISelectPresenterCommon
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
        /// <summary>
        /// 全ヒトデゲージのカウンターとフィルターをセット
        /// </summary>
        /// <param name="seastarGageViews">ヒトデゲージのビュー配列</param>
        /// <returns>成功／失敗</returns>
        public bool SetCounterBetweenAndFillAmountAllGage(SeastarGageView[] seastarGageViews);
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
        /// <summary>
        /// ステージIDに基づいたページ番号を取得する
        /// </summary>
        /// <returns>ページ番号</returns>
        public int GetContentsCountInPage();
    }
}
