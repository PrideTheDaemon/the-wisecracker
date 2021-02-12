﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerStats stats;
    private CharacterController controller;

    private GameObject projectilesContainer;
    private Animator animator;

    private Vector3 position = new Vector3();

    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<PlayerStats>();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        projectilesContainer = GameObject.FindGameObjectWithTag("ProjectilesContainer");
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        RotatePlayer();

        if (Input.GetButton("Fire1") && stats.CanShoot)
            Fire();
    }

    private void Fire()
    {
        GameObject grenade = Instantiate(stats.grenadePrefab,
            transform.position, transform.rotation,
            projectilesContainer.transform);
        grenade.SetActive(true);

        stats.lastShot = 0;
    }

    private void RotatePlayer()
    {
        var mousePosition = Utils.MousePosition();
        mousePosition.y = transform.position.y;
        transform.LookAt(mousePosition);
    }

    private void MovePlayer()
    {
        stats.moving = stats.canMove && 0 <
            Mathf.Abs(Input.GetAxis("Vertical")) +
            Mathf.Abs(Input.GetAxis("Horizontal"));

        if (stats.moving) animator.Play("Moving");
        else animator.Play("Idle");

        position = (transform.TransformDirection(Vector3.forward)
            * stats.positionSpeed
            * Input.GetAxis("Vertical"))
            + (transform.TransformDirection(Vector3.right)
            * stats.positionSpeed
            * Input.GetAxis("Horizontal"));

        if (!controller.isGrounded) position.y -= stats.gravity;
        position *= Time.deltaTime;

        controller.Move(position);
    }
}
