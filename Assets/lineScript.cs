using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lineScript : MonoBehaviour
{

    public static List<GameObject> lines = new List<GameObject>();

    public GameObject vert1 = null;
    public GameObject vert2 = null;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<LineRenderer>().SetPosition(0,vert1.transform.position);
        this.GetComponent<LineRenderer>().SetPosition(1,vert2.transform.position);
        //lines = new List<GameObject>();
    }

    public static void clearLiners()
    {
        lines.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<LineRenderer>().SetPosition(0, vert1.transform.position);
        this.GetComponent<LineRenderer>().SetPosition(1, vert2.transform.position);
    }

    public static GameObject getLine(GameObject searchVert1, GameObject searchVert2)
    {
        for (int i = 0; i<lines.Count; i++)
        {
            if(lines[i].GetComponent<lineScript>().vert1 == searchVert1 && lines[i].GetComponent<lineScript>().vert2 == searchVert2)
            {
                return lines[i];
            }
            else if (lines[i].GetComponent<lineScript>().vert1 == searchVert2 && lines[i].GetComponent<lineScript>().vert2 == searchVert1)
            {
                return lines[i];
            }
        }

        return null;
    }

    public static bool isVertexConnected(GameObject vert)
    {
        for (int i = 0; i < lines.Count; i++)
        {
            if (lines[i].GetComponent<lineScript>().vert1 == vert || lines[i].GetComponent<lineScript>().vert2 == vert)
            {
                return true;
            }
        }
        return false;
    }

    public static int vertexConnectionCount(GameObject vert)
    {
        int count = 0;
        for (int i = 0; i < lines.Count; i++)
        {
            if (lines[i].GetComponent<lineScript>().vert1 == vert || lines[i].GetComponent<lineScript>().vert2 == vert)
            {
                count += 1;
            }
        }
        return count;
    }

}
