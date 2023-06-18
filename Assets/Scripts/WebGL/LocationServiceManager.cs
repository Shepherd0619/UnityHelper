namespace WebGLHelper
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using Newtonsoft.Json.Linq;
    using UnityEngine;
    using UnityEngine.UI;

    public class LocationServiceManager : MonoBehaviour
    {
        public float xOrigin;  // GPS原点经度
        public float yOrigin; //GPS原点海拔
        public float zOrigin;  // GPS原点纬度
        public float scaleFactor = 100f;  // GPS坐标缩放比例

        public Vector3 CurrentGPSLocation;

        [DllImport("__Internal")]
        private static extern void getLocation();

        public static LocationServiceManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        private void Update() {
            getLocation();
        }

        public void SetPosition(string message)
        {
            // 解析 JSON 字符串
            JObject messageJSON = JObject.Parse(message);

            // 获取经纬度
            float longitude = (float)messageJSON["longitude"];
            float altitude = (float)messageJSON["altitude"];
            float latitude = (float)messageJSON["latitude"];
            // 将收到的GPS位置信息转换为Unity场景中的坐标
            var unityPosition = GPS2Unity(new Vector3(longitude, altitude, latitude));
            CurrentGPSLocation = new Vector3(longitude, altitude, latitude);
            // 将位置坐标应用到GameObject上
            Camera.main.transform.position = new Vector3(unityPosition.x, unityPosition.y, unityPosition.z);
        }

        // 将GPS位置信息转换为Unity场景中的坐标
        public Vector3 GPS2Unity(Vector3 position)
        {
            var x = (position.x - xOrigin) * scaleFactor;
            var y = (position.y - yOrigin) * scaleFactor;
            var z = (position.z - zOrigin) * scaleFactor;
            return new Vector3(x, y, z);
        }

        // 将Unity场景中的坐标转换为GPS坐标
        public Vector3 Unity2GPS(Vector3 position)
        {
            var x = position.x / scaleFactor + xOrigin;
            var y = position.y / scaleFactor + yOrigin;
            var z = position.z / scaleFactor + zOrigin;
            return new Vector3(x, y, z);
        }

        IEnumerator UpdateMainCameraPosition()
        {
            while (true)
            {
                getLocation();
                yield return new WaitForSecondsRealtime(1.0f);
            }
        }

        public void StartUpdateMainCameraPosition()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            StartCoroutine(UpdateMainCameraPosition());
#endif
        }

        public void StopUpdateMainCameraPosition()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            StopCoroutine(UpdateMainCameraPosition());
#endif
        }

        public void DisplayCurrentGPSLocationToUGUIText(Text txt)
        {
            txt.text = CurrentGPSLocation.ToString();
        }

        public void SetOriginGPSLocation(float x, float y, float z){
            xOrigin = x;
            yOrigin = y;
            zOrigin = z;
            Debug.Log("[LocationServiceManager]Origin GPS Location updated! "+xOrigin+", "+yOrigin+", "+zOrigin);
        }

        //<summary>
        //Vector3参数只读取其x和z值。
        //</summary>
        public void SetOriginGPSLocation(Vector3 pos){
            xOrigin = pos.x;
            yOrigin = pos.y;
            zOrigin = pos.z;
            Debug.Log("[LocationServiceManager]Origin GPS Location updated! "+xOrigin+", "+yOrigin + ", "+zOrigin);
        }
    }
}