using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurvePointManger : MonoBehaviour
{
    public void SetCurvePointPrefab(GameObject curvePointPrefab)
    {
        CurvePointPrefab = curvePointPrefab;
    }
    public void StartCurve(Vector3 startPos,  Vector3 endPos, int numPoints, float g, float intervalTime, float totalTime, float defautFlyTime)
    {
        if (numPoints <= 0) return;

        IsFinished = false;

        StartedPointNum = 0;
        FinishedPointNum = 0;
        TotalPointNum = numPoints;
        IntervalTime = intervalTime;
        TotalTime = totalTime;
        FlyTime = TotalTime - (TotalPointNum - 1) * IntervalTime;
        FlyTime = FlyTime > 0.0f ? FlyTime : defautFlyTime;
        Runtime = 0.0f;
        G = g;
        StartPos = startPos;

        Speed = new Vector3((endPos.x - startPos.x) / FlyTime,
                            (endPos.y - startPos.y) / FlyTime - 0.5f * G * FlyTime,
                            (endPos.z - startPos.z) / FlyTime);

        RequestFreeCurvePoints(numPoints);
    }

    public bool IsFinish()
    {
        return IsFinished;
    }

    void Update()
    {
        if(!IsFinished)
        {
            CheckFinish();

            UpdateStartedPoints(Time.deltaTime);

            StartNextPointIfNecessary();

            UpdateTimes(Time.deltaTime);
        }
    }

    private void CheckFinish()
    {
        if(FinishedPointNum >= TotalPointNum)
        {
            IsFinished = true;
        }
    }

    private float GetDropHeight(float g, float t1, float t2)
    {
        return 0.5f * g * (t2 * t2 - t1 * t1);
    }

    private void UpdatePointPos(int pointIndex, float delta)
    {
        if (pointIndex >= ActivedCurvePoints.Count) return;
        
        ActivedCurvePoints[pointIndex].transform.Translate(Speed * delta);

        Vector3 Drop = new Vector3(0.0f, GetDropHeight(G, ActivedCurvePointTimer[pointIndex], ActivedCurvePointTimer[pointIndex] + delta), 0.0f);
        ActivedCurvePoints[pointIndex].transform.Translate(Drop);
    }

    private void UpdateStartedPoints(float delta)
    {
        for (int i = FinishedPointNum; i < StartedPointNum; i++)
        {
            float fixedDelta = 0.0f;

            if (ActivedCurvePointTimer[i] + delta < FlyTime)
            {
                fixedDelta = delta;
            }
            else
            {
                fixedDelta = FlyTime - ActivedCurvePointTimer[i];
            }

            if(fixedDelta > 0.0f)
            {
                UpdatePointPos(i, fixedDelta);
            }
            else
            {
                ActivedCurvePoints[FinishedPointNum].SetActive(false);
                FinishedPointNum++;
            }         
        }
    }

    private void StartNextPointIfNecessary()
    {
        if (((StartedPointNum < TotalPointNum)&&(Runtime - StartedPointNum * IntervalTime >= 0)) || (StartedPointNum == 0))
        {
            StartedPointNum++;
            StartPoint(StartedPointNum - 1);
        }
    }

    private void UpdateTimes(float delta)
    {
        Runtime += delta;

        for(int i = FinishedPointNum; i < StartedPointNum; i++)
        {
            ActivedCurvePointTimer[i] += delta;
        }
    }

    private void StartPoint(int pointIndex)
    {
        if (pointIndex >= ActivedCurvePoints.Count) return;
        
        ActivedCurvePoints[pointIndex].transform.position = StartPos;
        ActivedCurvePoints[pointIndex].SetActive(true);  
    }

    private void NextFreeCurvePoint()
    {
        GameObject curvePoint = null;

        if (FreeCurvePoints.Count > 0)
        {
            curvePoint = FreeCurvePoints[0];
            FreeCurvePoints.RemoveAt(0);
        }
        else
        {
            curvePoint = Instantiate(CurvePointPrefab);
        }

        if (curvePoint)
        {
            curvePoint.SetActive(false);
            ActivedCurvePoints.Add(curvePoint);
            ActivedCurvePointTimer.Add(0.0f);
        };
    }

    private void FreeAllCurvePoints()
    {
        ActivedCurvePointTimer.Clear();

        foreach (var point in ActivedCurvePoints)
        {
            point.SetActive(false);
            FreeCurvePoints.Add(point);
        }
        ActivedCurvePoints.Clear();
    }

    private void RequestFreeCurvePoints(int NumPoints)
    {
        FreeAllCurvePoints();

        for (int i=0; i<NumPoints ;i++)
        {
            NextFreeCurvePoint();           
        }
    }


    private bool IsFinished = false;

    private int StartedPointNum = 0;
    private int FinishedPointNum = 0;
    private int TotalPointNum = 0;
    private float IntervalTime = 0.0f;
    private float FlyTime = 0.0f;
    private float TotalTime = 0.0f;
    private float Runtime = 0.0f;
    private float G = -9.8f;
    private Vector3 StartPos;
    private Vector3 Speed;

    private GameObject CurvePointPrefab;

    private List<float> ActivedCurvePointTimer = new List<float>();
    private List<GameObject> ActivedCurvePoints = new List<GameObject>();
    private List<GameObject> FreeCurvePoints = new List<GameObject>();
}

public class CurveManger : MonoBehaviour
{
    public void StartNewCurve(Vector3 startPos, Vector3 endPos, int numPoints)
    {
        var curvePointManger = NextFreeCurvePointManger();
        curvePointManger.StartCurve(startPos, endPos, numPoints, G, IntervalTime, TotalTime, DefaultFlyTime);
    }
    
    void Update()
    {
        foreach(var curvePointManger in ActiveCurvePointMangers)
        {
            if(curvePointManger.IsFinish())
            {
                PendingFreeCurvePointMangers.Add(curvePointManger);
            }      
        }

        if (PendingFreeCurvePointMangers.Count > 0)
        {
            foreach (var curvePointManger in PendingFreeCurvePointMangers)
            {
                FreeCurvePointMangers.Add(curvePointManger);
                ActiveCurvePointMangers.Remove(curvePointManger);
            }

            PendingFreeCurvePointMangers.Clear();
        }
    }

    private CurvePointManger NextFreeCurvePointManger()
    {
        CurvePointManger curvePointManger = null;

        if (FreeCurvePointMangers.Count > 0)
        {
            curvePointManger = FreeCurvePointMangers[0];
            FreeCurvePointMangers.RemoveAt(0);
        }
        else
        {
            curvePointManger = gameObject.AddComponent<CurvePointManger>();
        }

        if (curvePointManger && CurvePointPrefab)
        {
            curvePointManger.SetCurvePointPrefab(CurvePointPrefab);
            ActiveCurvePointMangers.Add(curvePointManger);
        }

        return curvePointManger;
    }

    public float IntervalTime = 0.1f;
    public float DefaultFlyTime = 0.5f;
    public float TotalTime = 2.0f;
    public float G = -9.8f;

    public GameObject CurvePointPrefab;

    private List<CurvePointManger> PendingFreeCurvePointMangers = new List<CurvePointManger>();
    private List<CurvePointManger> ActiveCurvePointMangers = new List<CurvePointManger>();
    private List<CurvePointManger> FreeCurvePointMangers = new List<CurvePointManger>();
}