using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Area.View
{
    /// <summary>
    /// シーンカット
    /// タイムライン
    /// ビュー
    /// </summary>
    [RequireComponent(typeof(PlayableDirector))]
    public class CutSceneTimelineView : MonoBehaviour, ICutSceneTimelineView
    {
        /// <summary>プレイエイブル</summary>
        [SerializeField] private PlayableDirector playableDirector;

        public bool SetPlayableDirectorEnabled(bool isEnabled)
        {
            try
            {
                playableDirector.enabled = isEnabled;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        private void Reset()
        {
            playableDirector = GetComponent<PlayableDirector>();
        }
    }

    /// <summary>
    /// シーンカット
    /// タイムライン
    /// ビュー
    /// インターフェース
    public interface ICutSceneTimelineView
    {
        /// <summary>
        /// プレイエイブルを有効／無効にする
        /// </summary>
        /// <param name="isEnabled">有効／無効</param>
        /// <returns>成功／失敗</returns>
        public bool SetPlayableDirectorEnabled(bool isEnabled);
    }
}
