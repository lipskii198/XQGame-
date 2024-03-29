using System.Collections.Generic;
using System.IO;
using UnityEngine;
using YamlDotNet.Serialization;

namespace _game.Scripts.Dialogue
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/DialogueTrigger", order = 1)]
    public class DialogueTrigger : ScriptableObject
    {
        public string dialoguePath = "";

        void Start()
        {
            TriggerDialogue();
        }
        //Attach this trigger to whatever you want. A button, entering a level, etc
        //Specify the dialogue you want with a yaml file and that's it!
        public void TriggerDialogue() {
            string filePath = Application.dataPath + "/" + dialoguePath;
        
            // Read the YAML file using a StreamReader
            StreamReader reader = new StreamReader(filePath);
            var deserializer = new DeserializerBuilder().Build();
            var result = deserializer.Deserialize<List<Dictionary<string, Dialogue>>>(reader);
            DialogueManager manager = FindObjectOfType<DialogueManager>();
            // Access the deserialized data
            foreach (var item in result) {
                foreach (var dialogue in item) {
                    //Push every dialogue item into the dialogue manager to be rendered
                
                    manager.EnqueueDialogue(dialogue.Value);
                }
            }
            manager.StartSequence();
        }
    }
}
