var LocationService = {
    //获取设备位置
    getLocation: function() {
        navigator.geolocation.getCurrentPosition(function(position) {
            // 定义Unity场景中的位置坐标
            var unityPosition = new Unity.Vector3(position.coords.longitude, 0, position.coords.latitude);
            // 将位置坐标传递给Unity
            //unityInstance.SendMessage('LocationServiceManager', 'SetPosition', unityPosition);
            Unity.callStatic('LocationServiceManager', 'SetPosition', unityPosition);
        });
    }
};