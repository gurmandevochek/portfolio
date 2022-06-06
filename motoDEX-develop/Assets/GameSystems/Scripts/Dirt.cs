using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dirt : MonoBehaviour
{

    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Game Manager").GetComponent<GameManager>().player;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Wheel")
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().hasFallenDown = true;
            Destroy(GameObject.FindGameObjectWithTag("Player"), 3f);
            StartCoroutine("PlayerDead");
        }
    }

    IEnumerator PlayerDead()
    {
        yield return new WaitForSeconds(6f);
        Instantiate(player, transform.position+new Vector3(10f, 0f, 0f), player.transform.rotation);
    }   
}
