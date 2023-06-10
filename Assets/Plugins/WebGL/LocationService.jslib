mergeInto(LibraryManager.library, {
    getLocation: function() {
        navigator.geolocation.getCurrentPosition(function(position) {
            
            var unityPosition = new Unity.Vector3(position.coords.longitude, 0, position.coords.latitude);
            
            //unityInstance.SendMessage('LocationServiceManager', 'SetPosition', unityPosition);
            Unity.callStatic('LocationServiceManager', 'SetPosition', unityPosition);
        });
    }
});