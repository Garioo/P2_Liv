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
    private DialogueNode[] dialogueNodes; // Array of dialogue nodes parsed from JSON
    private int currentNodeIndex; // Index of the current dialogue node
    public GameManager.GameState targetState; // Target game state to transition to
    GameManager gameManager; // Reference to the GameManager script
    private bool isLivVideoPlaying; // Flag to check if Liv's video is playing
    

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

 public void StartDialogue(int nextNodeIndex)
{
    // Set the current node index to the specified index
    currentNodeIndex = nextNodeIndex;

    // Retrieve the current node based on the updated index
    DialogueNode currentNode = dialogueNodes[currentNodeIndex];

    // Check if the current node has a video clip associated with Liv's friend
    if (!string.IsNullOrEmpty(currentNode.videoClipIdentifierFriend))
    {
        // Find the video clip with the given identifier
        int clipIndex = FindVideoClipIndex(currentNode.videoClipIdentifierFriend);
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
            Debug.LogError("Video clip with identifier " + currentNode.videoClipIdentifierFriend + " not found!");
        }
    }

    // Play the specified sound for Liv's friend
    if (!string.IsNullOrEmpty(currentNode.soundIdentifierFriend))
    {
        AudioManager.instance.PlayAudio(currentNode.soundIdentifierFriend);
    }

    // Check if the current node has a video clip associated with Liv
    if (!string.IsNullOrEmpty(currentNode.videoClipIdentifierLiv))
    {
        // Find the video clip with the given identifier
        int clipIndex = FindVideoClipIndex(currentNode.videoClipIdentifierLiv);
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
            Debug.LogError("Video clip with identifier " + currentNode.videoClipIdentifierLiv + " not found!");
        }
    }
    
    DisplayDialogue();
}
    
    void DisplayDialogue() // Display the dialogue text and choices
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
            choiceButtons[i].onClick.RemoveAllListeners();
            int choiceIndex = i;
            // Add a new listener for the choice button
            choiceButtons[i].onClick.AddListener(() => OnChoiceSelected(nextNodeIndex, currentNode.choices[choiceIndex].videoClipIdentifierLiv, currentNode.choices[choiceIndex].soundIdentifierLiv));
        }
    }

    // Handle player choice selection
  public void OnChoiceSelected(int nextNodeIndex, string videoClipIdentifierLiv, string soundIdentifierLiv)
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
        if (!string.IsNullOrEmpty(soundIdentifierLiv))
        {
            AudioManager.instance.PlayAudio(soundIdentifierLiv);
        }

        // Check if a video clip should be played
        if (!string.IsNullOrEmpty(videoClipIdentifierLiv))
        {
            // Find the video clip with the given identifier
            int clipIndex = FindVideoClipIndex(videoClipIdentifierLiv);
            // Check if the video clip was found
            if (clipIndex != -1)
            {
                // Assign the video clip to the VideoPlayer component
                videoPlayer.clip = DialogVideos[clipIndex];
                // Add a listener for the video end event
                videoPlayer.loopPointReached += (vp) => OnVideoEnd(videoPlayer);
                // Play the video clip
                videoPlayer.Play();
                // Set isLivVideoPlaying to true because Liv's video is playing
                isLivVideoPlaying = true;
            }
            else
            {
                Debug.LogError("Video clip with identifier " + videoClipIdentifierLiv + " not found!");
            }
        }
        else
        {
            // Start dialogue with the correct next node index
            StartDialogue(currentNodeIndex);
        }
    }
}


    // Helper method to find the index of the video clip with the given identifier
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

    // Handle the video end event   
   public void OnVideoEnd(VideoPlayer vp)
{
    Debug.Log("Video playback ended.");

    // Stop the video playback
    videoPlayer.clip = null;

    // If Liv's video was playing, increment the current node index and start playing the next node's dialogue
    if (isLivVideoPlaying)
    {
        // Start a timer for a specific duration
        StartCoroutine(StartTimer(0.00001f));

    }
    else
    {
        // If Liv's friend's video was playing, do nothing and maintain the current node index
    }
}
private IEnumerator StartTimer(float duration)
    {
        yield return new WaitForSeconds(duration);
        isLivVideoPlaying = false;
        
        // Start dialogue with the correct next node index
        StartDialogue(currentNodeIndex);
    }

    [System.Serializable]
    public class DialogueData
    {
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
    // Array of dialogue options (choices)
    public DialogueOption[] choices;
    // Identifier for Liv's video clip
    public string videoClipIdentifierLiv;
    // Identifier for Liv's sound
    public string soundIdentifierLiv;
    // Identifier for Liv's friend's video clip
    public string videoClipIdentifierFriend;
    // Identifier for Liv's friend's sound
    public string soundIdentifierFriend;
}

[System.Serializable]
// Define the structure of the dialogue option
public class DialogueOption
{
    // Choice text
    public string text;
    // Index of the next node
    public int nextNodeId;
    // Identifier for Liv's video clip
    public string videoClipIdentifierLiv;
    // Identifier for Liv's sound
    public string soundIdentifierLiv;
    // Identifier for Liv's friend's video clip
    public string videoClipIdentifierFriend;
    // Identifier for Liv's friend's sound
    public string soundIdentifierFriend;
}

}
