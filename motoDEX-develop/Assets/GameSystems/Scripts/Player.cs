using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{

    public float speed = 0f;
    public Animator animator;

    Vector3 dir;

    public float xDir = 0f;

    public Vector3 clampedDir;

    public bool leftBoudary = false;

    public bool rightBoundary = false;

    public bool leftDisabled = false;

    public bool rightDisabled = false;

    public bool leftPressed = false;

    public bool rightPressed = false;

    public GameObject leftButton;

    public GameObject rightButton;

    public bool hasFallenDown = false;

    public ParticleSystem smoke;

    private string chibiMaterialsPath = "Materials";
    [SerializeField] private SkinnedMeshRenderer bodyRenderer;
    [SerializeField] private SkinnedMeshRenderer motoRenderer;

    // Start is called before the first frame update

    private void Awake()
    {
        setMaterials(SavedPlayer.savedPlayer);
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        smoke = smoke.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        if(GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>().gameStart)
        {
            if(!hasFallenDown)
            {
                
                animator.SetBool("start", false);

                if(GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>().gameFinished)
                {
                    transform.Translate(20f * dir * Time.deltaTime);
                    animator.SetBool("finish", true);
                    animator.SetBool("fallen", false);
                    animator.SetBool("turnLeft", false);
                    animator.SetBool("turnRight", false);
                }
                else
                {
                    animator.SetBool("fallen", false);
                    if(GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>().speeding)
                    {
                        // smoke.Play();
                        if(speed < 30f)
                        speed += 1;
                    }
                    else 
                    {
                        // smoke.Stop();
                        if(speed > 0f)
                        speed -= 1;
                    }

                    if(rightPressed)
                    {
                        if(transform.position.z > -18f)
                        {
                            xDir = 0.3f;
                        }
                        else
                        {
                            xDir = 0f;
                        }
                    }

                    if(leftPressed)
                    {
                        if(transform.position.z < 9f)
                        {
                            xDir = -0.3f;
                        }
                        else
                        {
                            xDir = 0f;
                        }
                    }
                
                    dir = new Vector3(xDir, 0f, 1f);
                    transform.Translate(speed * dir * Time.deltaTime);
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, 90f, transform.eulerAngles.z);
                }
            }
            else
            {
                animator.SetBool("fallen", true);
                animator.SetBool("turnLeft", false);
                animator.SetBool("turnRight", false);
                // smoke.Stop();
            }
        }
        else
        {
            animator.SetBool("start", true);
            // smoke.Stop();
        }
       
    }

    private void setMaterials(int matNumber)
    {
        if (matNumber == 0) { return; }

        var path = chibiMaterialsPath + "/racer_mat " + matNumber;
        Material mat = Resources.Load(path, typeof(Material)) as Material;
        bodyRenderer.material = mat;
        motoRenderer.material = mat;
    }

}
