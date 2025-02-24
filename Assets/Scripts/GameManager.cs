using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;
using FirebaseWebGL.Scripts.FirebaseBridge;
using FirebaseWebGL.Scripts.Objects;


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


            FirebaseAuth.SignInWithEmailAndPassword("test01@gmail.com", "test01", "GameObject", "DisplayInfo", "DisplayErrorObject");

        }
        public void DisplayInfo(string info)
        {
            Debug.Log(info);
        }
        public void DisplayError(string error)
        {
            Debug.LogError(error);
        }
    }
}

