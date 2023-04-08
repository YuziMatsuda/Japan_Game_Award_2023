using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Area.View 
{
    [RequireComponent(typeof(Image))]
    ///<summary>
    ///各ユニットの制御
    /// </summary>
    public class Robot_UnitImage : MonoBehaviour
    {
        [SerializeField] private Vector3 pos;
        [SerializeField] private Vector3 rotate;

        public void SetPositionAndEulerAngle()
        {
            Transform myTransform = this.transform;
            myTransform.position = pos;
            myTransform.eulerAngles = rotate;
        }

        public void SetImageAltha(bool visibled)
        {
            var color = GetComponent<Image>().color;
            var colorwork = new Color(color.r, color.g, color.b, visibled ? 1.0f : 0.0f);
            GetComponent<Image>().color = colorwork;
        }
    }
}
