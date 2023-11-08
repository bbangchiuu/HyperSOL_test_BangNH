using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotController : MonoBehaviour
{
    public bool[] finishPaths { get; private set; }
    private WaveController waveController;
    public Path MovingPath;
    private float MovingSpeed = 2.5f;
    public bool isRotation = false;
    private float RotationSpeed = 15;
    public float RotationOffset;
    public int IndexPath = 0;
    int CurrentPointIndex = 0;
    float MinDistanceLimit = 0.1f;

    public int IndexEnemy;

    public float MaxHP = 10;
    private float Hp;
    public void ActiveObj(Path path, int indexEnemy, WaveController waveController)
    {
        this.CurrentPointIndex = 0;
        this.MovingPath = path;
        this.IndexEnemy = indexEnemy;
        this.IndexPath = 0;
        this.waveController = waveController;
        this.finishPaths = new bool[10];
        this.Hp = MaxHP;
    }

    void Update()
    {
        if (finishPaths[IndexPath]) return;

        switch (MovingPath.state)
        {
            case StateMove.line:
                MoveLine();
                break;
            case StateMove.point:
                MovePoint();
                break;
            case StateMove.loopdownup:
                MoveLoopDownUp();
                break;
        }
    }

    float timeInterval = 1;
    bool moveUp = false;
    void MoveLoopDownUp()
    {
        timeInterval -= Time.deltaTime;
        if(timeInterval <= 0)
        {
            timeInterval = 1;
            moveUp = !moveUp;
        }

        if(moveUp == false)
        {
            transform.Translate(new Vector2(0, -Time.deltaTime * MovingSpeed));
        }
        else
        {
            transform.Translate(new Vector2(0, Time.deltaTime * MovingSpeed));
        }
    }
    void MovePoint()
    {
        var indexPoint = IndexEnemy >= MovingPath.ListPoints.Count ? MovingPath.ListPoints.Count -1 : IndexEnemy;

        float Distance = Vector3.Distance(new Vector3(MovingPath.ListPoints[indexPoint].position.x, MovingPath.ListPoints[indexPoint].position.y, 0.0f), new Vector3(this.transform.position.x, this.transform.position.y, 0.0f));
        this.transform.position += (MovingPath.ListPoints[indexPoint].position - this.transform.position).normalized * MovingSpeed * Time.deltaTime;
        //Rotating
        if (isRotation)
        {
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, GetRotationZ()));
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
        }
        if (Distance <= MinDistanceLimit)
        {
            finishPaths[IndexPath] = true;
            waveController.ChangePath(IndexEnemy, IndexPath);
        }
    }
    void MoveLine()
    {
        //Moving
        float Distance = Vector3.Distance(new Vector3(MovingPath.ListPoints[CurrentPointIndex].position.x, MovingPath.ListPoints[CurrentPointIndex].position.y, 0.0f), new Vector3(this.transform.position.x, this.transform.position.y, 0.0f));
        this.transform.position += (MovingPath.ListPoints[CurrentPointIndex].position - this.transform.position).normalized * MovingSpeed * Time.deltaTime;
        //Rotating
        if (isRotation)
        {
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, GetRotationZ()));
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
        }
        //Checking End Path
        if (Distance <= MinDistanceLimit)
        {
            CurrentPointIndex++;
            if (CurrentPointIndex >= MovingPath.ListPoints.Count)
            {
                CurrentPointIndex = 0;
                finishPaths[IndexPath] = true;
                waveController.ChangePath(IndexEnemy, IndexPath);
            }
        }
    }
    float GetRotationZ()
    {
        Vector3 Direction = (MovingPath.ListPoints[CurrentPointIndex].position - this.transform.position).normalized;
        float result = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
        return result + RotationOffset;
    }

    public void TakeDamge(float dmg)
    {
        Hp -= dmg;
        if(Hp <= 0)
        {
            waveController.ChangePath(IndexEnemy, IndexPath);
            ObjectPoolManager.Unspawn(gameObject);
        }
    }

    public bool IsAlive()
    {
        return Hp > 0 && gameObject.activeInHierarchy;
    }
}
