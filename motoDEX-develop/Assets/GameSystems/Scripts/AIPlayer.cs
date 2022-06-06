using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : MonoBehaviour
{

    public float speed = 0f;

    public float maxSpeed = 30f;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("RandomSpeed", 2f, 4f);
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
    }

    public void Movement()
    {
        if(GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>().gameStart)
        {
            animator.SetBool("start", false);
            if(speed < maxSpeed)
            {
                speed += 1;
            }
            else if(speed > maxSpeed)
            {       
                speed = maxSpeed;
            }
            Vector3 dir = new Vector3(0f, 0f, 1f);
            transform.Translate(dir * speed * Time.deltaTime);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 90f, transform.eulerAngles.z);
        }
        else
        {
            animator.SetBool("start", true);
        }
    }

    public void RandomSpeed()
    {
        int rand = Random.Range(20, 30);
        maxSpeed = (float)rand;
    }
}
