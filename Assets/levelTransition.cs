using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using static Unity.Collections.AllocatorManager;

public class levelTransition : MonoBehaviour
{

    public CanvasGroup introText;

    public float enteringScene = 5f;
    public bool stillEntering = true;

    public bool winTriggered = false;
    public string nextScene;

    public Volume mainCameraVolume;
    public Bloom mainCameraBloom;

    // Start is called before the first frame update
    void Start()
    {
        //mainCameraBloom = mainCamera.GetComponent<Volume>().GetComponent<Bloom>();
        mainCameraVolume.profile.TryGet<Bloom>(out mainCameraBloom);

        // bloom in
        mainCameraBloom.active = true;
        mainCameraBloom.threshold.value = 0f;
        mainCameraBloom.intensity.value = 5000f;

        introText.alpha = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if(enteringScene > 0 && stillEntering)
        {
            enteringScene -= Time.deltaTime;
            mainCameraBloom.threshold.value += Time.deltaTime * 0.3f;
            mainCameraBloom.intensity.value -= Time.deltaTime * 700f;
            introText.alpha -= Time.deltaTime*0.3f;

        }
        if(enteringScene < 0 && stillEntering)
        {
            mainCameraBloom.active = false;
            stillEntering = false;
            introText.alpha = 0f;
        }


        GameObject[] listwow = GameObject.FindGameObjectsWithTag("Vertex");
        //Debug.Log(listwow.Length);
        //vertex_script[] listOfVertices = GetComponents<vertex_script>();
        bool winCond = true;
        //Debug.Log(listOfVertices.Length);
        for (int i = 0; i < listwow.Length; i++)
        {
            if (!listwow[i].GetComponent<vertex_script>().allConditionsFullfilled)
            {
                winCond = false;
            }
        }

        if (winCond && !winTriggered)
        {
            Debug.Log(" you win =) ");
            winTriggered = true;

            // TODO tweening libraries cant do this nicely or what? hmmm....
          //  TweenService.addTween();
          //LeanTween.value(mainCameraBloom.threshold.value,0f,100f,3)f
            mainCameraBloom.active = true;
            //mainCameraBloom.threshold.value = 0f;
           // mainCameraBloom.threshold.value
          //  mainCameraBloom.intensity.value = 5000f;
            Invoke("loadNextScene", 5);
        }

        if(winTriggered)
        {
            mainCameraBloom.threshold.value -= Time.deltaTime*0.3f;
            mainCameraBloom.intensity.value += Time.deltaTime * 700f;
        }
    }

    void loadNextScene()
    {
        if(nextScene != "Nope")
        {
            lineScript.clearLiners();
            SceneManager.LoadScene(nextScene);
        }
    }
}
