/* This script is responsible for managing the dialogue system in the game. 
It reads the dialogue data from a JSON file, displays the dialogue text, 
and handles player choices. It also plays video clips and audio based on the choices made by the player.

Litterature:
Link for buttons: 
    https://docs.unity3d.com/Manual/UIE-Click-Events.html
Links for parsing JSON data: 
    https://pavcreations.com/json-advanced-parsing-in-unity/#Parsing-JSON-Data
    https://forum.unity.com/threads/how-to-read-json-file.401306/

ChatGPT


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
            if (clipIndex != -1)
            {
                // Play the video clip
                // Assign the video clip to the VideoPlayer component
                videoPlayer.clip = DialogVideos[clipIndex]; 
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
            if (DialogVideos[i].name == identifier)
            {
                return i;
            }
        }
        return -1; // Return -1 if not found
    }

    // Handle the video end event   
    void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("Video playback ended.");
        videoPlayer.clip = null;
    }

    [System.Serializable]
    // Define the structure of the JSON data
    public class DialogueData
    {
        public DialogueNode[] nodes;
    }

    [System.Serializable]
    // Define the structure of the dialogue node
    public class DialogueNode
    {
        public int id;
        public string text;
        public DialogueOption[] choices;
    }

    [System.Serializable]
    // Define the structure of the dialogue option
    public class DialogueOption
    {
        public string text;
        public int nextNodeId;
        public string videoClipIdentifier;
        public string soundIdentifier;
    }
}
