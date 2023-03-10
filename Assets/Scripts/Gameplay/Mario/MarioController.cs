using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioController : MonoBehaviour
{
    public float run;
    public float jump;
    public float maxSpeed;

    Transform _trans;
    Rigidbody2D _rb;

    float inputX;
    bool inputJump;

    bool isGrounded;

    public List<KeyCode> jumpKeys = new List<KeyCode>();

    // Start is called before the first frame update
    void Start()
    {
        _trans = GetComponent<Transform>();
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        inputX = Input.GetAxis("Horizontal");

        bool jumpHeld = false;
        for (int i = 0; i < jumpKeys.Count; i++)
        {
            if (Input.GetKey(jumpKeys[i]) ||
                Input.GetAxis("Vertical") > 0.3)
            {
                inputJump = true;
                jumpHeld = true;
            }
        }

        if (!jumpHeld)
        {
            inputJump = false;
        }

        if (inputX == 0 &&
            _rb.velocity.y == 0)
        {
            _rb.drag = 3;
        }
        else
        {
            _rb.drag = 1;
        }
    }

    private void FixedUpdate()
    {
        if (inputX != 0)
            Run();

        if (inputJump && isGrounded)
            Jump();
    }

    void Run()
    {
        if (Mathf.Abs(_rb.velocity.x) >= maxSpeed)
            return;

        float flipSprite = ((inputX / Mathf.Abs(inputX)) + 1) * 90; // 0 if going left, 180 if going right i think

        _rb.AddForce(Vector2.right * run * inputX, ForceMode2D.Force);
        _trans.rotation = Quaternion.Euler(0, flipSprite, 0);
    }

    void Jump()
    {
        _rb.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
        isGrounded = false;
    }

    void EnemyBounce()
    {
        _rb.AddForce(Vector2.up * jump / 1.5f, ForceMode2D.Impulse);
        isGrounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        for (int i = 0; i < collision.contacts.Length; i++)
        {
            if (collision.contacts[i].normal.y > 0.5) // normals
            {
                isGrounded = true;
            }
        }

        if (collision.gameObject.tag == "Goomba")
        {
            if (collision.contacts[0].normal.y > 0.5)
            {
                EnemyBounce();
                collision.gameObject.GetComponent<Goomba>().SetSquish(true);
            }
            else if (!collision.gameObject.GetComponent<Goomba>().GetSquish())
            {
                Debug.Log("mario died lol");
            }
        }

        if (collision.gameObject.tag == "Koopa") // reading
        {
            if (collision.contacts[0].normal.y > 0.5 &&
                !collision.gameObject.GetComponent<Koopa>().GetSquish())
            {
                EnemyBounce();
                collision.gameObject.GetComponent<Koopa>().SetSquish(true);
                collision.gameObject.GetComponent<Koopa>().SetMoving(false);

                collision.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1, 1); // shell hitbox
            }
            else if (collision.gameObject.GetComponent<Koopa>().GetSquish() &&
                !collision.gameObject.GetComponent<Koopa>().GetKicked())
            {
                if (collision.gameObject.transform.position.x > transform.position.x)
                {
                    collision.gameObject.GetComponent<Koopa>().KickKoopa(new Vector2(1, 0));
                }
                if (collision.gameObject.transform.position.x < transform.position.x)
                {
                    collision.gameObject.GetComponent<Koopa>().KickKoopa(new Vector2(-1, 0));
                }
            }
            else if (collision.contacts[0].normal.y > 0.5 && 
                collision.gameObject.GetComponent<Koopa>().GetKicked())
            {
                EnemyBounce();
                collision.gameObject.GetComponent<Koopa>().SetKicked(false);
                collision.gameObject.GetComponent<Koopa>().SetMoving(false);

                collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2 (0, collision.gameObject.GetComponent<Rigidbody2D>().velocity.y); // stops horizontal movement
            }
            else
            {
                if (collision.gameObject.GetComponent<Koopa>().GetMoving())
                {
                    Debug.Log("turtle mario");
                }
            }
        }

        if (collision.gameObject.tag == "Plant")
        {
            Debug.Log("mario died lol");
        }

        if (collision.gameObject.tag == "Fireball")
        {
            Debug.Log("mario died lol");
        }
    }
}
