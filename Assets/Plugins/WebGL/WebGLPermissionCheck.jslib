mergeInto(LibraryManager.library, {
    checkCameraAndGPSAuthorization: function(onSuccess, onError) {
        var hasCameraAccess = false;
        var hasGPSAccess = false;

        // 检查是否支持getUserMedia
        if (navigator.mediaDevices && navigator.mediaDevices.getUserMedia) {
            // 请求摄像头和麦克风访问权限
            navigator.mediaDevices.getUserMedia({ 
                audio: true, 
                video: true 
            })
            .then(function(stream) {
                // 成功获取访问权限
                hasCameraAccess = true;
                checkAuthorizationComplete();
            })
            .catch(function(error) {
                // 用户拒绝或浏览器不支持
                onError(["Failed to get camera access. Error: " + error]);
            });
        } else {
            onError(["Your browser doesn't support camera access."]);
        }

        // 检查是否支持Geolocation API
        if (navigator.geolocation) {
            // 请求地理位置访问权限
            navigator.geolocation.getCurrentPosition(function(position) {
                // 成功获取访问权限hasGPSAccess = true;
                checkAuthorizationComplete();
            }, function(error) {
                // 用户拒绝或浏览器不支持
                onError(["Failed to get GPS access. Error: " + error.message]);
            });
        } else {
            onError(["Your browser doesn't support GPS access."]);
        }

        function checkAuthorizationComplete() {
            if (hasCameraAccess && hasGPSAccess) {
                onSuccess();
            }
        }
    }
});