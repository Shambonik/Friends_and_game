using UnityEngine;

public class DeviceInfo
{

    public static string AnroidID()
    {
        string android_id = "";  
        
        #if UNITY_EDITOR
                android_id = "5025eab5d6ba4f68";
#elif UNITY_ANDROID
                AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject currentActivity = up.GetStatic<AndroidJavaObject>("currentActivity");
                AndroidJavaObject contentResolver = currentActivity.Call<AndroidJavaObject>("getContentResolver");
                AndroidJavaClass secure = new AndroidJavaClass("android.provider.Settings$Secure");
                android_id = secure.CallStatic<string>("getString", contentResolver, "android_id");
        #endif

        return android_id;
    }
}
