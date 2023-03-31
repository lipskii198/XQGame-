using System.Collections;
using System.Collections.Generic;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using UnityEngine;
using System.IO;

public class DialogueTrigger : MonoBehaviour
{
    
    //Attach this trigger to whatever you want. A button, entering a level, etc
    //Specify the dialogue you want with a yaml file and that's it!
    public void TriggerDialogue() {
        string filePath = Application.dataPath + "/Cutscenes/sample-yaml.yaml";
        
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
