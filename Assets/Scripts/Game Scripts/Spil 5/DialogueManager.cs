using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextAsset dialogueJson; // Reference to the JSON file containing dialogue data
    public TextMeshProUGUI dialogueText; // Reference to the UI text element for displaying dialogue
    public Button[] choiceButtons; // Array of UI buttons for displaying dialogue choices

    private DialogueNode[] dialogueNodes; // Array to store dialogue nodes
    private int currentNodeIndex; // Index to track the current dialogue node

    public GameManager.GameState targetState;
    GameManager gameManager; // Reference to the GameManager

    void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found or GameManager component missing!");
        }

        gameManager.SetGameManagerReference(this); // Update the argument type to DialogueManager

        // Load dialogue data from JSON file
        LoadDialogue();
        // Start dialogue from the first node
        StartDialogue(0);
    }

    void LoadDialogue()
    {
        // Load JSON data from "dialogue_tree.json" file
        dialogueJson = Resources.Load<TextAsset>("dialogue_tree");
        if (dialogueJson != null)
        {
            Debug.Log("Dialogue JSON loaded successfully.");
        }
        else
        {
            Debug.LogError("Failed to load dialogue JSON.");
        }

        // Deserialize JSON data into DialogueData object
        DialogueData dialogueData = JsonUtility.FromJson<DialogueData>(dialogueJson.text);
        // Get dialogue nodes from DialogueData object
        dialogueNodes = dialogueData.nodes;
    }

    void StartDialogue(int nodeIndex)
    {
        // Set current node index
        currentNodeIndex = nodeIndex;
        // Display dialogue
        DisplayDialogue();
    }

    void DisplayDialogue()
    {
        if (dialogueText == null)
        {
            Debug.LogError("dialogueText is null!");
            return;
        }

        // Get current dialogue node
        DialogueNode currentNode = dialogueNodes[currentNodeIndex];
        Debug.Log("Displaying dialogue node " + currentNode.id + ": " + currentNode.text);

        // Display dialogue text
        if (currentNode != null && currentNode.text != null)
        {
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

        // Display choice buttons for available choices
        for (int i = 0; i < currentNode.choices.Length && i < choiceButtons.Length; i++)
        {
            // Activate choice button
            choiceButtons[i].gameObject.SetActive(true);
            // Set choice button text
            choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentNode.choices[i].text;
            // Set up click event to handle choice
            int nextNodeIndex = currentNode.choices[i].nextNodeId;
            choiceButtons[i].onClick.RemoveAllListeners();
            choiceButtons[i].onClick.AddListener(() => OnChoiceSelected(nextNodeIndex));
        }
    }

    public void OnChoiceSelected(int nextNodeIndex)
{
    if (nextNodeIndex == -1)
    {
        // If nextNodeIndex is -1, start playing the video
        gameManager.EnterState(targetState);
    }
    else
    {
        Debug.Log("Player selected choice leading to node " + nextNodeIndex);
        // Progress the dialogue to the next node
        StartDialogue(nextNodeIndex);
    }
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
    }
}
