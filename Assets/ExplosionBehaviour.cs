using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBehaviour : MonoBehaviour
{
    [SerializeField] private float lifeTime = 0.6f;

    private float timer;
    private PooledObject pooled;

    private ParticleSystem ps;
    private Animator anim;

    private void Awake()
    {
        pooled = GetComponent<PooledObject>();
        ps = GetComponent<ParticleSystem>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        timer = 0f;

        if (ps != null)
        {
            ps.Clear(true);
            ps.Play(true);
        }

        if (anim != null)
        {
            anim.Rebind();
            anim.Update(0f);
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            if (pooled != null) pooled.ReturnToPool();
            else gameObject.SetActive(false);
        }
    }
}
