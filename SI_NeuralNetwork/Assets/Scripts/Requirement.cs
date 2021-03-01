using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Requirement : MonoBehaviour, IComparable<Requirement>
{
    public static Requirement instance;

    public List<int> listOfInt;
    public int[] arrayOfInt;
    public int[][] jaggedArray2DOfInt;
    public int[][][] jaggedArray3DOfInt;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {

    }

    private void TestJaggedArray3D()
    {
        //1 Dimension
        jaggedArray3DOfInt = new int[4][][];
        //2 Dimension
        for (int x = 0; x < jaggedArray3DOfInt.Length; x++)
        {
            //2D lenght 1 à 3
            jaggedArray3DOfInt[x] = new int[UnityEngine.Random.Range(1,4)][];
        }
        for (int x = 0; x < jaggedArray3DOfInt.Length; x++)
        {
            for (int y = 0; y < jaggedArray3DOfInt[x].Length; y++)
            {
                //3D lenght 1 à 3
                jaggedArray3DOfInt[x][y] = new int[UnityEngine.Random.Range(1, 4)];
            }
        }


        for (int x = 0; x < jaggedArray3DOfInt.Length; x++)
        {
            for (int y = 0; y < jaggedArray3DOfInt[x].Length; y++)
            {
                for (int z = 0; z < jaggedArray3DOfInt[x][y].Length; z++)
                {
                    Debug.Log(jaggedArray3DOfInt[x][y][z]);
                    GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    Instantiate(go,new Vector3(x,y,z),Quaternion.identity);
                    Material mat = go.GetComponent<Renderer>().material;
                }
            }
        }
    }

    private void TestJaggedArray2D()
    {
        jaggedArray2DOfInt = new int[4][];
        jaggedArray2DOfInt[0] = new int[2];
        jaggedArray2DOfInt[1] = new int[3];
        jaggedArray2DOfInt[2] = new int[1];
        jaggedArray2DOfInt[3] = new int[6];

        jaggedArray2DOfInt[0][0] = 1;

        for (int x = 0; x < jaggedArray2DOfInt.Length; x++)
        {
            for (int y = 0; y < jaggedArray2DOfInt[x].Length; y++)
            {
                Debug.Log(jaggedArray2DOfInt[x][y]);
            }
        }
    }

    private void TestArray()
    {
        arrayOfInt = new int[4];

    }

    private void TestList()
    {
        listOfInt = new List<int>();

        listOfInt.Add(123);

    }

    public int CompareTo(Requirement other)
    {
        if (other.listOfInt == this.listOfInt)
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }
}
