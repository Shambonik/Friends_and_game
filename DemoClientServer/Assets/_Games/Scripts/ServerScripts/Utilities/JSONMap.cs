using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class JSONMap : MonoBehaviour
{

    [SerializeField] private GameObject point;
    private List<List<Vector2>> listFigures = new List<List<Vector2>>();

    private int[,] pointArray = new int[600,600];
        // Start is called before the first frame update
    void Start()
    {
        
        NavMeshHit hit;
        for (int i = -300; i < 300; i++)
        {
            for (int j = -300; j < 300; j++)
            {
                Vector3 position = new Vector3(((float)i)/2, 0, ((float)j)/2);
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
                if(!valid) pointArray[i + 300, j + 300] = 1;
                else pointArray[i + 300, j + 300] = 0;
            }
        }

        for (int i = 0; i < 600; i++)
        {
            for (int j = 0; j < 600; j++)
            {
                if (pointArray[i, j] == 1 && i > 0 && i < 598 && j > 0 && j < 598)
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
        
        for (int i = 0; i < 600; i++)
        {
            for (int j = 0; j < 600; j++)
            {
                if (pointArray[i, j] == 2)
                {
                    /*int iFirst = i;
                    int jFirst = j;*/
                    var points = addPoly(i, j, new List<Vector2>(), Vector2.zero);
                    listFigures.Add(points);
                    //pointArray[i, j] = 2;
                }
            }
        }

        // var g = Instantiate(point);
        // g.transform.position = new Vector3(i, 20, j);
    }
    

    private List<Vector2> addPoly(int i, int j, List<Vector2> points, Vector2 prevPoint)
    {
        pointArray[i, j] = 1;
        points.Add(new Vector2(i, j));
        if (i > 0 && j > 0 && pointArray[i - 1, j - 1] == 2)
        {
            pointArray[i - 1, j] = 0;
            pointArray[i, j - 1] = 0;
            points = addPoly(i - 1, j - 1, points, new Vector2(i, j));
        }
        else if (i < 599 && j < 599 && pointArray[i + 1, j + 1] == 2)
        {
            pointArray[i + 1, j] = 0;
            pointArray[i, j + 1] = 0;
            points = addPoly(i + 1, j + 1, points, new Vector2(i, j));
        }
        else if (i < 599 && j > 0 && pointArray[i + 1, j - 1] == 2)
        {
            pointArray[i + 1, j] = 0;
            pointArray[i, j - 1] = 0;
            points = addPoly(i + 1, j - 1, points, new Vector2(i, j));
        }
        else if (i > 0 && j < 599 && pointArray[i - 1, j + 1] == 2)
        {
            pointArray[i - 1, j] = 0;
            pointArray[i, j + 1] = 0;
            points = addPoly(i - 1, j + 1, points, new Vector2(i, j));
        }
        else if (i > 0 && pointArray[i - 1, j] == 2)
        {
            points = addPoly(i-1, j, points, new Vector2(i, j));
        }
        else if (i < 599 && pointArray[i + 1, j] == 2)
        {
            points = addPoly(i + 1, j, points, new Vector2(i, j));
        }
        else if (j < 599 && pointArray[i, j+1] == 2)
        {
            points = addPoly(i, j+1, points, new Vector2(i, j));
        }
        else if (j > 0 && pointArray[i, j-1] == 2)
        {
            points = addPoly(i, j-1, points, new Vector2(i, j));
        }
        else if (prevPoint != Vector2.zero)
        {
            points = addPoly((int)prevPoint.x, (int)prevPoint.y, points, Vector2.zero);
        }
        return points;
    }

    private void OnDrawGizmos()
    {
        foreach (var points in listFigures)
        {
            Vector3 previousPoint = Vector2.zero;
            int i = 0;
            Gizmos.color = Color.black;
            foreach (var pointCoord in points)
            {
                Vector3 point3 = new Vector3((pointCoord.x-300)/2, 20, (pointCoord.y-300)/2);
                if (i > 0)
                {
                    Gizmos.DrawLine(previousPoint, point3);
                }
                previousPoint = point3;
                i++;
                //var g = Instantiate(point);
                //g.transform.position = new Vector3((pointCoord.x-300)/2, 20, (pointCoord.y-300)/2);
            }
            Gizmos.DrawLine(previousPoint, new Vector3((points[0].x-300)/2, 20, (points[0].y-300)/2));
        }
        //throw new NotImplementedException();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
