mergeInto(LibraryManager.library, {
  RequestPointerStateChange: function (locked) {
    if (locked) {
      lockThePointer();
    } else {
      unlockThePointer();
    }
  },
});
