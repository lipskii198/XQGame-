using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{

    Queue<string> sentences;
    Queue<Dialogue> dialogues;
    // Start is called before the first frame update
    void Start()
    {   
        dialogues = new Queue<Dialogue>();
        sentences = new Queue<string>();
    }

    public void EnqueueDialogue(Dialogue d) {
        dialogues.Enqueue(d);
    }

    public void StartSequence() {
        while (dialogues.Count > 0) {
            Dialogue d = dialogues.Dequeue();
            StartDialogue(d);
        }
    }

    public void StartDialogue(Dialogue d) {
        Debug.Log("starting dialogue" + d.name);

        sentences.Clear();

        foreach (string sentence in d.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    //Called whenever the player hits the next button in the dialogue box
    public void DisplayNextSentence() {
        if (sentences.Count == 0) {
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        Debug.Log(sentence);
    }
    //Probably need to do some garbage collection, will look into later
    void EndDialogue() {
        Debug.Log("ending dialogue");
    }
}
