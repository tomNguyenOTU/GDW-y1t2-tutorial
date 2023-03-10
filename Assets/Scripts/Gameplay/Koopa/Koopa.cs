using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Koopa : MonoBehaviour
{

    [SerializeField] float kickForce = 2.5f;
    [SerializeField] float speed = 1.0f;

    bool squish;
    bool kicked;
    bool moving = true;
    bool movingLeft;

    // Update is called once per frame
    void Update()
    {
        if (!squish)
        {
            Move();
        }
        else
        {
            
        }
    }

    void Move()
    {
        if (movingLeft)
        {
            transform.position += Vector3.left * Time.deltaTime * speed;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.position += Vector3.right * Time.deltaTime * speed;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    public void KickKoopa(Vector2 direction)
    {
        GetComponent<Rigidbody2D>().AddForce(direction * kickForce, ForceMode2D.Impulse);
        kicked = true;
        moving = true;
    }

    public bool GetSquish()
    {
        return squish;
    }

    public void SetSquish(bool squish)
    {
        this.squish = squish;
    }    

    public bool GetKicked()
    {
        return kicked;
    }

    public void SetKicked(bool kicked)
    {
        this.kicked = kicked;
    }

    public bool GetMoving()
    {
        return moving;
    }

    public void SetMoving(bool moving)
    {
        this.moving = moving;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!squish)
        {
            movingLeft = !movingLeft;
        }

        if (!kicked)
        {
            if (collision.contacts[0].normal.x > 0)
            {
                KickKoopa(new Vector2(1, 0));
            }
            if (collision.contacts[0].normal.x < 0)
            {
                KickKoopa(new Vector2(-1, 0));
            }
        }

        if (collision.gameObject.tag == "Goomba")
        {
            collision.gameObject.GetComponent<Rigidbody2D>().gravityScale = 3;
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 8, ForceMode2D.Impulse);
            collision.gameObject.GetComponent<Collider2D>().enabled = false; // fall through the ground
            Destroy(collision.gameObject, 2);

            KickKoopa(new Vector2(-collision.contacts[0].normal.normalized.x, 0));
        }
    }
}
