using CSIE;
using FirebaseWebGL.Examples.Utils;
using FirebaseWebGL.Scripts.FirebaseBridge;
using FirebaseWebGL.Scripts.Objects;
using Unity.VisualScripting;
using System.Collections;
using UnityEngine;
using TMPro;
using System.IO;
using System;
using UnityEngine.UI;

public class StartupProcess : MonoBehaviour
{
    [SerializeField] private TMP_InputField _accountInputField;

    [SerializeField] private TMP_InputField _passwordInputField;

    [SerializeField] private Button _button;
    //public IEnumerator TryAutoLogin()
    //{
    //    var tokenPath = Path.Combine(Application.persistentDataPath, FILE_TOKEN_SAVE);
    //    if (!File.Exists(tokenPath))
    //    {
    //        yield break;
    //    }

    //    var credentialText = File.ReadAllText(tokenPath);
    //    UserCredential credential = null;

    //    try
    //    {
    //        credential = JsonConvert.DeserializeObject<UserCredential>(credentialText);
    //    }
    //    catch (Exception exception)
    //    {
    //        Debug.Log($"Unable to deserialized credential: {exception}");
    //        Task.Run(
    //            () => File.Delete(tokenPath)
    //        );
    //        _prompt.text = "自動登入失敗";
    //        yield break;
    //    }

    //    var loginTask = FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(
    //        credential.Email,
    //        credential.Password
    //    );

    //    yield return new WaitUntil(() => loginTask.IsCompleted);
    //    if (loginTask.IsCompletedSuccessfully)
    //    {
    //        GameManager.Instance.InitializeGame();
    //        yield break;
    //    }

    //    Debug.Log($"Auto login failed! {loginTask.Exception}");
    //    Task.Run(
    //        () => File.Delete(tokenPath)
    //    );
    //    _prompt.text = "自動登入失敗";
    //}


    public void OnLoginButtonClicked()
    {
        Debug.Log("123");
        if (_accountInputField.text.Length == 0)
        {
           // _prompt.text = "請輸入 Email 帳號！";
            return;
        }

        if (_passwordInputField.text.Length == 0)
        {
            //_prompt.text = "請輸入密碼！";
            return;
        }

        _button.interactable = false;
        StartCoroutine(LoginCoroutine(_accountInputField.text, _passwordInputField.text));
    }

    public IEnumerator LoginCoroutine(string email, string password)
    {
        FirebaseAuth.SignInWithEmailAndPassword( email, password , "GameManager", "DisplayInfo", "DisplayError");
        yield return new WaitForSeconds(0.25f);
        FirebaseAuth.OnAuthStateChanged("GameManager", "DisplayUser", "DisplayError");
        yield return new WaitForSeconds(0.25f);
        //if (authenticationTask.IsFaulted)
        //{
        //    if (authenticationTask.Exception != null && authenticationTask.Exception.GetBaseException() is FirebaseException)
        //    {
        //        FirebaseException exception = (FirebaseException)authenticationTask.Exception.GetBaseException();
        //        _prompt.text = $"無法登入伺服器({exception.ErrorCode}){Environment.NewLine}{FirebaseAuthHook.GetTranslatedError(exception)}";
        //        Debug.Log($"An error occurred while trying to login ----- {Environment.NewLine} {authenticationTask.Exception}");
        //    }
        //    else
        //    {
        //        _prompt.text = "無法登入伺服器";
        //        Debug.Log("An unknown error occurred while trying to login");
        //    }

        //    _button.interactable = true;
        //    yield break;
        //}
        //else if (authenticationTask.IsCanceled)
        //{
        //    _prompt.text = "";
        //    _button.interactable = true;
        //    yield break;
        //}

        //// if logged in successfully
        //var user = authenticationTask.Result;
        //Debug.Log($"Logged in successfully! {{uid={user.UserId}}}");

        //var credential = new UserCredential(email, password);
        //var serializedCredential = JsonConvert.SerializeObject(credential);
        //var credentialPath = Path.Combine(Application.persistentDataPath, FILE_TOKEN_SAVE);
        //var saveCredentialTask = Task.Run(
        //    () => File.WriteAllText(
        //        credentialPath,
        //        serializedCredential
        //        )
        //);

        // yield return new WaitUntil(() => saveCredentialTask.IsCompleted);

        //if (!saveCredentialTask.IsCompletedSuccessfully)
        //{
        //    GameManager.Instance.PromptManager.CreateToast(
        //        5.0f,
        //        ToastLevel.WARNING,
        //        "自動登入可能無法在這個裝置上使用"
        //        );
        //    Debug.Log($"Failed to save credential {saveCredentialTask.Exception}");
        //}
        //else
        //{
        //    GameManager.Instance.PromptManager.CreateToast(
        //        5.0f,
        //        ToastLevel.DONE,
        //        "已啟用自動登入"
        //    );
        //}
        _button.interactable = true;
        yield return null;
    }
}
