using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    public Animator doorAnim;

    public float distanceToOpen;
    private bool isDoorOpen;

    private PlayerController player;
    private PlayerProgressTracker gameProgress;

    private bool playerExiting;

    public Transform exitPoint;
    public float movePlayerSpeed;

    public string levelToLoad;

    public BoxCollider2D exitObjectCollider;

    public int doorNumber;

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerVitalsController.instance.GetComponent<PlayerController>();
        gameProgress = PlayerVitalsController.instance.GetComponent<PlayerProgressTracker>();

        // open door from start if correct doorOpen progress tracked
        if (doorNumber == 0)
        {
            if (gameProgress.zeroDoorOpen)
            {
                doorAnim.SetBool("doorOpen", true);
                isDoorOpen = true;
                exitObjectCollider.enabled = false;
            }
            else
            {
                doorAnim.SetBool("doorOpen", false);
                isDoorOpen = false;
                exitObjectCollider.enabled = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (doorNumber == 0)
        {
            if (gameProgress.zeroDoorOpen)
            {
                doorAnim.SetBool("doorOpen", true);
                isDoorOpen = true;
                exitObjectCollider.enabled = false;
            }
            else
            {
                doorAnim.SetBool("doorOpen", false);
                isDoorOpen = false;
                exitObjectCollider.enabled = true;
            }
        }
        // space for more doors

        if (playerExiting)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, exitPoint.position, movePlayerSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            if (!playerExiting && isDoorOpen && !player.isExitingLevel)
            {
                player.canMove = false;

                StartCoroutine(UseDoorCo());
            }
        }
    }

    IEnumerator UseDoorCo()
    {
        playerExiting = true;
        player.isExitingLevel = true;

        player.thePlayerAnimations.enabled = false;

        UIController.instance.startFadeToBlack();

        yield return new WaitForSeconds(1.5f);

        RespawnController.instance.SetSpawn(exitPoint.position);
        player.canMove = true;
        player.thePlayerAnimations.enabled = true;

        UIController.instance.startFadeFromBlack();

        PlayerPrefs.SetString("ContinueLevel", levelToLoad);
        PlayerPrefs.SetFloat("PosX", exitPoint.position.x);
        PlayerPrefs.SetFloat("PosY", exitPoint.position.y);
        PlayerPrefs.SetFloat("PosZ", exitPoint.position.z);

        player.isExitingLevel = false;

        SceneManager.LoadScene(levelToLoad);
    }
}
