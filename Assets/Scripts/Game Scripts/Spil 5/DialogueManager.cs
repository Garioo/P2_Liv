using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public TextAsset dialogueJson; // JSON file containing dialogue data
    public TextMeshProUGUI dialogueText; // Text component to display dialogue
    public Button[] choiceButtons; // Array of buttons for player choices
    public VideoPlayer videoPlayer; // VideoPlayer component to play video clips
    public VideoClip[] DialogVideos; // Array of video clips to play during dialogue
    public AudioSource audioSource; // AudioSource component to play sounds

    private DialogueNode[] dialogueNodes; // Array of dialogue nodes parsed from JSON
    private int currentNodeIndex; // Index of the current dialogue node
    public GameManager.GameState targetState; // Target game state to transition to
    GameManager gameManager; // Reference to the GameManager script

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        if (gameManager == null)
        {
            Debug.LogError("GameManager object not found in the scene.");
        }

        videoPlayer.loopPointReached += OnVideoEnd;
        LoadDialogue();
        StartDialogue(0);
    }

    void LoadDialogue()
    {
        dialogueJson = Resources.Load<TextAsset>("dialogue_tree");
        if (dialogueJson != null)
        {
            Debug.Log("Dialogue JSON loaded successfully.");
        }
        else
        {
            Debug.LogError("Failed to load dialogue JSON.");
        }

        DialogueData dialogueData = JsonUtility.FromJson<DialogueData>(dialogueJson.text);
        dialogueNodes = dialogueData.nodes;
    }

    void StartDialogue(int nodeIndex)
    {
        currentNodeIndex = nodeIndex;
        DisplayDialogue();
    }

    void DisplayDialogue()
    {
        if (dialogueText == null)
        {
            Debug.LogError("dialogueText is null!");
            return;
        }

        DialogueNode currentNode = dialogueNodes[currentNodeIndex];
        Debug.Log("Displaying dialogue node " + currentNode.id + ": " + currentNode.text);

        if (currentNode != null && currentNode.text != null)
        {
            dialogueText.text = currentNode.text;
        }
        else
        {
            Debug.LogError("Dialogue text is null!");
        }

        foreach (Button button in choiceButtons)
        {
            button.gameObject.SetActive(false);
        }

        for (int i = 0; i < currentNode.choices.Length && i < choiceButtons.Length; i++)
        {
            choiceButtons[i].gameObject.SetActive(true);
            choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentNode.choices[i].text;
            int nextNodeIndex = currentNode.choices[i].nextNodeId;
            string videoClipIdentifier = currentNode.choices[i].videoClipIdentifier;
            string soundIdentifier = currentNode.choices[i].soundIdentifier;
            choiceButtons[i].onClick.RemoveAllListeners();
            choiceButtons[i].onClick.AddListener(() => OnChoiceSelected(nextNodeIndex, videoClipIdentifier, soundIdentifier));
        }
    }

    public void OnChoiceSelected(int nextNodeIndex, string videoClipIdentifier, string soundIdentifier)
    {
        if (nextNodeIndex == -1)
        {
            gameManager.EnterState(targetState);
            AudioManager.instance.PlayAudio("event:/Final Cutscene");
        }
        else
        {
            Debug.Log("Player selected choice leading to node " + nextNodeIndex);
            PlaySound(soundIdentifier);
            PlayVideo(videoClipIdentifier);
            StartDialogue(nextNodeIndex);
        }
    }

    void PlaySound(string soundIdentifier)
    {
         if (!string.IsNullOrEmpty(soundIdentifier))
        {
            AudioManager.instance.PlayAudio(soundIdentifier);
        }
        else
        {
            Debug.LogError("Failed to load audio clip: " + soundIdentifier);
        }
    }

    void PlayVideo(string videoClipIdentifier)
    {
        int clipIndex = FindVideoClipIndex(videoClipIdentifier);
        if (clipIndex != -1)
        {
            videoPlayer.clip = DialogVideos[clipIndex];
            videoPlayer.Play();
        }
        else
        {
            Debug.LogError("Video clip with identifier " + videoClipIdentifier + " not found!");
        }
    }

    private int FindVideoClipIndex(string identifier)
    {
        for (int i = 0; i < DialogVideos.Length; i++)
        {
            if (DialogVideos[i].name == identifier)
            {
                return i;
            }
        }
        return -1; // Return -1 if not found
    }

    public void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("Video playback ended.");
        videoPlayer.clip = null;
    }

    [System.Serializable]
    public class DialogueData
    {
        public DialogueNode[] nodes;
    }

    [System.Serializable]
    public class DialogueNode
    {
        public int id;
        public string text;
        public DialogueOption[] choices;
    }

    [System.Serializable]
    public class DialogueOption
    {
        public string text;
        public int nextNodeId;
        public string videoClipIdentifier;
        public string soundIdentifier;
    }
}
