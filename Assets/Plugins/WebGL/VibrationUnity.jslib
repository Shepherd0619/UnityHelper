mergeInto(LibraryManager.library, {
  Vibrate: function() {
    if ('vibrate' in navigator) {
      navigator.vibrate(200);
    } else if ('mozVibrate' in navigator) {
      navigator.mozVibrate(200);
    } else if ('webkitVibrate' in navigator) {
      navigator.webkitVibrate(200);
    }
  }
});