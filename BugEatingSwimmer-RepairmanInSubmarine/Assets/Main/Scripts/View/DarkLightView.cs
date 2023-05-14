using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// 暗闇
    /// ビュー
    /// </summary>
    public class DarkLightView : MonoBehaviour
    {
        /// <summary>暗闇のスプライト</summary>
        [SerializeField] private BodySpriteDarkLight bodySpriteDarkLight;

        private void Reset()
        {
            bodySpriteDarkLight = GetComponentInChildren<BodySpriteDarkLight>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
