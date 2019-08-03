﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    UP,
    DOWN,
    LEFT,
    RIGHT,
    FORWARD,
    BACKWARD,
    BLANK,
}

public class Pusher : MonoBehaviour
{
    //The direction that this pusher imparts its  force in.
    private System.Diagnostics.Stopwatch timer;
    GameObject soundManager;
    public Direction direction;
    private bool waitingToDeploy;
    private const long deployTimer = 1500;
    private GameObject grabbedObject;
    private ParticleSystem Grab;
    AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        source = this.GetComponent<AudioSource>();
        Grab = GetComponentInChildren<ParticleSystem>();
        Grab.Stop();
        waitingToDeploy = false;
        timer = new System.Diagnostics.Stopwatch();
        transform.rotation = Quaternion.Euler(45,45,45);
        soundManager = GameObject.Find("SoundManager");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, Time.deltaTime * 50, 0, Space.World);
        if(waitingToDeploy)
        {
            if(timer.ElapsedMilliseconds > deployTimer)
            {
                //Deploy the payload!
                grabbedObject.GetComponent<Payload>().Release();
                waitingToDeploy = false;
                timer.Stop();
                timer.Reset();
            }
        }
    }

    public void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("Payload"))
        {
            if(source.isPlaying)
            {
                source.Stop();
            }

            source.Play();

            grabbedObject = col.gameObject;
            grabbedObject.GetComponent<Payload>().Grab(direction, this.transform);
            waitingToDeploy = true;
            timer.Start();
            Grab.Play();
        }
    }

    public void Rewind()
    {
        timer.Stop();
        timer.Reset();
        Grab.Stop();

    }

}
