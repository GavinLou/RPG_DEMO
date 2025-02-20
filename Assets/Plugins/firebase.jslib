mergeInto(LibraryManager.library, {
  SignInWithEmail: function (emailPtr, passwordPtr) {
    var email = UTF8ToString(emailPtr);
    var password = UTF8ToString(passwordPtr);
    firebase.auth().signInWithEmailAndPassword(email, password)
      .then(function (userCredential) {
        var user = userCredential.user;
        console.log("loginsucess¡IUID:", user.uid);
        window.unityInstance.SendMessage('GameManager', 'OnFirebaseAuthSuccess', user.uid);
      })
      .catch(function (error) {
        console.error("loginerror:", error.message);
        window.unityInstance.SendMessage('GameManager', 'OnFirebaseAuthFailed', error.message);
      });
  },

  SaveDataToFirestore: function (userIdPtr, score) {
    var userId = UTF8ToString(userIdPtr);
    var scoreValue = score;

    db.collection("users").doc(userId).set({ score: scoreValue }, { merge: true })
      .then(() => {
        console.log("Savesucess¡I");
      })
      .catch((error) => {
        console.error("error:", error);
      });
  }
});
