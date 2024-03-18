using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalBarController : MonoBehaviour
{
    public float movementSpeed = 1.75f;

    public Rigidbody2D leftLifter;
    public Rigidbody2D rightLifter;

    private Vector3 startPosition;

    private Vector2 movementOffsetLeft = new Vector2();
    private Vector2 movementOffsetRight = new Vector2();
    public float maxDifference = 1;

    private bool inputEnabled = false;

    public float startPositionVerticalOffset = 2.5f;

    public Joystick leftJoyStickController;
    public Joystick rightJoyStickController;




    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(inputEnabled == true)
        {
            movementOffsetLeft = Vector2.zero;
            movementOffsetRight = Vector2.zero;

            if (leftJoyStickController.Vertical != 0)
            {
                movementOffsetLeft += leftJoyStickController.Vertical * Time.fixedDeltaTime * movementSpeed * Vector2.up;
            }
            if (rightJoyStickController.Vertical != 0)
            {
                movementOffsetRight += rightJoyStickController.Vertical * Time.fixedDeltaTime * movementSpeed * Vector2.up;
            }

            if (Input.GetKey(KeyCode.Q))
            {
                movementOffsetLeft += Time.fixedDeltaTime * movementSpeed * Vector2.up;
            }
            if (Input.GetKey(KeyCode.E))
            {
                movementOffsetRight += Time.fixedDeltaTime * movementSpeed * Vector2.up;
            } 

            if (Input.GetKey(KeyCode.A))
            {
                movementOffsetLeft -= Time.fixedDeltaTime * movementSpeed * Vector2.up;
            }
            if (Input.GetKey(KeyCode.D))
            {
                movementOffsetRight -= Time.fixedDeltaTime * movementSpeed * Vector2.up;
            }

            if(Mathf.Abs(leftLifter.position.y -(rightLifter.position.y + movementOffsetRight.y)) <= maxDifference)
            {
                rightLifter.MovePosition(rightLifter.position + movementOffsetRight);
            }
            if(Mathf.Abs((leftLifter.position.y + movementOffsetLeft.y) - rightLifter.position.y) <= maxDifference)
            {
                leftLifter.MovePosition(leftLifter.position + movementOffsetLeft);
            }
        }
    }

    IEnumerator MoveBarToStartPosition()
    {
        while (leftLifter.position.y < startPosition.y + startPositionVerticalOffset)
        {
            leftLifter.MovePosition(leftLifter.position + Vector2.up * movementSpeed * Time.fixedDeltaTime);
            rightLifter.MovePosition(rightLifter.position + Vector2.up * movementSpeed * Time.fixedDeltaTime);

            yield return new WaitForEndOfFrame();
        }
        inputEnabled = true;

        GameControler.instance.ReadyForNextHole();
        yield return null;
    }

    IEnumerator MoveBarToBotomPosition()
    {
        while(leftLifter.position.y > startPosition.y || rightLifter.position.y > startPosition.y)
        {
            if(leftLifter.position.y > startPosition.y)
            {
                leftLifter.MovePosition(leftLifter.position - Vector2.up * movementSpeed * Time.fixedDeltaTime);
            }
            if (rightLifter.position.y > startPosition.y)
            {
                rightLifter.MovePosition(rightLifter.position - Vector2.up * movementSpeed * Time.fixedDeltaTime);
            }
            yield return new WaitForEndOfFrame();
        }

        GameControler.instance.ResetBall();

        if(GameControler.instance.gameCompletedState == false && GameControler.instance.gameOverState == false)
        {
            StartCoroutine(MoveBarToStartPosition());
        }

        yield return null;
    }

    [ContextMenu("Mover a la posición inicial")]

    public void MoveBarToStartPositionFunction()
    {
        StartCoroutine(MoveBarToStartPosition());
    }
    [ContextMenu("Mover a la posición del Boton")]

    public void MoveBarToBottomPositionFunction()
    {
        inputEnabled = false;
        StartCoroutine(MoveBarToBotomPosition());
    }

}


