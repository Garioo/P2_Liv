using UnityEngine;

public class VibrationManager : MonoBehaviour
{
    public static void Vibrate(long milliseconds)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
            vibrator.Call("vibrate", milliseconds);
        }
        else
        {
            Debug.LogWarning("Vibration is only supported on Android devices.");
        }
    }
}
