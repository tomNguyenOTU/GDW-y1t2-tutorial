using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Goomba : MonoBehaviour
{
    [SerializeField] float deathTimer = 0.2f;

    bool squish;
    bool movingLeft;

    float flipTimer = 0;
    float speed = 1.5f;

    // Update is called once per frame
    void Update()
    {
        if (!squish)
        {
            Move();
        }
        else
        {
            Destroy(gameObject, deathTimer);
        }

        if (flipTimer <= Time.realtimeSinceStartup)
        {
            transform.Rotate(new Vector3(0, 1, 0), 180);
            flipTimer = Time.realtimeSinceStartup + 0.25f;
        }
    }

    void Move()
    {
        if (movingLeft)
        {
            transform.position += Vector3.left * Time.deltaTime * speed;
        }
        else
        {
            transform.position += Vector3.right * Time.deltaTime * speed;
        }
    }

    public bool GetSquish()
    {
        return squish;
    }

    public void SetSquish(bool squish)
    {
        this.squish = squish;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        movingLeft = !movingLeft;
    }
}
