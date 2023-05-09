using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//If we want to sync anything in the level, they all follow the "conductor" i.e. syncing
public class Conductor : MonoBehaviour
{
    [SerializeField] public float bpm;
    public float crotchet;
    public float offset; //mp3 files have offset at beginning for metadata
    public float songposition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //songposition = (float)(AudioSettings.dspTime - dsptimesong) *song.pitch - offset;
    }
}
