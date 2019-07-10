using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour
{
    private Player player;
    private BoxCollider2D bc;
    public bool isMainBush = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        bc = gameObject.GetComponent<BoxCollider2D>();
        if (!isMainBush)
        {
            bc.enabled = false;
            //if (!isMainBush)
            //{
            //    transform.position += Vector3.up * 5.0f;
            //}

            Debug.Log("Starting!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMainBush && player.transform.position.y > transform.position.y)
        {
            bc.enabled = true;
        }
    }
}
