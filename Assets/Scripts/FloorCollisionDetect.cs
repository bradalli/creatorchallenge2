using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FloorCollisionDetect : MonoBehaviour
{
    [SerializeField] UnityEvent collisionEnter2DEvent;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            collisionEnter2DEvent.Invoke();
            transform.position = Vector3.up * 5;
            gameObject.SetActive(false);
        }
    }
}
