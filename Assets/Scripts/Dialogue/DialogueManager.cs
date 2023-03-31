using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{

    Queue<string> sentences;
    Queue<Dialogue> dialogues;
    AudioClip audioClip;
    Vector3 audioPosition;
    bool buttonPressed;
    bool sequenceStarted;
    // Start is called before the first frame update
    void Start()
    {
        dialogues = new Queue<Dialogue>();
        sentences = new Queue<string>();
        audioPosition = new Vector3(0, 0, 0);
        sequenceStarted = false;

    }
    void Update()
    {
      
            // Pause the loop and wait for a button to be pressed
            if (Input.GetKeyDown(KeyCode.Space) && sequenceStarted)
            {
                DisplayNextSentence();
            }
        
    }

    public void EnqueueDialogue(Dialogue d)
    {
        dialogues.Enqueue(d);
    }

    public void StartSequence()
    {   
        sequenceStarted = true;
        while (dialogues.Count > 0)
        {
            Dialogue d = dialogues.Dequeue();
            StartDialogue(d);
        }
    }

    public void StartDialogue(Dialogue d)
    {
        Debug.Log("starting dialogue " + d.name);

        sentences.Clear();
        //Play audio
        if (d.audio.file != "")
        {
            // Load the AudioClip from a file
            audioClip = Resources.Load<AudioClip>(d.audio.file);
            Debug.Log(d.audio);
            // Play the audio clip
            if (audioClip != null) AudioSource.PlayClipAtPoint(audioClip, audioPosition, d.audio.volume);
        }
        //Render character sprite
        //Render dialogue box

        foreach (string sentence in d.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
        
        
    }

    //Called whenever the player hits the next button in the dialogue box
    public void DisplayNextSentence()
    {   
        if (sentences.Count != 0) {
            string sentence = sentences.Dequeue();
            Debug.Log(sentence);
        } else {
            EndDialogue();
            return;
        }
        
    }
    //Probably need to do some garbage collection, will look into later
    void EndDialogue()
    {
        sequenceStarted = false;
        //Remove character sprite
        Debug.Log("ending dialogue");
    }
}
