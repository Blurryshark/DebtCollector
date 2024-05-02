using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleAnimation : MonoBehaviour {
    public float scaleAmount = 0.1f;
    public float rotationAmount = 20f;
    public float speed = 2f;

    private Vector3 origSz;
    private float origRot;
    
    void Start() {
        origSz = transform.localScale;
        origRot = transform.localEulerAngles.z;
    }

    // Update is called once per frame
    void Update() {
        float scale = Mathf.Sin(Time.time * speed) * scaleAmount;
        transform.localScale = origSz + new Vector3(scale, scale, scale);

        float rotation = Mathf.Sin(Time.time * speed) * rotationAmount;
        transform.localRotation = Quaternion.Euler(0f, 0f, origRot + rotation);
    }
}
