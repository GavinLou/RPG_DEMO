using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace CSIE
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private Canvas container;

        [SerializeField]
        private GameObject commonProcessPrefab;

        [SerializeField]
        private GameObject startupProcessPrefab;

        private bool autoSave;
        private bool gameInitialized;

        private CommonProcess commonProcess;
        public CommonProcess CommonProcess => commonProcess;

        private StartupProcess startupProcess;
        public StartupProcess StartupProcess => startupProcess;

        void Start()
        {
            var commonProcessObject = Instantiate(commonProcessPrefab, container.transform); //loading畫面
            commonProcess = commonProcessObject.GetComponent<CommonProcess>();

            StartCoroutine(InternalStart());
        }

        private IEnumerator InternalStart()
        {
            var startupProcessGameObject = Instantiate(startupProcessPrefab, container.transform);
            startupProcessGameObject.GameObject().transform.SetAsFirstSibling();
            startupProcess = startupProcessGameObject.GetComponent<StartupProcess>();

            float awaitTimestamp = Time.realtimeSinceStartup;
            yield return startupProcessGameObject;
            if (gameInitialized)
            {
                yield break;
            }
            float nowTimestamp = Time.realtimeSinceStartup;
            yield return new WaitForSeconds(3 - (nowTimestamp - awaitTimestamp) <= 0 ? 0 : 3 - (nowTimestamp - awaitTimestamp));
            commonProcess.SplashOut();

            LoginUser("test01@gmail.com", "test01");
        }

        [DllImport("__Internal")]
        private static extern void SignInWithEmail(string email, string password);

        [DllImport("__Internal")]
        private static extern void SaveDataToFirestore(string userId, int score);

        private string userId = "test01@gmail.com";

        // 這裡模擬登入，實際中可能來自用戶輸入
        public void LoginUser(string email, string password)
        {
            SignInWithEmail(email, password);
        }

        // 登入成功的回調函數
        public void OnFirebaseAuthSuccess(string uid)
        {
            Debug.Log("登入成功！UID：" + uid);
            userId = uid;
        }

        // 登入失敗的回調函數
        public void OnFirebaseAuthFailed(string errorMessage)
        {
            Debug.LogError("登入失敗：" + errorMessage);
        }

        // 儲存分數的函數
        public void SaveScore(int score)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                SaveDataToFirestore(userId, score);
            }
            else
            {
                Debug.LogError("請先登入！");

            }
        }
    }
}

