namespace WebGLHelper
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class LocationServiceManager : MonoBehaviour
    {
        public float xOrigin;  // GPS原点经度
        public float zOrigin;  // GPS原点纬度
        public float scaleFactor = 100f;  // GPS坐标缩放比例

        [DllImport("__Internal")]
        private static extern void getLocation();

        public void SetPosition(Vector3 position)
        {
            // 将收到的GPS位置信息转换为Unity场景中的坐标
            var unityPosition = GPS2Unity(position);

            // 将位置坐标应用到GameObject上
            transform.position = unityPosition;
        }

        // 将GPS位置信息转换为Unity场景中的坐标
        private Vector3 GPS2Unity(Vector2 position)
        {
            var x = (position.x - xOrigin) * scaleFactor;
            var z = (position.y - zOrigin) * scaleFactor;
            return new Vector3(x, 0, z);
        }

    }
}