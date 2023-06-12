mergeInto(LibraryManager.library, {
    //获取设备位置
    getLocation: function() {
        navigator.geolocation.getCurrentPosition(function(position) {
            var longitude = position.coords.longitude;
            var latitude = position.coords.latitude;
             // 将位置坐标传递给Unity
            var message = {
                "longitude": longitude,
                "latitude": latitude
            };
            var messageJSON = JSON.stringify(message);
            unityInstance.SendMessage('LocationServiceManager', 'SetPosition', messageJSON);
        });
    }
});