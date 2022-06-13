using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    private PlayerController thePlayer;
    public BoxCollider2D cameraBounds;

    private float halfHeight, halfWidth;

    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<PlayerController>();
        halfHeight = Camera.main.orthographicSize;
        halfWidth = halfHeight * Camera.main.aspect;
    }

    // Update is called once per frame
    void Update()
    {
        if (thePlayer != null)
        {
            transform.position = new Vector3(
                Mathf.Clamp(thePlayer.transform.position.x, cameraBounds.bounds.min.x + halfWidth, cameraBounds.bounds.max.x - halfWidth),
                Mathf.Clamp(thePlayer.transform.position.y, cameraBounds.bounds.min.y + halfHeight, cameraBounds.bounds.max.y - halfHeight),
                transform.position.z);
        } else
        {
            thePlayer = FindObjectOfType<PlayerController>();
        }
    }
}
