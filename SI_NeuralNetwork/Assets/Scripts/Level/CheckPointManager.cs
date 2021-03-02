using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    [HideInInspector]
    public static CheckPointManager instance;

    [Header("Component")]
    public Transform checkPointsParent;

    [Header("Variable")]
    public Transform[] checkPointsArray;

    private void Awake()
    {
        instance = this;
        checkPointsParent = this.transform;
        Initialisation();
    }

    [ContextMenu("SetCheckPoints")]
    public void Initialisation()
    {
        //Generation de l'array
        checkPointsArray = new Transform[checkPointsParent.childCount];

        for (int i = 0; i < checkPointsArray.Length; i++)
        {
            //Remplis l'array
            checkPointsArray[i] = checkPointsParent.GetChild(i);

            //Ajoute le component Checkpoint s'il ne l'a pas déjà
            if (!checkPointsArray[i].gameObject.GetComponent<CheckPoint>())
            {
                checkPointsArray[i].gameObject.AddComponent(typeof(CheckPoint));
            }

            //Lui donne le prochain checkpoint
            checkPointsArray[i].gameObject.GetComponent<CheckPoint>().nextCheckpoint = checkPointsParent.GetChild((i + 1) % (checkPointsParent.childCount));
        }
    }
}
