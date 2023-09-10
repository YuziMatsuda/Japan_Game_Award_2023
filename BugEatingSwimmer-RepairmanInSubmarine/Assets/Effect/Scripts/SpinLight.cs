using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using UniRx;

public class SpinLight : MonoBehaviour
{
    [SerializeField] private float speed = 500f;
    [SerializeField] private SpriteRenderer teban;
    private float _angle = 0f;
    [SerializeField] private ParticleSystem parentParticle;

    private void Reset()
    {
        teban = GetComponent<SpriteRenderer>();
        parentParticle = transform.parent.GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        parentParticle.ObserveEveryValueChanged(x => x.isPlaying)
            .Subscribe(x => teban.enabled = x);
    }

    private void Update()
    {
        _angle += speed * Time.deltaTime;
        teban.transform.eulerAngles = new Vector3(0, 0, _angle);
    }
}
