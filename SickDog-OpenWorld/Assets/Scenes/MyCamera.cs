using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCamera : MonoBehaviour
{
    private Transform cameraPos;
    // Start is called before the first frame update
    void Start()
    {
        cameraPos = Dage.Instance.transform.Find("cameraPos");
        transform.rotation= cameraPos.rotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //lerp函数实现镜头跟随

        //transform.position = cameraPos.position;
        //transform.rotation = cameraPos.rotation;

        //transform.position =Vector3.Lerp(transform.position,cameraPos.position,0.5f);
        //transform.rotation = Quaternion.Lerp(transform.rotation, cameraPos.rotation,0.1f);


        transform.LookAt(Dage.Instance.transform.position);
    }
}
