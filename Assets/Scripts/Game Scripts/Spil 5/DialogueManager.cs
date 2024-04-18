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

public class DialogueManager : MonoBehaviour
{

    public TextAsset dialogueJson; // JSON file containing dialogue data
    public TextMeshProUGUI dialogueText; // Text component to display dialogue
    public Button[] choiceButtons; // Array of buttons for player choices
    public VideoPlayer videoPlayer; // VideoPlayer component to play video clips
    public VideoClip[] DialogVideos; // Array of video clips to play during dialogue
    private DialogueNode[] dialogueNodes; // Array of dialogue nodes parsed from JSON
    private int currentNodeIndex; // Index of the current dialogue node
    public GameManager.GameState targetState; // Target game state to transition to
    GameManager gameManager; // Reference to the GameManager script

    void Start() 
    {
        // Find the GameManager object in the scene and get its GameManager component
        gameManager = FindObjectOfType<GameManager>();

        if (gameManager == null)
        {
            Debug.LogError("GameManager object not found in the scene.");
        } 
        // Subscribe to the video end event
        videoPlayer.loopPointReached += OnVideoEnd;
        LoadDialogue(); 
        StartDialogue(0); 
    }

    // Load the dialogue data from the JSON file
    void LoadDialogue()
    {
        // Load the JSON file from the Resources folder
        dialogueJson = Resources.Load<TextAsset>("dialogue_tree");
        if (dialogueJson != null)
        {
            Debug.Log("Dialogue JSON loaded successfully.");
        }
        else
        {
            Debug.LogError("Failed to load dialogue JSON.");
        }

        // Parse the JSON data (Convert JSON to C# object)
        DialogueData dialogueData = JsonUtility.FromJson<DialogueData>(dialogueJson.text);
        // Assign the dialogue nodes to the array
        dialogueNodes = dialogueData.nodes;  
    }

    // Start the dialogue with the specified node index
    void StartDialogue(int nextNodeIndex)
    {
        // Set the current node index to the specified index
        currentNodeIndex = nextNodeIndex;
        DisplayDialogue();
    }
    
    void DisplayDialogue() // Display the dialogue text and choices
    {
        if (dialogueText == null)
        {
            Debug.LogError("dialogueText is null!");
            return;
        }

        // Get the current dialogue node
        DialogueNode currentNode = dialogueNodes[currentNodeIndex]; 
        Debug.Log("Displaying dialogue node " + currentNode.id + ": " + currentNode.text);

        // Check if the current node and text are not null
        if (currentNode != null && currentNode.text != null)
        {
            // Display the dialogue text
            dialogueText.text = currentNode.text;
        }
        else
        {
            Debug.LogError("Dialogue text is null!");
        }

        // Hide all choice buttons
        foreach (Button button in choiceButtons)
        {
            // Hide the choice button
            button.gameObject.SetActive(false);
        }

        for (int i = 0; i < currentNode.choices.Length && i < choiceButtons.Length; i++) // Display choice buttons
        {
            // Show the choice button
            choiceButtons[i].gameObject.SetActive(true); 
            // Set the choice text
            choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentNode.choices[i].text;
            // Get the index of the next node based on the player choice
            int nextNodeIndex = currentNode.choices[i].nextNodeId;
            // Remove any existing listeners from the button
            choiceButtons[i].onClick.RemoveAllListeners();
            // Store the current choice index
            int choiceIndex = i;
            // Add a new listener for the choice button
            choiceButtons[i].onClick.AddListener(() => OnChoiceSelected(nextNodeIndex, currentNode.choices[choiceIndex].videoClipIdentifier, currentNode.choices[choiceIndex].soundIdentifier));
        }
    }

    // Handle player choice selection
   public void OnChoiceSelected(int nextNodeIndex, string videoClipIdentifier, string soundIdentifier) 
{
    // Store the current node index before changing it
    int previousNodeIndex = currentNodeIndex;

    // Check if the next node index is -1 (special case)
    if (nextNodeIndex == -1) 
    {
        // Transition to the target game state
        gameManager.EnterState(targetState); 
        // Play the final cutscene audio
        AudioManager.instance.PlayAudio("event:/Cutscenes/Final Cutscene"); 
    }
    else
    {
        // Check if the next node index is valid
        if (nextNodeIndex >= 0 && nextNodeIndex < dialogueNodes.Length)
        {
            // Update the current node index to the next node
            currentNodeIndex = nextNodeIndex;
        }
        else
        {
            Debug.LogError("Invalid nextNodeIndex: " + nextNodeIndex);
            return;
        }

        // Retrieve the current node based on the updated index
        DialogueNode currentNode = dialogueNodes[currentNodeIndex];
        Debug.Log("Player selected choice leading to node " + currentNode.id);

        // Play sound if it exists for the current choice
        if (!string.IsNullOrEmpty(soundIdentifier))
        {
            AudioManager.instance.PlayAudio(soundIdentifier);
        }

        // Check if a video clip should be played
        if (!string.IsNullOrEmpty(videoClipIdentifier))
        {
            // Find the video clip with the given identifier
            int clipIndex = FindVideoClipIndex(videoClipIdentifier);
            // Check if the video clip was found
            if (clipIndex != -1)
            {
                
                // Assign the video clip to the VideoPlayer component
                videoPlayer.clip = DialogVideos[clipIndex]; 
                // Play the video clip
                videoPlayer.Play();
            }
            else
            {
                Debug.LogError("Video clip with identifier " + videoClipIdentifier + " not found!");
            }
        }
    }

    // Start dialogue with the correct next node index
    StartDialogue(currentNodeIndex);
}


    // Find the index of the video clip with the given identifier
    private int FindVideoClipIndex(string identifier) 
    {
        // Iterate through the video clips array
        for (int i = 0; i < DialogVideos.Length; i++)
        {
            // Check if the video clip name matches the identifier
            if (DialogVideos[i].name == identifier)
            {
                // Return the index if found
                return i;
            }
        }
        return -1; // Return -1 if not found
    }

    // Handle the video end event   
    void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("Video playback ended.");
        // Stop the video playback
        videoPlayer.clip = null;
    }

    [System.Serializable]
    // Define the structure of the JSON data
    public class DialogueData
    {
        // Array of dialogue nodes
        public DialogueNode[] nodes;
    }

    [System.Serializable]
    // Define the structure of the dialogue node
    public class DialogueNode
    {
        // Node ID
        public int id;
        // Dialogue text
        public string text;
        // Array of dialogue choices
        public DialogueOption[] choices;
    }

    [System.Serializable]
    // Define the structure of the dialogue option
    public class DialogueOption
    {
        // Choice text
        public string text;
        // Index of the next node
        public int nextNodeId;
        // Identifier for the video clip to play
        public string videoClipIdentifier;
        // Identifier for the sound to play
        public string soundIdentifier;
    }
}
