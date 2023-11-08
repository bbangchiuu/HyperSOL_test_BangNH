using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    Transform[] barrels;
    [SerializeField]
    GameObject danPrefab;

    public float speedMove = 5;
    public float atkSpeed = 1;
    private float mCoolDown = 0;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        HandleMove();
        HandleFire();
    }
    public void StartCoolDown()
    {
        mCoolDown = 1f / atkSpeed;
    }

    void HandleFire()
    {
        if(mCoolDown > 0)
        {
            mCoolDown -= Time.deltaTime;
        }
        else
        {
            Fire();
            StartCoolDown();
        }
    }

    void Fire()
    {
        for(int i = 0; i < barrels.Length; i++)
        {
            var trans = barrels[i];
            GameObject projObj = null;
            projObj = ObjectPoolManager.Spawn(danPrefab, trans.position, trans.rotation);

            BulletController bullet = projObj.GetComponent<BulletController>();
            bullet.ActiveDan();
        }
    }

    private Vector2 endPoint;   //Last touch position
    void HandleMove()
    {
#if UNITY_ANDROID || UNITY_IOS
        if (Input.GetMouseButton(0))
        {
            endPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = endPoint - (Vector2)transform.position;
            transform.Translate(direction * speedMove * Time.deltaTime);
        }
#else        
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 direction = new Vector2(horizontal, vertical);
        transform.Translate(direction * speedMove * Time.deltaTime);
#endif
    }
}
