using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Common;

namespace Main.Model
{
    /// <summary>
    /// 始点
    /// 静的に設定する用
    /// </summary>
    public class PivotConfig : MonoBehaviour
    {
        /// <summary>方角モードのデフォルト値</summary>
        [SerializeField] private EnumDirectionMode enumDirectionModeDefault = EnumDirectionMode.Up;
        /// <summary>方角モードのデフォルト値</summary>
        public EnumDirectionMode EnumDirectionModeDefault => enumDirectionModeDefault;
        /// <summary>アトミックモード</summary>
        [SerializeField] private EnumAtomicMode enumAtomicMode = EnumAtomicMode.Atoms;
        /// <summary>アトミックモード</summary>
        public EnumAtomicMode EnumAtomicMode => enumAtomicMode;
        /// <summary>ノードコードの識別ID</summary>
        [SerializeField] private EnumNodeCodeID enumNodeCodeId = EnumNodeCodeID.StartNode_1;
        /// <summary>ノードコードの識別ID</summary>
        public EnumNodeCodeID EnumNodeCodeID => enumNodeCodeId;
        /// <summary>エラー方角モードの配列</summary>
        [SerializeField] private EnumDirectionMode[] errorDirections;
        /// <summary>エラー方角モードの配列</summary>
        public EnumDirectionMode[] ErrorDirections => errorDirections;
        /// <summary>サン（ゴ）ショウコードであるか</summary>
        [SerializeField] private bool readonlyCodeMode;
        /// <summary>サン（ゴ）ショウコードであるか</summary>
        public bool ReadonlyCodeMode => readonlyCodeMode;
        /// <summary>
        /// サン（ゴ）ショウコードであるかをセット
        /// </summary>
        /// <param name="readonlyCodeMode">サン（ゴ）ショウコードであるか</param>
        /// <returns>成功／失敗</returns>
        public bool SetReadonlyCodeMode(bool readonlyCodeMode)
        {
            try
            {
                this.readonlyCodeMode = readonlyCodeMode;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }
        /// <summary>サン（ゴ）ショウコード破片</summary>
        [SerializeField] private Transform[] coralParts;
        /// <summary>サン（ゴ）ショウコード破片</summary>
        public Transform[] CoralParts => coralParts;
        /// <summary>感情コードであるか</summary>
        [SerializeField] private bool emotionsCodeMode;
        /// <summary>感情コードであるか</summary>
        public bool EmotionsCodeMode => emotionsCodeMode;
        /// <summary>インタラクトID</summary>
        [SerializeField] private EnumInteractID enumInteractID;
        /// <summary>インタラクトID</summary>
        public EnumInteractID EnumInteractID => enumInteractID;
        /// <summary>
        /// インタラクトIDをセット
        /// </summary>
        /// <param name="enumInteractID">インタラクトID</param>
        /// <returns>成功／失敗</returns>
        public bool SetEnumInteractID(EnumInteractID enumInteractID)
        {
            try
            {
                this.enumInteractID = enumInteractID;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }
    }
}
