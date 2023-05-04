using UnityEngine;

namespace _game.Scripts.Dialogue
{
    public class Trigger : MonoBehaviour
    {
        // An instance of the ScriptableObject defined above.
        public DialogueTrigger trigger;
        public string dialaguePath;

        void Start()
        {
            trigger.dialoguePath = dialaguePath;
            trigger.TriggerDialogue();
        }

    }
}
