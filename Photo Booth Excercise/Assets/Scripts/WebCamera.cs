using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class WebCamera : MonoBehaviour
{
    [Header("Webcam")]
    [SerializeField] private RawImage display;
    private WebCamTexture webCam;
    [SerializeField] private GameObject UIWebCam;
    [SerializeField] private GameObject takePhotoButton;

    [Header("Take Photo")]
    [SerializeField] private GameObject flash;
    [SerializeField] private RawImage photoTaken;
    [SerializeField] private GameObject acceptButton;

    [Header("Scene")]
    [SerializeField] bool saveInStreamingAssets = true;
    [SerializeField] ChangeScene changeScene;
    private float maxTime = 60;
    [SerializeField] private float actualTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartWebCamera();
        PrepareStartUI();
    }

    private void StartWebCamera()
    {
        webCam = new WebCamTexture();

        if (!webCam.isPlaying)
        {
            webCam.Play();
        }

        display.texture = webCam;
        display.gameObject.SetActive(true);
    }

    private void PrepareStartUI()
    {
        takePhotoButton.SetActive(true);
        acceptButton.SetActive(false);
        photoTaken.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        actualTime += Time.deltaTime;
        if (actualTime >= maxTime)
        {
            StopCamera();
            changeScene.SetScene();
        }

        if (Input.GetMouseButtonDown(0))
        {
            actualTime = 0;
        }
    }

    public void TakePhoto()
    {
        Debug.Log("Photo");
        StartCoroutine(CapturePhoto());
    }

    IEnumerator CapturePhoto()
    {
        UIWebCam.SetActive(false);

        yield return new WaitForEndOfFrame();
        
        Texture2D texture = ScreenCapture.CaptureScreenshotAsTexture(4);
        photoTaken.texture = texture;
        flash.gameObject.SetActive(true);
        
    }

    public void PhotoTaken()
    {
        UIWebCam.SetActive(true);
        photoTaken.gameObject.SetActive(true);
        takePhotoButton.SetActive(false);
        acceptButton.SetActive(true);
    }

    public void SavePhoto()
    {
        Save();
        PrepareStartUI();
    }

    private void Save()
    {
        Texture2D textureSave = photoTaken.texture as Texture2D;
        string name = "Photo" + System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".png";
        string path;
        if (saveInStreamingAssets)
        {
            path = Application.streamingAssetsPath + "/" + name;
        }
        else
        {
            path = Application.persistentDataPath + "/" + name;
        }

        byte[] bytes = textureSave.EncodeToPNG();
        File.WriteAllBytes(path, bytes);
    }

    public void StopCamera()
    {
        if (webCam.isPlaying)
        {
            webCam.Stop();
        }
    }
}
