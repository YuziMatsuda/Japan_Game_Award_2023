using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Area.Common
{
    /// <summary>
    /// ロボットの結合状態
    /// </summary>
    public enum EnumRobotPanel
    {
        /// <summary>バラバラ</summary>
        FallingApart,
        /// <summary>ボディ起動</summary>
        OnStartBody,
        /// <summary>ヘッドの結合（エラー）</summary>
        ConnectedFailureHead,
        /// <summary>ヘッドの結合</summary>
        ConnectedHead,
        /// <summary>ライトアームの結合</summary>
        ConnectedRightarm,
        /// <summary>レフトアームの結合</summary>
        ConnectedLeftarm,
        /// <summary>両方のアームの結合</summary>
        ConnectedDoublearm,
        /// <summary>すべて結合</summary>
        Full,
    }
}
