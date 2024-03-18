using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float enterTheHoleTime = 0.2f;
    Rigidbody2D ballRigibody;

    private Vector3 startBallPosition;
    private Vector3 startBallScale;

    private void Awake()
    {
        ballRigibody = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        startBallPosition = transform.position;
        startBallScale = transform.localScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.gameObject.tag == "Hole")
        {

            if(collision.transform.gameObject == GameControler.instance.GetCurrentHole())
            {
                GameControler.instance.HandleBallInHole(true);
            }
            else
            {
                GameControler.instance.HandleBallInHole(false);
            }
            ballRigibody.simulated = false;
            StartCoroutine(MoveToHoleCoroutine(collision.transform));
        }
    }
    IEnumerator  MoveToHoleCoroutine(Transform holeTransform)
    {
        float t = 0;
        Vector3 ballPosition = transform.position;

        while(t <= 1)
        {
            transform.position = Vector3.Lerp(ballPosition, holeTransform.position, t);
            transform.localScale = startBallPosition * Mathf.Lerp(1, 0.75f, t);

            t += Time.deltaTime / enterTheHoleTime;

            yield return new WaitForEndOfFrame();
        }

        transform.position = holeTransform.position;

        GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        GetComponent<CircleCollider2D>().enabled = false;
        ballRigibody.simulated = true;

        ballRigibody.gravityScale = 2.5f;
        ballRigibody.velocity = Vector2.zero;
        ballRigibody.angularVelocity = 0;
    }

    public void ResetBallPosition()
    {
        transform.position = startBallPosition;
        GetComponent<CircleCollider2D>().enabled = true;
        ballRigibody.velocity = Vector2.zero;
        ballRigibody.angularVelocity = 0;
        GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.None;
        transform.localScale = startBallScale;

        ballRigibody.gravityScale = 4.57f;
    }


    void Update()
    {
        
    }
}
