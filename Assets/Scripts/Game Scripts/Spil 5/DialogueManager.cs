/* 
--------------------------  
    DialogueManager.cs
Description: Manages the dialogue system in the game, including reading dialogue data from a JSON file, 
displaying dialogue text, handling player choices, and playing video clips and audio based on player decisions.
--------------------------

Litterature:
    * Unity UI Button Click Events:
        [Unity UI Button Click Events Documentation](https://docs.unity3d.com/Manual/UIE-Click-Events.html)
    * Parsing JSON Data in Unity:
        [Parsing JSON Data in Unity Documentation](https://pavcreations.com/json-advanced-parsing-in-unity/#Parsing-JSON-Data)
    * Unity Forum Thread on Reading JSON File:
        [Reading JSON File in Unity Forum Thread](https://forum.unity.com/threads/how-to-read-json-file.401306/)
    * ChatGPT:
        [ChatGPT](https://openai.com/)
*/

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    // JSON file containing dialogue data
    public TextAsset dialogueJson;
    
    // Text component to display dialogue
    public TextMeshProUGUI dialogueText;
    
    // Array of buttons for player choices
    public Button[] choiceButtons;
    
    // VideoPlayer component to play video clips
    public VideoPlayer videoPlayer;
    
    // Array of video clips to play during dialogue
    public VideoClip[] DialogVideos;
    
    // AudioSource component to play sounds
    public AudioSource audioSource;

    // Array of dialogue nodes parsed from JSON
    private DialogueNode[] dialogueNodes;
    
    // Index of the current dialogue node
    private int currentNodeIndex;
    
    // Target game state to transition to
    public GameManager.GameState targetState;
    
    // Reference to the GameManager script
    GameManager gameManager;

    void Start()
    {
        // Find the GameManager script in the scene
        gameManager = FindObjectOfType<GameManager>();

        // If GameManager script is not found, log an error
        if (gameManager == null)
        {
            Debug.LogError("GameManager object not found in the scene.");
        }

        // Subscribe to the loopPointReached event of the VideoPlayer component
        videoPlayer.loopPointReached += OnVideoEnd;

        // Load the dialogue from the JSON file and start the dialogue at node 0
        LoadDialogue();
        StartDialogue(0);
    }

    void LoadDialogue()
    {
        // Load the dialogue JSON file from the Resources folder
        dialogueJson = Resources.Load<TextAsset>("dialogue_tree");

        // If the dialogue JSON file is loaded successfully, log a success message
        if (dialogueJson != null)
        {
            Debug.Log("Dialogue JSON loaded successfully.");
        }
        else
        {
            // If the dialogue JSON file fails to load, log an error
            Debug.LogError("Failed to load dialogue JSON.");
        }

        // Parse the dialogue data from the JSON file
        DialogueData dialogueData = JsonUtility.FromJson<DialogueData>(dialogueJson.text);
        dialogueNodes = dialogueData.nodes;
    }

    void StartDialogue(int nodeIndex)
    {
        // Set the current node index to the specified node index
        currentNodeIndex = nodeIndex;

        // Display the dialogue for the current node
        DisplayDialogue();
    }

    void DisplayDialogue()
    {
        // If the dialogueText component is null, log an error and return
        if (dialogueText == null)
        {
            Debug.LogError("dialogueText is null!");
            return;
        }

        // Get the current dialogue node
        DialogueNode currentNode = dialogueNodes[currentNodeIndex];

        // Log the current dialogue node ID and text
        Debug.Log("Displaying dialogue node " + currentNode.id + ": " + currentNode.text);

        // If the current dialogue node and its text are not null, display the text
        if (currentNode != null && currentNode.text != null)
        {
            dialogueText.text = currentNode.text;
        }
        else
        {
            // If the dialogue text is null, log an error
            Debug.LogError("Dialogue text is null!");
        }

        // Hide all choice buttons
        foreach (Button button in choiceButtons)
        {
            button.gameObject.SetActive(false);
        }

        // Display the choice buttons for the current node
        for (int i = 0; i < currentNode.choices.Length && i < choiceButtons.Length; i++)
        {
            // Show the choice button
            choiceButtons[i].gameObject.SetActive(true);

            // Set the text of the choice button
            choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentNode.choices[i].text;

            // Get the index of the next node, video clip identifier, and sound identifier for the choice
            int nextNodeIndex = currentNode.choices[i].nextNodeId;
            string videoClipIdentifier = currentNode.choices[i].videoClipIdentifier;
            string soundIdentifier = currentNode.choices[i].soundIdentifier;

            // Remove all listeners from the choice button and add a new listener with the selected choice parameters
            choiceButtons[i].onClick.RemoveAllListeners();
            choiceButtons[i].onClick.AddListener(() => OnChoiceSelected(nextNodeIndex, videoClipIdentifier, soundIdentifier));
        }
    }

    public void OnChoiceSelected(int nextNodeIndex, string videoClipIdentifier, string soundIdentifier)
    {
        // If the next node index is -1, transition to the target game state and play the final cutscene audio
        if (nextNodeIndex == -1)
        {
            gameManager.EnterState(targetState);
            AudioManager.instance.PlayAudio("event:/Final Cutscene");
        }
        else
        {
            // Log the selected choice and play the sound and video associated with the choice
            Debug.Log("Player selected choice leading to node " + nextNodeIndex);
            PlaySound(soundIdentifier);
            PlayVideo(videoClipIdentifier);
            StartDialogue(nextNodeIndex);
        }
    }

    void PlaySound(string soundIdentifier)
    {
        // If the sound identifier is not empty or null, play the audio clip
        if (!string.IsNullOrEmpty(soundIdentifier))
        {
            AudioManager.instance.PlayAudio(soundIdentifier);
        }
        else
        {
            // If the sound identifier is empty or null, log an error
            Debug.LogError("Failed to load audio clip: " + soundIdentifier);
        }
    }

    void PlayVideo(string videoClipIdentifier)
    {
        // Find the index of the video clip with the specified identifier
        int clipIndex = FindVideoClipIndex(videoClipIdentifier);

        // If the video clip index is found, set the VideoPlayer's clip and play it
        if (clipIndex != -1)
        {
            videoPlayer.clip = DialogVideos[clipIndex];
            videoPlayer.Play();
        }
        else
        {
            // If the video clip index is not found, log an error
            Debug.LogError("Video clip with identifier " + videoClipIdentifier + " not found!");
        }
    }

    private int FindVideoClipIndex(string identifier)
    {
        // Iterate through the DialogVideos array and find the index of the video clip with the specified identifier
        for (int i = 0; i < DialogVideos.Length; i++)
        {
            if (DialogVideos[i].name == identifier)
            {
                return i;
            }
        }

        // Return -1 if the video clip index is not found
        return -1;
    }

    public void OnVideoEnd(VideoPlayer vp)
    {
        // Log a message when the video playback ends and clear the VideoPlayer's clip
        Debug.Log("Video playback ended.");
        videoPlayer.clip = null;
    }

    // Serializable class for storing dialogue data
    [System.Serializable]
    public class DialogueData
    {
        public DialogueNode[] nodes;
    }

    // Serializable class for storing dialogue node data
    [System.Serializable]
    public class DialogueNode
    {
        public int id;
        public string text;
        public DialogueOption[] choices;
    }

    // Serializable class for storing dialogue option data
    [System.Serializable]
    public class DialogueOption
    {
        public string text;
        public int nextNodeId;
        public string videoClipIdentifier;
        public string soundIdentifier;
    }
}
