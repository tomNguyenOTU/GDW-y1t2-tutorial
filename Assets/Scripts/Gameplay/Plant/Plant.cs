using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    [SerializeField] private GameObject firePrefab;
    [SerializeField] private float fireSpeed;
    [SerializeField] private float cooldown;
    [SerializeField] private float pipeOffset;

    Transform marioTrans;
    Renderer render;

    public Vector3 shooting;
    public Vector3 hidden;

    bool looping = true;
    bool visible;

    void Start()
    {
        marioTrans = GameObject.Find("Mario").GetComponent<Transform>();
        render = GetComponent<Renderer>();

        hidden = transform.position;
        shooting = transform.position + new Vector3(0, pipeOffset, 0);

        StartCoroutine(PlantLoop());

        Debug.Log(hidden);
        Debug.Log(shooting);
    }


    void Update()
    {
        if (visible != render.isVisible)
        {
            if (render.isVisible)
            {
                StartCoroutine(PlantLoop());
                Debug.Log("Starting coroutine");
            }
            else
            {
                StopCoroutine(PlantLoop());
                Debug.Log("Stopping coroutine");
            }
        }

        visible = render.isVisible;
    }

    void Shoot()
    {
        var bullet = (Instantiate(firePrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, 0))));
        bullet.GetComponent<Rigidbody2D>().velocity = (marioTrans.position - transform.position).normalized * fireSpeed;
        Destroy(bullet, 2);
    }
    
    IEnumerator LerpPosition(Vector3 pos)
    {
        float lerpTime = 0f;

        while (lerpTime < cooldown / 2)
        {
            float t = lerpTime / (cooldown / 2); // smooth step
            t = t * t * (3f - 2f * t);

            transform.position = Vector3.Lerp(transform.position, pos, t);
            lerpTime += Time.deltaTime;
            yield return null;
        }

        transform.position = pos;
    }

    IEnumerator PlantLoop()
    {
        while (looping)
        {
            StartCoroutine(LerpPosition(shooting));
            yield return new WaitForSeconds(cooldown);

            Shoot();
            yield return new WaitForSeconds(cooldown / 2);

            StartCoroutine(LerpPosition(hidden));
            yield return new WaitForSeconds(cooldown);
        }
    }
}
