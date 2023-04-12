using System.Collections.Generic;

namespace _game.Scripts.Dialogue
{
    //An object that plays a sentence, with a sound (optional), character (optional), name (optional)
//All are specified in a yaml file

    public class Dialogue
    {
        public string name { get; set; }
        public string sprite { get; set; }
        public Audio audio { get; set; }
        public List<string> sentences { get; set; }
    }

    public class Audio
    {
        public string file { get; set; }
        public float volume { get; set; }
    }
}