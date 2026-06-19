using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResultData
{
    public int levelId;

    public bool completed;

    public float phase1Time;
    public float phase2Time;
    public float averageTime;

    public int phase1Stars;
    public int phase2Stars;
}