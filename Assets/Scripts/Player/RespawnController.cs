using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnController : MonoBehaviour
{

    public static RespawnController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    public GameObject deathEffect;

    private Vector3 respawnPoint;
    public float waitToRespawn;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerVitalsController.instance.gameObject;

        respawnPoint = player.transform.position;
    }

    public void SetSpawn(Vector3 newPosition)
    {
        respawnPoint = newPosition;
    }

    public void Respawn()
    {
        StartCoroutine(RespawnCo());
    }

    IEnumerator RespawnCo()
    {
        player.SetActive(false);
        if (deathEffect != null)
        {
            Instantiate(deathEffect, player.transform.position, player.transform.rotation);
        }

        yield return new WaitForSeconds(waitToRespawn);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        player.transform.position = respawnPoint;
        player.SetActive(true);

        PlayerVitalsController.instance.fillVital("health", "respawn");
        PlayerVitalsController.instance.fillVital("thirst", "respawn");
    }
}
