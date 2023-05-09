using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatScroller : MonoBehaviour
{
    [SerializeField] public float bpm;
    public float tempo;
    public float crotchet;
    public float offset; //mp3 files have offset at beginning for metadata
    public float songposition;

    public bool started;
    // Start is called before the first frame update
    void Start()
    {
        tempo = bpm/60f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!started) {
            if (/*something to start, probably triggered by some event*/true) {
                started = true;
            }
        } else {
            transform.position += new Vector3(0f, tempo*Time.deltaTime, 0f); //I'll need to change this later by using the songposition instead of deltaTime. Small inaccuracies will add up
        }
    }
}
