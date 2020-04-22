using UnityEngine;
using System.Collections;

public class DisableTracking : MonoBehaviour {
    private Camera cam;
    private Vector3 startPos;
//Useful when you want to disable VR/camera position tracking - link to headset movement..
    void Start () {
        cam = GetComponentInChildren<Camera>();
        startPos = transform.localPosition;
    }
	
    void Update () {
        transform.localPosition = startPos - cam.transform.localPosition;
        transform.localRotation = Quaternion.Inverse(cam.transform.localRotation);
    }
}