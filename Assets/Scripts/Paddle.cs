using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] Rigidbody2D playerRb;
    [SerializeField] float movementSpeed;

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePosition = new Vector2(Mathf.Clamp(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, -15, 15), 
            Mathf.Clamp(Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -7, 0));
        //transform.position = mousePosition;

        float step = movementSpeed * Time.deltaTime; // calculate distance to move
        playerRb.position = Vector3.MoveTowards(playerRb.position, mousePosition, step);
    }

    public void BallHit()
    {
        gameObject.SendMessage("BallHitRewardIncrement");
    }
}
