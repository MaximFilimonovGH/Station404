using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementRun : MonoBehaviour
{

    private CameraController theCamera;
    public Transform movePoint;
    public float cameraSpeed;

    private bool timeToRun;
    public Transform startPoint;

    private float halfHeight, halfWidth;

    private PlayerController player;

    public float timeToStartMove;
    private float startCounter;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            theCamera.enabled = false;
            startCounter = timeToStartMove;
            timeToRun = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        theCamera = FindObjectOfType<CameraController>();
        halfHeight = Camera.main.orthographicSize;
        halfWidth = halfHeight * Camera.main.aspect;
        player = PlayerVitalsController.instance.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (startCounter > 0)
        {
            startCounter -= Time.deltaTime;
        }
        else
        {
            theCamera.transform.position = Vector3.MoveTowards(theCamera.transform.position, movePoint.position, cameraSpeed * Time.deltaTime);
        }

        if (player.transform.position.x - halfWidth > theCamera.transform.position.x)
        {
            RespawnController.instance.SetSpawn(startPoint.position);
            theCamera.enabled = true;
            timeToRun = false;
            PlayerVitalsController.instance.DamagePlayerVital(100, "health");
        }

    }
}
