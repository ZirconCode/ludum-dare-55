using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroy_after_t : MonoBehaviour
{
    // Start is called before the first frame update

    public float timeToLive = 0.5f;

    void Start()
    {
       // Debug.Log(this.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        timeToLive -= Time.deltaTime;
        if(timeToLive < 0 )
        {
            //Debug.Log(timeToLive);
            Destroy(this.gameObject);
        }
                
    }
}
