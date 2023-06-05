namespace WebGLHelper
{
    using UnityEngine;
    using System.Runtime.InteropServices;

    public class VibrationMgr : MonoBehaviour
    {
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void Vibrate();
#endif

        public static void VibrateDevice()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
        Vibrate();
#endif
        }
    }
}
