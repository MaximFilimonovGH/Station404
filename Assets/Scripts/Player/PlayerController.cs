using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Rigidbody2D thePlayer;

    public float moveSpeed;
    public float jumpForce;

    // ground point
    public Transform groundPoint;
    private bool isOnGround;
    public LayerMask whatIsGround;

    // can move
    public bool canMove;

    // abilities
    private PlayerAbilityTracker abilities;
    // jump related
    private bool canDoubleJump;
    // dash related
    public float dashSpeed, dashTime, dashCooldown;
    private float dashCounter, dashRechargeCounter;
    // after image related for dashing
    public SpriteRenderer theSR, afterImage;
    public float afterImageLifetime, timeBetweenAfterImages;
    private float afterImageCounter;
    public Color afterImageColor;

    // shot
    public ShotController shotToFire;
    public Transform shotPoint;

    // animations
    public Animator thePlayerAnimations;

    public bool isExitingLevel;

    // Start is called before the first frame update
    void Start()
    {
        // assign abilities booleans
        abilities = GetComponent<PlayerAbilityTracker>();
        
        // we can move from the start, right
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        // if we can move and time is not frozen
        if (canMove && Time.timeScale != 0)
        {

            // check if dash is on cooldown
            if (dashRechargeCounter > 0)
            {
                dashRechargeCounter -= Time.deltaTime;
            }
            // dash is not on cooldown
            else
            {
                if (Input.GetButtonDown("Fire2") && abilities.canDash)
                {
                    dashCounter = dashTime;

                    ShowAfterImage();
                }
            }

            // if dashing now
            if (dashCounter > 0)
            {
                dashCounter -= Time.deltaTime;

                thePlayer.velocity = new Vector2(dashSpeed * transform.localScale.x, thePlayer.velocity.y);

                // show after images
                afterImageCounter -= Time.deltaTime;
                if (afterImageCounter <= 0)
                {
                    ShowAfterImage();
                }

                dashRechargeCounter = dashCooldown;
            }
            // if not dashing
            else
            {
                // move sideways
                thePlayer.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, thePlayer.velocity.y);

                // handle direction change
                if (thePlayer.velocity.x < 0)
                {
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                }
                else if (thePlayer.velocity.x > 0)
                {
                    transform.localScale = Vector3.one;
                }
            }

            // check if on ground
            isOnGround = Physics2D.OverlapCircle(groundPoint.position, .2f, whatIsGround);

            // jump if on ground
            if (Input.GetButtonDown("Jump") && (isOnGround || (canDoubleJump && abilities.canDoubleJump)))
            {
                // if on ground level then jump first time
                if (isOnGround)
                {
                    canDoubleJump = true; // set that double jump is possible
                }
                // now we are double jumping
                else
                {
                    canDoubleJump = false; // set that double jump is not possible
                    thePlayerAnimations.SetTrigger("doubleJump");
                }

                thePlayer.velocity = new Vector2(thePlayer.velocity.x, jumpForce);
            }

            // shooting
            if (Input.GetButtonDown("Fire1"))
            {
                ShotController shot = Instantiate(shotToFire, shotPoint.position, shotPoint.rotation);
                shot.moveDirection = new Vector2(transform.localScale.x, 0f);
                thePlayerAnimations.SetTrigger("shotFired");
            }

            // dying on Enter
            if (Input.GetButtonDown("Fire3"))
            {
                PlayerVitalsController.instance.DamagePlayerVital(5, "health");
            }

        } else
        {
            thePlayer.velocity = Vector2.zero;
        }

        // animations
        thePlayerAnimations.SetBool("isOnGround", isOnGround);
        thePlayerAnimations.SetFloat("speed", Mathf.Abs(thePlayer.velocity.x));
    }

    public void ShowAfterImage()
    {
        SpriteRenderer image = Instantiate(afterImage, transform.position, transform.rotation);
        image.sprite = theSR.sprite;
        image.transform.localScale = transform.localScale;
        image.color = afterImageColor;

        Destroy(image.gameObject, afterImageLifetime);

        afterImageCounter = timeBetweenAfterImages;
    }
}
