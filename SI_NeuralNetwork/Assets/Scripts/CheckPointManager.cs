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

        }
    }


}
