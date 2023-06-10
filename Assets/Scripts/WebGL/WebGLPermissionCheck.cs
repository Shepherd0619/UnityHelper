namespace WebGLHelper
{
    using UnityEngine;
    using System.Collections;
    using System.Runtime.InteropServices;
    using UnityEngine.Events;

    public class WebGLPermissionCheck : MonoBehaviour
    {
        public UnityEvent OnSuccessEvent;
        public UnityEvent OnErrorEvent;
#if UNITY_WEBGL
        [DllImport("__Internal")]
        private static extern void checkCameraAndGPSAuthorization(System.Action onSuccess, System.Action<string[]> onError);
#endif

        public void CheckPermission(){
            #if UNITY_WEBGL
            checkCameraAndGPSAuthorization(
                () =>
                {
                    // 成功获取授权
                    // 在这里编写代码，例如进入下一个场景：
                    // SceneManager.LoadScene(nextSceneName);
                },
                (errors) =>
                {
                    // 获取授权失败
                    // 在这里编写代码，例如显示错误提示：
                    Debug.LogError("[WebGLPermissionCheck]Check failed! Please refer to the following error logs.");
                    for (int i = 0; i < errors.Length; i++)
                    {
                        Debug.LogError(errors[i]);
                    }
                }
            );
#endif
        }
    }
}