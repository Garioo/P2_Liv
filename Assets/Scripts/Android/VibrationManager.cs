/*
--------------------------  
    VibrationManager.cs
Description: A script to vibrate the device for the specified number of milliseconds
--------------------------  
    Literature:
    * Unity Tutorial - Phone Vibration:
        [How to make a phone vibrate!!](https://www.youtube.com/watch?v=o6xVLzs1kVk&ab_channel=Comp-3Interactive)
    * Unity Scripting API - Handheld.Vibrate: 
        [Handheld.Vibrate Documentation](https://docs.unity3d.com/ScriptReference/Handheld.Vibrate.html)
    * ChatGPT:
        [ChatGPT](https://openai.com/)
        
*/

using UnityEngine;

public class VibrationManager : MonoBehaviour
{
    // Vibrate the device for the specified number of milliseconds
    public static void Vibrate(long milliseconds)
    {
        // Check if the application is running on Android
        if (Application.platform == RuntimePlatform.Android)
        {
            // Get the UnityPlayer class
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            // Get the current activity
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            // Get the vibrator service
            AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
            // Vibrate the device
            vibrator.Call("vibrate", milliseconds);
        }
        // Check if the application is running on iOS
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            // Vibrate the device
            Handheld.Vibrate();
        }
        else
        {
            // Log a warning message
            Debug.LogWarning("Vibration not supported on this platform.");
        }
        
    }
}
