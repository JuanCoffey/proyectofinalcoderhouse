using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject CameraFollow;
    public GameObject CameraNorth;
    public GameObject CameraSouth;
    public GameObject CameraEast;
    public GameObject CameraWest;

    public List<GameObject> Cameras;
    private byte activeCameraIndex;

    void Start()
    {
        Cameras = new List<GameObject>() { CameraFollow, CameraNorth, CameraEast, CameraSouth, CameraWest };
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
         style.normal.textColor = Color.black;
        style.fontSize = 20;

        GUI.Label(new Rect(0, 0, 200, 200), "W,A,S,D to move", style);
        GUI.Label(new Rect(0, 50, 200, 200), "Q,E to rotate", style);
        GUI.Label(new Rect(0, 100, 200, 200), "Space to switch Cam", style);
    }

    void Update()
    {
        checkInput();
    }

    private void checkInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleCamera();
        }
    }

    public void ToggleCamera()
    {
        activeCameraIndex++;
        //Debug.LogError("activeCameraIndex  "+activeCameraIndex);
        if (activeCameraIndex > Cameras.Count - 1)
        {
            activeCameraIndex = 0;
        }

        for (int i = 0; i < Cameras.Count; i++)
        {
            if (i == activeCameraIndex)
            {
                Cameras[i].SetActive(true);
            }
            else
            {
                Cameras[i].SetActive(false);
            }
        }
    }
}
