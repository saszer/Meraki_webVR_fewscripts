using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Linq;

/*
From Meraki VR Experience : https://design.embracingearth.space/2018/12/13/meraki-1-o/
This is a modified script which controls the Camera Dynamics: 
In Meraki 1.0 WebVR Experience, you can switch between two Camera Dynamics. 
One of the camera dynamics is such a way to respresent immersion of looking inside oneself. 
The Rotation from VR Headset is converted into a lookat heart object, so when user rotates their head(in VR) -
instead of rotating normally(from a point) as it should, the camera rotates around the object(heart) -lookAT

Original Script is from Mozilla WebVR https://github.com/MozillaReality/unity-webxr-export/tree/master/Assets/WebXR/Scripts
This Script is modified WebVRCamera.cs and will not work independently but needs to be combined with the repo.
*/
public class Uppermgmt : MonoBehaviour
{
    private const float Y_ANGLE_MIN = 0.0f;
    private const float Y_ANGLE_MAX = 50.0f;
    //Adding the object you want the camera to rotate around - lookAt.
    public Transform lookAt;
    public Transform camTransform;
    public float distance = 10.0f;

    private float currentX = 0.0f;
    private float currentY = 0.0f;
    private float sensitivityX = 4.0f;
    private float sensitivityY = 1.0f;
	
	private Matrix4x4 sitStand;

    private Camera cameraMain, cameraL, cameraR;
    public bool vrActive = true;
	
	
	
	
	
    [DllImport("__Internal")]
    private static extern void PostRender();
	
    private IEnumerator endOfFrame()
    {
        // Wait until end of frame to report back to WebVR browser to submit frame.
        yield return new WaitForEndOfFrame();
        PostRender ();
	}
	
	 void OnEnable()
    {	 
        WebVRManager.Instance.OnVRChange += onVRChange;
        WebVRManager.Instance.OnHeadsetUpdate += onHeadsetUpdate;
        
        cameraMain = GameObject.Find("CameraMain").GetComponent<Camera>();
        cameraL = GameObject.Find("CameraL").GetComponent<Camera>();
        cameraR = GameObject.Find("CameraR").GetComponent<Camera>();

        cameraMain.transform.Translate(new Vector3(0, WebVRManager.Instance.DefaultHeight, 0));
    }

    private void Start()
    {  
        camTransform = transform;
		
    }

	   void Update()
    { 
	   
	   {
        if (vrActive)
        {   
            cameraMain.enabled = false;
            cameraL.enabled = true;
            cameraR.enabled = true;
	   } 
	    
        else
        {
            cameraMain.enabled = true;
            cameraL.enabled = false;
            cameraR.enabled = false;
        }
	   } 
        #if !UNITY_EDITOR && UNITY_WEBGL
        // Calls Javascript to Submit Frame to the browser WebVR API.
        StartCoroutine(endOfFrame());
        #endif
    } 

	   private void onVRChange(WebVRState state)
    {
        vrActive = state == WebVRState.ENABLED;
    }
	
	private void onHeadsetUpdate (
        Matrix4x4 leftProjectionMatrix,
        Matrix4x4 leftViewMatrix,
        Matrix4x4 rightProjectionMatrix,
        Matrix4x4 rightViewMatrix,
        Matrix4x4 sitStandMatrix)
    {
        if (vrActive)
        {
            SetTransformFromViewMatrix (leftViewMatrix * sitStandMatrix.inverse);
            cameraL.projectionMatrix = leftProjectionMatrix;
            SetTransformFromViewMatrix (rightViewMatrix * sitStandMatrix.inverse);
            cameraR.projectionMatrix = rightProjectionMatrix;
        }
    }
	
    private void SetTransformFromViewMatrix(Matrix4x4 webVRViewMatrix)
    {
	    Matrix4x4 trs = TransformViewMatrixToTRS(webVRViewMatrix);
		
        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.LookRotation(trs.GetColumn(2), trs.GetColumn(1));
        //Main Change done for Meraki VR experience >> 
        //This Links the Rotation of VR headset to an inverted LookAt.
        camTransform.position = lookAt.position + rotation * dir;
        camTransform.LookAt(lookAt.position);
    }

/* Orignal Code from Mozilla WebVR which acts on normal camera dynamics , Linked to VR headset. 
    In Meraki VR experience - Another Script containing this structure on another camera exists and this camera dynamic can 
    be switched between, to experience both.

      // According to https://answers.unity.com/questions/402280/how-to-decompose-a-trs-matrix.html
    private void SetTransformFromViewMatrix(Transform transform, Matrix4x4 webVRViewMatrix)
    {
        Matrix4x4 trs = TransformViewMatrixToTRS(webVRViewMatrix);
        transform.localPosition = trs.GetColumn(3);
        transform.localRotation = Quaternion.LookRotation(trs.GetColumn(2), trs.GetColumn(1));
        transform.localScale = new Vector3(
            trs.GetColumn(0).magnitude,
            trs.GetColumn(1).magnitude,
            trs.GetColumn(2).magnitude
        );
    }
*/
	
	  // According to https://forum.unity.com/threads/reproducing-cameras-worldtocameramatrix.365645/#post-2367177
    private Matrix4x4 TransformViewMatrixToTRS(Matrix4x4 openGLViewMatrix)
    {
        openGLViewMatrix.m20 *= -1;
        openGLViewMatrix.m21 *= -1;
        openGLViewMatrix.m22 *= -1;
        openGLViewMatrix.m23 *= -1;
        return openGLViewMatrix.inverse;
    }
	
	
}
