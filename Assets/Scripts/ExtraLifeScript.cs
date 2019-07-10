using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ExtraLifeScript : MonoBehaviour
{
    #region physics_components
    Rigidbody2D heartRB;
    #endregion

    #region Unity_functions
    // Runs once on creation
    private void Awake()
    {
        heartRB = GetComponent<Rigidbody2D>();
    }

    // Runs every frame
    private void Update()
    {
       
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.transform.GetComponent<Player>().GainLife();
            Destroy(this.gameObject);
        }
    }
}
