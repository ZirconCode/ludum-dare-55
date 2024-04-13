using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
//using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class vertex_script : MonoBehaviour
{

    public Animator animator;

    public GameObject linePrefab;
    private SpriteRenderer selfSprite;

    public GameObject particleEffect;
    //private GameObject effect;

    static GameObject selectedVertex = null;

    private bool mouseIn = false;
    private Vector3 mouseDownPos;
    float dragDistanceBarrier = 0.2f;
    private bool dragging = false;

    //   public BoxCollider2D tableCollider;
    //   public CircleCollider2D activeCircleCollider;
    // [SerializeField]
    // public Bounds test;

    // sigh...
    [SerializeField]
    public Vector2 activeCircleCenter;
    [SerializeField]
    public float activeCircleRadius;

    [SerializeField]
    public Vector2 tableTopLeft;
    [SerializeField]
    public Vector2 tableBottomRight;

    //public List<condition> conditions = new List<condition>();
    //public struct condition
    //{
       // string conditionType;
       // int conditionNumber;
    //}

    public GameObject infoBubble;

    public int connectionsRequired = 0;
    public bool allConditionsFullfilled = false;


    // Start is called before the first frame update
    void Start()
    {
        selfSprite = GetComponent<SpriteRenderer>();
        mouseDownPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // sigh...
        //  (-5.63, 4.47, -10.00)
        // tableTopLeft = new Vector2(-0.04f - (11.38932f / 2f) , -0.747f - (9.452839f / 2f) + 0.4f);

        // (8.34, -4.73, -10.00)
        //tableBottomRight = new Vector2(-0.04f + (11.38932f / 2f), -0.747f + (9.452839f / 2f));

        // condition friendCon = new condition("",1);

        string tmpText = "0";
        if(connectionsRequired > 0)
        {
            tmpText = "";
            for (int i = 0; i < connectionsRequired; i++)
            {
                tmpText += "I";
            }
        }    
        //infoBubble.GetComponentInChildren<Canvas>(true).GetComponentInChildren<TextMeshPro>(true).text = tmpText;
        infoBubble.GetComponentInChildren<TextMeshProUGUI>(true).text = tmpText;
    }

    // Update is called once per frame
    void Update()
    {
        bool thisVertexInCircle = (Vector2.Distance(activeCircleCenter, this.gameObject.transform.position) < activeCircleRadius);
        if (thisVertexInCircle && lineScript.vertexConnectionCount(this.gameObject) == connectionsRequired) // TODO and in circle
        {
            selfSprite.color = Color.green;
            animator.SetInteger("state", 2);
            allConditionsFullfilled = true;
        }
        else
        {
            selfSprite.color = Color.white;
            animator.SetInteger("state", 0);
            allConditionsFullfilled = false;
        }

        //selfSprite.color = Color.red;
        if (selectedVertex == this.gameObject)
        {
            selfSprite.color = Color.blue;
            animator.SetInteger("state", 1);
        }
        else if (mouseIn)
        {
            selfSprite.color = Color.red;
            animator.SetInteger("state", 1);
        }

    }
    private void OnMouseEnter()
    {
        Vector3 mouseCoord = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        bool mouseInCircle = (Vector2.Distance(activeCircleCenter, mouseCoord) < activeCircleRadius);
        //bool mouseInTable = (mouseCoord.x < tableBottomRight.x && mouseCoord.x > tableTopLeft.x && mouseCoord.y > tableBottomRight.y && mouseCoord.y < tableTopLeft.y);

        // selfSprite.color = Color.red;
        mouseIn = true;

        // GameObject effect =
        if(mouseInCircle)
        {
            GameObject.Instantiate(particleEffect, transform.position, particleEffect.transform.rotation); // Input.mousePosition, Quaternion.identity, canvas.transform
        }
        //Debug.Log(effect.gameObject.transform.position);
        //Invoke("DestroyEffect", 0.5f);

        infoBubble.SetActive(true);
    }

    private void OnMouseDown()
    {
        mouseDownPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragging = false;
    }

    private void OnMouseUp()
    {
        Vector3 mouseCoord = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        bool mouseInCircle = (Vector2.Distance(activeCircleCenter, mouseCoord) < activeCircleRadius);
        //bool mouseInTable = (mouseCoord.x < tableBottomRight.x && mouseCoord.x > tableTopLeft.x && mouseCoord.y > tableBottomRight.y && mouseCoord.y < tableTopLeft.y);
        bool thisVertexInCircle = (Vector2.Distance(activeCircleCenter, this.gameObject.transform.position) < activeCircleRadius);

        if (!dragging)
        {
            if (selectedVertex == null)
            {
                if(mouseInCircle)
                {
                    selectedVertex = this.gameObject;
                }
                
                //selfSprite.color = Color.green;
            }
            else if (selectedVertex == this.gameObject)
            {
                selectedVertex = null;
            }
            else
            {

                if(thisVertexInCircle)
                {
                    if (lineScript.getLine(selectedVertex, this.gameObject) == null)
                    {
                        GameObject tmpLine = GameObject.Instantiate(linePrefab, linePrefab.transform.position, linePrefab.transform.rotation);
                        tmpLine.GetComponent<lineScript>().vert1 = this.gameObject;
                        tmpLine.GetComponent<lineScript>().vert2 = selectedVertex;
                        lineScript.lines.Add(tmpLine);
                    }
                    else
                    {
                        GameObject tmpLine = lineScript.getLine(selectedVertex, this.gameObject);
                        lineScript.lines.Remove(tmpLine);
                        Destroy(tmpLine);
                    }

                    selectedVertex = null;
                    //lineScript.lines.Add
                    // connect them with a line or something
                }

            }
        }
       
        

        
    }

    private void OnMouseDrag()
    {
        Vector3 mouseCoord = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log(mouseCoord);

        if (!dragging)
        {
            if (Vector3.Distance(mouseDownPos, mouseCoord) > dragDistanceBarrier)
            {
                dragging = true;
            }
        }
        if(dragging)
        {
            // unselect this vertex
            selectedVertex = null;


            //  Debug.Log(activeCircleCollider.bounds.Contains(mouseCoord));
            // Debug.Log(activeCircleCollider.bounds);
            //if(tableCollider.bounds.Contains(mouseCoord))

            //Debug.Log(Vector2.Distance(activeCircleCenter, mouseCoord));
            bool mouseInCircle = (Vector2.Distance(activeCircleCenter, mouseCoord) < activeCircleRadius);
            bool mouseInTable = (mouseCoord.x < tableBottomRight.x && mouseCoord.x > tableTopLeft.x && mouseCoord.y > tableBottomRight.y && mouseCoord.y < tableTopLeft.y);


            if (mouseInCircle)
            {
                this.transform.position = new Vector3(mouseCoord.x, mouseCoord.y, 0.21f); // TODO hardcoded Z...
            }

            if(mouseInTable && !mouseInCircle)
            {
                if(! lineScript.isVertexConnected(this.gameObject))
                {
                    this.transform.position = new Vector3(mouseCoord.x, mouseCoord.y, 0.21f); // TODO hardcoded Z...
                }
            }


        }

      //  Debug.Log("dist"+Vector2.Distance(this.transform.position, mouseCoord));
       // Vector2.Distance(this.transform.position, mouseCoord);
        //   Input.mousePosition;
      //  Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
      //  float tmpZ = this.transform.position.z;
       ////////////// 
     //   Debug.Log("settingpre " + this.transform.position);
       // this.transform.position.Set(this.transform.position.x, this.transform.position.y, 0.21f);
       // Debug.Log("setting "+this.transform.position);
      //  this.GetComponent<SpriteRenderer>().ren
    }


    public void DestroyEffect()
    {
        //Destroy(effect);
    }

    private void OnMouseExit()
    {
       // selfSprite.color = Color.white;
        mouseIn = false;

        infoBubble.SetActive(false);
    }

}
