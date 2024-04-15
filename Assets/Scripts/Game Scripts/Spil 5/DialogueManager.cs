using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;

public class DialogueManager : MonoBehaviour
{
    public TextAsset dialogueJson; 
    public TextMeshProUGUI dialogueText; 
    public Button[] choiceButtons; 
    public VideoPlayer videoPlayer;
    public VideoClip[] DialogVideos; 
    private DialogueNode[] dialogueNodes; 
    private int currentNodeIndex; 

    public GameManager.GameState targetState;
    GameManager gameManager; 

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        if (gameManager == null)
        {
            Debug.LogError("GameManager object not found in the scene.");
        } 
        videoPlayer.loopPointReached += OnDialogVideoFinished;
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
            choiceButtons[i].onClick.RemoveAllListeners();
            int choiceIndex = i;
            choiceButtons[i].onClick.AddListener(() => OnChoiceSelected(nextNodeIndex, currentNode.choices[choiceIndex].videoClipIdentifier));
        }
    }

    public void OnChoiceSelected(int nextNodeIndex, string videoClipIdentifier)
    {
        if (nextNodeIndex == -1)
        {
            gameManager.EnterState(targetState);
            AudioManager.instance.StopAudio("event:/Cutscenes/Telefon Samtale");
            AudioManager.instance.PlayAudio("event:/Cutscenes/Sidste Cutscene");
        }
        else
        {
            Debug.Log("Player selected choice leading to node " + nextNodeIndex);
            if (!string.IsNullOrEmpty(videoClipIdentifier))
            {
                // Find the index of the video clip with the matching identifier
                int clipIndex = FindVideoClipIndex(videoClipIdentifier);
                if (clipIndex != -1)
                {
                    // Play the video clip
                    videoPlayer.clip = DialogVideos[clipIndex];
                    videoPlayer.Play();
                }
                else
                {
                    Debug.LogError("Video clip with identifier " + videoClipIdentifier + " not found!");
                }
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

    public void OnDialogVideoFinished(VideoPlayer vp)
    {
        videoPlayer.clip = null;
        NextDialogueNode(currentNodeIndex + 1); // Pass the nextNodeIndex argument
    }

    public void NextDialogueNode(int nextNodeIndex)
    {
        StartDialogue(nextNodeIndex);
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
        public string videoClipIdentifier; // Identifier for the associated video clip
    }
}
