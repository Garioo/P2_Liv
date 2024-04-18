/*
------------------------------
    PlayerVideoCollider.cs
Description: A script to trigger video playback when the player collides with the collider or enters the trigger zone
------------------------------
*/

using UnityEngine;

public class PlayerVideoCollider : MonoBehaviour
{

    [SerializeField]
    // Tag of the video to play
    private string videoTag;

    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Play the video with the specified tag
            VideoManager.instance.PlayVideo(videoTag);
        }
    }

    // Method to play the video when the player enters the collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player enters the collider
        if (other.CompareTag("Player"))
        {
            // Play the video with the specified tag
            VideoManager.instance.PlayVideo(videoTag);
        }
    }
}
