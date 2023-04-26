using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Select.View;
using Select.Model;
using System.Linq;

namespace Select.Common
{
    /// <summary>
    /// アルゴリズムのオーナー
    /// </summary>
    public class AlgorithmOwner : MonoBehaviour, IAlgorithmOwner
    {
        /// <summary>支点とコード配列</summary>
        private Transform[] _pivotAndCodeIShortUI;

        /// <summary>
        /// モジュールトレーサー
        /// </summary>
        [System.Serializable]
        private struct ModuleTracer
        {
            /// <summary>ノードコードの識別ID配列</summary>
            public EnumNodeCodeID[] enumNodeCodeIDs;
            /// <summary>方角モード配列</summary>
            public EnumDirectionMode[] enumDirectionModes;
        }
        /// <summary>
        /// モジュールトレーサー
        /// [0] ユニット１：ヘッドのIT
        /// [-] ユニット２：ボディは結合される側なので判定なし
        /// [1] ユニット３：ライトアームのIT
        /// [2] ユニット４：レフトアームのIT
        /// [-] ユニット５：コアは対象外
        /// [-] ユニット６：ヴォイドは対象外
        /// </summary>
        [SerializeField] private ModuleTracer[] moduleTracers;

        public int SetPivotAndCodeIShortUIs(Transform[] pivotAndCodeIShortUIs)
        {
            try
            {
                _pivotAndCodeIShortUI = pivotAndCodeIShortUIs;
                return _pivotAndCodeIShortUI.Length;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return -1;
            }
        }

        public int CheckIT()
        {
            try
            {
                if (_pivotAndCodeIShortUI.Length < 1)
                    return -1;

                var areaOpenedAndITState = SelectGameManager.Instance.SceneOwner.GetAreaOpenedAndITState();
                for (int i = 0; i < moduleTracers.Length; i++)
                {
                    switch (i)
                    {
                        case 0:
                            var quasiAssignmentForm = new SelectPresenterCommon().LoadSaveDatasCSVAndGetQuasiAssignmentForm();
                            if (FindNodeCodeIDAndEqualsDirection(moduleTracers[i], _pivotAndCodeIShortUI) &&
                                int.Parse(areaOpenedAndITState.Where(q => int.Parse(q[EnumAreaOpenedAndITState.UnitID]) == 1)
                                    .Select(q => q)
                                    .ToArray()[0][EnumAreaOpenedAndITState.State]) == (int)EnumAreaOpenedAndITStateState.Cleared &&
                                quasiAssignmentForm.Where(q => int.Parse(q[EnumQuasiAssignmentForm.MainSceneStagesModulesStateIndex]) == 1)
                                    .Select(q => q)
                                    .ToArray()
                                    .Length == quasiAssignmentForm.Where(q => int.Parse(q[EnumQuasiAssignmentForm.MainSceneStagesModulesStateIndex]) == 1 &&
                                        q[EnumQuasiAssignmentForm.Assigned].Equals(ConstGeneric.DIGITFORM_TRUE))
                                    .Select(q => q)
                                    .ToArray()
                                    .Length)
                            {
                                return 1;
                            }
                            break;
                        //case 1:
                        //    // T.B.D ノードコードIDと向きのチェック
                        //    return 3;
                        //case 2:
                        //    // T.B.D ノードコードIDと向きのチェック
                        //    return 4;
                        default:
                            // 未到達
                            break;
                    }
                }

                // 見つからない＆既に更新済み
                return 0;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return -1;
            }
        }

        /// <summary>
        /// コードIDとコード向きが全て一致しているか
        /// </summary>
        /// <param name="moduleTracer">モジュールトレーサー</param>
        /// <param name="pivotAndCodeIShortUIs">支点とコード配列</param>
        /// <returns></returns>
        private bool FindNodeCodeIDAndEqualsDirection(ModuleTracer moduleTracer, Transform[] pivotAndCodeIShortUIs)
        {
            var isFindAndEquals = new bool[moduleTracer.enumNodeCodeIDs.Length];
            for (int i = 0; i < moduleTracer.enumNodeCodeIDs.Length; i++)
            {
                isFindAndEquals[i] = 0 < pivotAndCodeIShortUIs.Where(q => q.GetComponent<PivotConfig>() != null &&
                    q.GetComponent<PivotConfig>().EnumNodeCodeID.Equals(moduleTracer.enumNodeCodeIDs[i]) &&
                    q.GetComponent<PivotAndCodeIShortUIView>() != null &&
                    ((EnumDirectionMode)q.GetComponent<PivotAndCodeIShortUIView>().EnumDirectionMode.Value).Equals(moduleTracer.enumDirectionModes[i]))
                    .Select(q => q)
                    .ToArray().Length;
            }

            return isFindAndEquals.Where(q => q)
                .Select(q => q)
                .ToArray().Length == isFindAndEquals.Length;
        }
    }

    /// <summary>
    /// アルゴリズムのオーナー
    /// インターフェース
    /// </summary>
    public interface IAlgorithmOwner
    {
        /// <summary>
        /// 支点とコード配列をセット
        /// </summary>
        /// <param name="pivotAndCodeIShortUIs">支点とコード配列</param>
        /// <returns>格納数（0以下はエラー）</returns>
        public int SetPivotAndCodeIShortUIs(Transform[] pivotAndCodeIShortUIs);
        /// <summary>
        /// IT実施
        /// モジュールトレーサーから対象ノードコードの組み合わせをチェック
        /// 対象ノードコードの向きをチェック
        /// エリアのステータスがクリア状態なら更新対象エリアIDを返す
        /// </summary>
        /// <returns>更新対象エリアID（0より小さい値はエラー）</returns>
        public int CheckIT();
    }
}
