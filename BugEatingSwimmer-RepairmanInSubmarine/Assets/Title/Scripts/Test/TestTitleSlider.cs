using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Title.Test
{
    /// <summary>
    /// スライダーのテスト
    /// </summary>
    public class TestTitleSlider : MonoBehaviour
    {
        [SerializeField] private Slider slider;

        // Start is called before the first frame update
        void Start()
        {
            slider.value = 1f;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
