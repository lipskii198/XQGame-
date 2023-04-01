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

    void LateUpdate()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            DisplayNext();
        }
    }

    public void EnqueueDialogue(Dialogue d)
    {
        dialogues.Enqueue(d);
    }

    public void StartSequence()
    {
        sequenceStarted = true;

        DisplayNext();

    }

    public void DisplayNextDialogue()
    {   
        Dialogue d = dialogues.Dequeue();
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
        DisplayNext();


    }

    public void DisplayNext()
    {
        if (sentences.Count == 0 && dialogues.Count == 0)
        {
            EndDialogue();
        }
        else if (sentences.Count == 0 && dialogues.Count > 0)
        {
            DisplayNextDialogue();
        }
        else
        {
            DisplayNextSentence();
        }
    }


    //Called whenever the player hits the next button in the dialogue box
    public void DisplayNextSentence()
    {

        string sentence = sentences.Dequeue();
        Debug.Log(sentence);


    }
    //Probably need to do some garbage collection, will look into later
    void EndDialogue()
    {
        sequenceStarted = false;
        //Remove character sprite
        Debug.Log("ending dialogue");
    }
}
