using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float LifeTimeDuration = 15f;
    protected float LifeTime;
    public float Speed = 5f;
    private Vector2 m_Direction = new Vector2(0,1f);
    private float Dmg = 6;
    public virtual void ActiveDan()
    {
        gameObject.SetActive(true);
        LifeTime = LifeTimeDuration;
    }
    void Update()
    {
        float distance = Speed * Time.deltaTime;
        transform.Translate(m_Direction * distance);

        if (LifeTime > 0)
        {
            LifeTime -= Time.deltaTime;
        }
        else
        {
            DeactiveProj();
        }
    }

    private void DeactiveProj()
    {
        ObjectPoolManager.Unspawn(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var unit = collision.GetComponent<BotController>();
        if(unit != null)
        {
            unit.TakeDamge(Dmg);
        }
        DeactiveProj();
    }
}
