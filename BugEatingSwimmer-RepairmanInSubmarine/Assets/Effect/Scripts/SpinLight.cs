using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;

public class SpinLight : MonoBehaviour
{
    [SerializeField] int maxcnt;
    [SerializeField] float speed;
    [SerializeField] SpriteRenderer teban;
    float angle = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        angle += speed * Time.deltaTime;
        teban.transform.eulerAngles = new Vector3(0, 0, angle);
    }
}
