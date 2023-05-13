using Main.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// コシギンチャク
    /// ビュー
    /// </summary>
    public class LoinclothView : MonoBehaviour, IBodySpriteLoincloth
    {
        /// <summary>コシギンチャクスプライト</summary>
        [SerializeField] private BodySpriteLoincloth bodySpriteLoincloth;

        public bool PlaySwing()
        {
            return ((IBodySpriteLoincloth)bodySpriteLoincloth).PlaySwing();
        }

        private void Reset()
        {
            bodySpriteLoincloth = GetComponentInChildren<BodySpriteLoincloth>();
        }
    }
}
