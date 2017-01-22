﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour {
    public float hackRange = 16;
    GameController gameController;
    // Use this for initialization

	void Start () {
        gameController = FindObjectOfType<GameController>();

    }
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        Physics.Linecast(Camera.main.transform.position, Camera.main.transform.position + 16 * (transform.position - Camera.main.transform.position ).normalized, out hit, 1<<11);
        Debug.DrawLine(Camera.main.transform.position, Camera.main.transform.position + 16 * (transform.position - Camera.main.transform.position).normalized);
        if (hit.collider && Input.GetButton("Interact")) {
            gameController.Hack(hit.collider.transform.parent.gameObject);
        }

    }
}