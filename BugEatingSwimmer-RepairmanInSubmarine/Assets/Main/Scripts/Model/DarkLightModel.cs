using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.View;
using UniRx;
using System.Linq;

namespace Main.Model
{
    /// <summary>
    /// 暗闇
    /// モデル
    /// </summary>
    public class DarkLightModel : MonoBehaviour, IDarkLightModel
    {
        /// <summary>設定</summary>
        [SerializeField] private DarkLightConfig darkLightConfig;

        public IEnumerator SetStartTimer(System.IObserver<int> observer)
        {
            if (0f < darkLightConfig.PlayLightDownRate)
            {
                var count = -1;
                foreach (var item in darkLightConfig.Scales.Where(q => q != null))
                {
                    yield return new WaitForSeconds(darkLightConfig.PlayLightDownRate);
                    count++;
                    observer.OnNext(count);
                }
            }
            observer.OnNext(-1);
            yield return null;
        }

        private void Reset()
        {
            darkLightConfig = GetComponent<DarkLightConfig>();
        }
    }

    /// <summary>
    /// 暗闇
    /// モデル
    /// インターフェース
    /// </summary>
    public interface IDarkLightModel
    {
        /// <summary>
        /// タイマーセット
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <returns>コルーチン</returns>
        public IEnumerator SetStartTimer(System.IObserver<int> observer);
    }
}
