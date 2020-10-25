using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class JSONMap : MonoBehaviour
{

    [SerializeField] private GameObject point;

    private int[,] pointArray = new int[300,300];
        // Start is called before the first frame update
    void Start()
    {
        
        NavMeshHit hit;
        for (int i = -150; i < 150; i++)
        {
            for (int j = -150; j < 150; j++)
            {
                Vector3 position = new Vector3(i, 0, j);
                bool valid = false;
                for (float k = -1; k < 2; k += 0.2f)
                {
                    position = new Vector3(position.x, transform.position.y + k, position.z);
                    bool isValid = NavMesh.SamplePosition(position, out hit, .3f, NavMesh.AllAreas);
                    if (isValid)
                    {
                        valid = true;
                        break;
                    }
                }

                //var g = Instantiate(point);
                //g.transform.position = new Vector3(position.x, 20, position.z);
                if(!valid) pointArray[i + 150, j + 150] = 1;
                else pointArray[i + 150, j + 150] = 0;
            }
        }

        for (int i = 0; i < 300; i++)
        {
            for (int j = 0; j < 300; j++)
            {
                if (pointArray[i, j] == 1 && i > 0 && i < 298 && j > 0 && j < 298)
                {
                    Debug.Log("Zashel");
                    for (int i2 = i - 1; i2 <= i + 1; i2++)
                    {
                        for (int j2 = j - 1; j2 <= j + 1; j2++)
                        {
                            if (pointArray[i2, j2] == 0)
                            {
                                pointArray[i, j] = 2;
                                break;
                            }
                        }
                        if (pointArray[i, j] == 2) break;
                    }
                }
                else if(pointArray[i, j] == 1)
                {
                    pointArray[i, j] = 2;
                }
            }
        }
        for (int i = 0; i < 300; i++)
        {
            for (int j = 0; j < 300; j++)
            {
                if (pointArray[i, j] == 2)
                {
                    var g = Instantiate(point);
                    g.transform.position = new Vector3(i-150, 20, j-150);
                }
            }
        }
        // var g = Instantiate(point);
        // g.transform.position = new Vector3(i, 20, j);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
