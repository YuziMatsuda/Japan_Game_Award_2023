using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Main.Model;
using UniRx;

namespace Main.Common
{
    /// <summary>
    /// パワーシェルのリーダー
    /// </summary>
    public class PowerShellLeader : MonoBehaviour, IMainGameManager
    {
        public void OnStart()
        {
            var powerShellModels = GameObject.FindGameObjectsWithTag(ConstTagNames.TAG_NAME_POWERSHELL)
                .Where(q => q != null &&
                    q.GetComponent<PowerShellModel>() != null)
                .Select(q => q.GetComponent<PowerShellModel>())
                .ToArray();
            for (var i = 0; i < powerShellModels.Length; i++)
            {
                var idx = i;
                powerShellModels[idx].IsCollisioned.ObserveEveryValueChanged(x => x.Value)
                    .Subscribe(x =>
                    {
                        if (x)
                            if (!powerShellModels[idx].DestroyItem())
                                Debug.LogError("アイテムを削除呼び出しの失敗");
                    });
            }
        }
    }
}
