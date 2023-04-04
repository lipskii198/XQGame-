using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
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

    public SpriteRenderer spriteRenderer;
    public Sprite newSprite;
    GameObject spriteObject;
    // Start is called before the first frame update
    void Awake()
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

        //Render character sprite
        if (d.sprite != "" && File.Exists(Application.dataPath + "/Resources/" + d.sprite)) {
            Debug.Log(Application.dataPath + "/Resources/" + d.sprite);
            byte[] fileData = File.ReadAllBytes(Application.dataPath + "/Resources/" + d.sprite);
            if (fileData.Length > 0) {
                // Create a new GameObject
                spriteObject = new GameObject("SpriteObject");
                spriteRenderer = spriteObject.AddComponent<SpriteRenderer>();
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(fileData);

                Rect rect = new Rect(0, 0, texture.width, texture.height);
                Sprite sprite = Sprite.Create(texture, rect, Vector2.zero);
                spriteRenderer.sprite = sprite;
            } else {
                Debug.Log("couldnt get sprite from " + d.sprite);
            }
        }

        //Play audio
        if (d.audio.file != "")
        {
            // Load the AudioClip from a file
            audioClip = Resources.Load<AudioClip>(d.audio.file);

            // Play the audio clip
            if (audioClip != null) AudioSource.PlayClipAtPoint(audioClip, audioPosition, d.audio.volume);
        }


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
        if (spriteObject) Destroy(spriteObject);
        animator.SetBool("isOpen", false);
        //Remove character sprite
        Debug.Log("ending dialogue");
    }
}
