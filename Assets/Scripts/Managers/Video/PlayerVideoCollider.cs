using UnityEngine;

public class PlayerVideoCollider : MonoBehaviour
{
    [SerializeField]
    private string videoTag; // Tag of the video to play

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            VideoManager.instance.PlayVideo(videoTag);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            VideoManager.instance.PlayVideo(videoTag);
        }
    }
}
