using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooController : MonoBehaviour
{

    public Transform[] patrolPoints;
    private int currentPoint;
    public bool isMovingGoo;

    public float moveSpeed, waitAtPoints;
    private float waitCounter;

    public Rigidbody2D theRB;

    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        currentPoint = 0;

        waitCounter = waitAtPoints;

        foreach(Transform pPoint in patrolPoints)
        {
            pPoint.SetParent(null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isMovingGoo)
        {        
            // far away from point
            if (Mathf.Abs(transform.position.x - patrolPoints[currentPoint].position.x) > .2f)
            {
                // to the left of the point
                if (transform.position.x < patrolPoints[currentPoint].position.x)
                {
                    theRB.velocity = new Vector2(moveSpeed, theRB.velocity.y);
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                }
                // to the left of the point
                else
                {
                    theRB.velocity = new Vector2(-moveSpeed, theRB.velocity.y);
                    transform.localScale = Vector3.one;
                }
            }
            // found the point
            else
            {
                theRB.velocity = new Vector2(0f, theRB.velocity.y);

                waitCounter -= Time.deltaTime;
                if (waitCounter <= 0)
                {
                    waitCounter = waitAtPoints;

                    currentPoint++;
                    if (currentPoint >= patrolPoints.Length)
                    {
                        currentPoint = 0;
                    }
                }
            }

            anim.SetFloat("speed", Mathf.Abs(theRB.velocity.x));
        }

    }
}
