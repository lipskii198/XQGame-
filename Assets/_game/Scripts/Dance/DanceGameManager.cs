using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceGameManager : _game.Scripts.Managers.GameManager
{
    [SerializeField] public AudioSource music;
    [SerializeField] public bool startPlaying;
    [SerializeField] public BeatScroller bs;

    public static DanceGameManager instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (!startPlaying) {
            if (Input.anyKeyDown) {
                startPlaying = true;
                bs.started = true;
                music.Play();
            }
        }
    }

    public void NoteHit() {
        
    }

    public void NoteMissed()
    {

    }
}
