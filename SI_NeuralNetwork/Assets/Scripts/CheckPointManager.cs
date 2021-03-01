using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    public Transform checkPointsParent;

    public Transform[] checkPointsArray;

    private void Start()
    {
        checkPointsArray = new Transform[checkPointsParent.childCount];

        for (int i = 0; i < checkPointsArray.Length; i++)
        {
            checkPointsArray[i] = checkPointsParent.GetChild(i);
            checkPointsArray[i].gameObject.AddComponent(typeof(CheckPoint));
            checkPointsArray[i].gameObject.GetComponent<CheckPoint>().nextCheckpoint = checkPointsParent.GetChild(i+1);
        }
    }


}
