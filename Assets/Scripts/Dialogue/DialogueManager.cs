using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{

    Queue<string> sentences;
    Queue<Dialogue> dialogues;
    AudioClip audioClip;
    Vector3 audioPosition;
    bool buttonPressed;
    bool sequenceStarted;

    public TextMeshPro nameText;
    public TextMeshPro speech;

    public Animator animator;

    [SerializeField] float timeBtwChars = 0.03f;

    bool typing = false;

    string currentSentence = "";
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
        nameText.text = d.name;
        animator.SetBool("isOpen", true);
        sentences.Clear();
        //Play audio
        if (d.audio.file != "")
        {
            // Load the AudioClip from a file
            audioClip = Resources.Load<AudioClip>(d.audio.file);

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
        if (typing)
        {
            DisplayNextSentence();
        }
        else if (sentences.Count == 0 && dialogues.Count == 0)
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

    IEnumerator TypeSentence()
    {
        speech.text = "";
        typing = true;
        foreach (char letter in currentSentence.ToCharArray())
        {
            speech.text += letter;
            yield return new WaitForSeconds(timeBtwChars); ;
        }
        typing = false;
    }

    //Called whenever the player hits the next button in the dialogue box
    public void DisplayNextSentence()
    {

        if (typing == true)
        {
            //finish sentence
            StopAllCoroutines();
            EndSentence();
            typing = false;
            Debug.Log("ended sentence early");
        }
        else
        {
            currentSentence = sentences.Dequeue();
            StartCoroutine(TypeSentence());
        }

    }

    public void EndSentence()
    {
        speech.text = currentSentence;
    }
    //Probably need to do some garbage collection, will look into later
    void EndDialogue()
    {
        sequenceStarted = false;
        animator.SetBool("isOpen", false);
        //Remove character sprite
        Debug.Log("ending dialogue");
    }
}
