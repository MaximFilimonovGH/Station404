using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{

    private PlayerController thePlayer;
    public BoxCollider2D cameraBounds;
    public Vector3 offset;
    [Range(1,10)] public float smoothFactor;

    private float halfHeight, halfWidth;

    // Start is called before the first frame update
    void Start()
    {
        halfHeight = Camera.main.orthographicSize;
        halfWidth = halfHeight * Camera.main.aspect;

        thePlayer = FindObjectOfType<PlayerController>();
        moveToPlayer();

        string sceneName = SceneManager.GetActiveScene().name;
        string levelNumber = sceneName.Substring(sceneName.LastIndexOf(" ") + 1);

        AudioManager.instance.PlayLevelMusic(int.Parse(levelNumber));
    }

    // Update is called once per frame
    void Update()
    {
        if (thePlayer != null)
        {
            followPlayer();
        }
        else
        {
            //thePlayer = FindObjectOfType<PlayerController>();
            thePlayer = PlayerVitalsController.instance.GetComponent<PlayerController>();
            moveToPlayer();
        }
    }
    void moveToPlayer()
    {
        Vector3 playerPosition = new Vector3(
                Mathf.Clamp(thePlayer.transform.position.x, cameraBounds.bounds.min.x + halfWidth, cameraBounds.bounds.max.x - halfWidth),
                Mathf.Clamp(thePlayer.transform.position.y, cameraBounds.bounds.min.y + halfHeight, cameraBounds.bounds.max.y - halfHeight),
                transform.position.z);

        transform.position = playerPosition;
    }

    void followPlayer()
    {
        Vector3 targetPosition = new Vector3(
                Mathf.Clamp(thePlayer.transform.position.x, cameraBounds.bounds.min.x + halfWidth, cameraBounds.bounds.max.x - halfWidth),
                Mathf.Clamp(thePlayer.transform.position.y, cameraBounds.bounds.min.y + halfHeight, cameraBounds.bounds.max.y - halfHeight),
                transform.position.z);

        Vector3 smoothPosition = Vector3.Lerp(transform.position, targetPosition, smoothFactor * Time.fixedDeltaTime);

        transform.position = smoothPosition;

        //transform.position = new Vector3(
        //    Mathf.Clamp(thePlayer.transform.position.x, cameraBounds.bounds.min.x + halfWidth, cameraBounds.bounds.max.x - halfWidth),
        //    Mathf.Clamp(thePlayer.transform.position.y, cameraBounds.bounds.min.y + halfHeight, cameraBounds.bounds.max.y - halfHeight),
        //    transform.position.z);
    }
}
