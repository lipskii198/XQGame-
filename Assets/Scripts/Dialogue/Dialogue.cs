using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//An object that plays a sentence, with a sound (optional), character (optional), name (optional)
//All are specified in a yaml file

public class Dialogue
{
    public string name { get; set; }
    public string sprite { get; set; }
    public string audio { get; set; }
    public List<string> sentences { get; set; }
}

