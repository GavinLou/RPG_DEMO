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
using FirebaseWebGL.Examples.Utils;
using UnityEngine.UI;
using static Userdata;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System;
using UnityEngine.UIElements;
using Models;


namespace CSIE
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Canvas container;
        [SerializeField] private GameObject commonProcessPrefab;
        [SerializeField] private GameObject startupProcessPrefab;
        [SerializeField] private GameObject GameProcessPrefab;

        private static GameManager _instance;
        public static GameManager Instance
        {
            get => _instance;
        }

        private UserData _userData;
        public UserData UserData
        {
            get => _userData;
        }
        //private ReadOnlyDictionary<string, Branch> _branches;
        //private ReadOnlyDictionary<string, Item> _items;
        //private ReadOnlyDictionary<string, Command> _commands;
        //private ReadOnlyDictionary<string, Challenge> _challenges;

        //public ReadOnlyDictionary<string, Item> ItemRegistry
        //{
        //    get => _items;
        //}

        //public ReadOnlyDictionary<string, Branch> BranchRegistry
        //{
        //    get => _branches;
        //}

        //public ReadOnlyDictionary<string, Command> CommandRegistry
        //{
        //    get => _commands;
        //}

        //public ReadOnlyDictionary<string, Challenge> ChallengeRegistry
        //{
        //    get => _challenges;
        //}

        private bool autoSave;
        private bool gameInitialized;

        private CommonProcess commonProcess;
        public CommonProcess CommonProcess => commonProcess;

        private StartupProcess startupProcess;
        public StartupProcess StartupProcess => startupProcess;

        private GameProcess gameProcess;
        public GameProcess GameProcess => gameProcess;

        //private EndingProcess _endingProcess;
        //public EndingProcess EndingProcess => _endingProcess;

        //private ResultProcess _resultProcess;
        //public ResultProcess ResultProcess;
        //public PromptManager PromptManager
        //{
        //    get => _commonProcess.PromptManager;
        //}

        void Start()
        {
            Debug.Log("GameManager 物件已加載並激活");
            var commonProcessObject = Instantiate(commonProcessPrefab, container.transform); //loading畫面
            commonProcessObject.GameObject().transform.SetAsFirstSibling();
            commonProcess = commonProcessObject.GetComponent<CommonProcess>();

           
            StartCoroutine(InternalStart());
        }

        private IEnumerator InternalStart()
        {
            commonProcess.SplashIn();
            yield return new WaitForSeconds(1f);
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
            yield return new WaitForSeconds(1f);
        }
        public void OnFirebaseInitialized(string message)
        {
            Debug.Log("Firebase 初始化完成：" + message);
            StartCoroutine(WaitForUnityReady());
        }

        // 等待 Unity 完全加載並準備好
        IEnumerator WaitForUnityReady()
        {
            while (!IsUnityReady())
            {
                Debug.Log("等待 Unity 完全加載...");
                yield return new WaitForSeconds(0.5f);
            }

            Debug.Log("Unity 加載完成，可以執行 Firebase 初始化後的流程");
        }

        bool IsUnityReady()
        {
            return commonProcess != null && commonProcess.gameObject.activeInHierarchy;
        }



        public void InitializeGame()
        {
            gameInitialized = true;
            StartCoroutine(InternalInitializeGame());
        }

        private IEnumerator InternalInitializeGame()
        {
            yield return new WaitForSeconds(1f);
            startupProcess.gameObject.SetActive(false);
            Debug.Log("開始生成 GameProcess...");
            var GameProcessGameObject = Instantiate(GameProcessPrefab, container.transform);

            if (GameProcessGameObject == null)
            {
                Debug.LogError("GameProcessPrefab 沒有成功生成！");
                yield break;
            }
            GameProcessGameObject.GameObject().transform.SetAsFirstSibling();
            gameProcess = GameProcessGameObject.GetComponent<GameProcess>();

            if (gameProcess == null)
            {
                Debug.LogError("GameProcess 沒有成功取得組件！");
                yield break;
            }
            if (!GameProcessGameObject.activeSelf)
            {
                Debug.LogWarning("GameProcess 是關閉狀態，將其啟動");
                GameProcessGameObject.SetActive(true);
            }
            //var collection = firestore.Collection(DATABASE_COLLECTION_USERDATA);
            //var saveReference = collection.Document(firebaseUser.UserId);
            //var saveRetrievalTask = saveReference.GetSnapshotAsync();

            //  Debug.Log($"[Firestore] Retrieving userdata ({firebaseUser.UserId})...");
            // while (!saveRetrievalTask.IsCompleted)
            // {
            //     yield return new WaitForSeconds(0.25f);
            // }

            // if (!saveRetrievalTask.IsCompletedSuccessfully)
            // {
            //Debug.Log($"[Firestore] Unable to retrieve userdata: {saveRetrievalTask.Exception}");
            //_commonProcess.SplashOut();
            //_commonProcess.PromptManager.CreateToast(10, ToastLevel.ERROR, "無法載入資料，請告知保育員");
            //yield break;
            // }

            // var save = saveRetrievalTask.Result;
            //  if (save.Exists)
            //  {
            //Debug.Log($"[Firestore] Found save for the current user {firebaseUser.UserId}.");
            //_userData = save.ConvertTo<UserData>();
            // }
            //else
            // {
            //Debug.Log("[Firestore] Creating new save...");
            //_userData = new UserData(firebaseUser.UserId);
            //// AddAsync would generate a random document Id
            //// So here we use SetAsync
            //var dataCreationTask = saveReference.SetAsync(_userData, SetOptions.MergeAll);
            //Debug.Log("[Firestore] Uploading new save...");
            //while (!dataCreationTask.IsCompleted)
            //{
            //    yield return new WaitForSeconds(0.25f);
            //}

            //if (!dataCreationTask.IsCompletedSuccessfully)
            //{
            //    Debug.Log($"[Firestore] Unable to create new save ({collection.Document().Path}): {dataCreationTask.Exception}");
            //    commonProcess.SplashOut();
            //    commonProcess.PromptManager.CreateToast(10, ToastLevel.ERROR, "無法上傳資料，請告知保育員");
            //    yield break;
            //}
            //Debug.Log("[Firestore] New save is created successfully!");
            //  }

            //var request = Resources.LoadAsync<GameObject>("Process/Game/GameProcess");
            //yield return request;
            //var gameProcessGameObject = Instantiate(request.asset, _container.transform);
            //_gameProcess = gameProcessGameObject.GetComponent<GameProcess>();
            ////
            //request = Resources.LoadAsync<GameObject>("Process/Ending/EndingProcess");
            //yield return request;
            //var endingProcessObject = Instantiate(request.asset, _container.transform);
            //_endingProcess = endingProcessObject.GetComponent<EndingProcess>();
            //_endingProcess.gameObject.transform.SetAsLastSibling();
            //_endingProcess.gameObject.SetActive(false);
            ////
            //request = Resources.LoadAsync<GameObject>("Process/Result/ResultProcess");
            //yield return request;
            //var resultProcessObject = Instantiate(request.asset, _container.transform);
            //_resultProcess = resultProcessObject.GetComponent<ResultProcess>();
            //_resultProcess.transform.SetAsLastSibling();
            //_resultProcess.gameObject.SetActive(false);
            ////
            //_startupProcess.gameObject.SetActive(false);
            Debug.Log("e044");
            //if (_userData.Advancements.ContainsKey("C1") && _userData.Advancements["C1"].Contains("256")
            //    || _userData.Advancements.ContainsKey("C2") && _userData.Advancements["C2"].Contains("256"))
            //{
            //    GameManager.Instance.FinishGame();
            //}
            commonProcess.SplashOut();
            yield return new WaitForSeconds(1f);
        }

        //public bool OnChallenge(string id, string answer)
        //{
        //    //if (!_challenges.ContainsKey(id))
        //    //{
        //    //    PromptManager.CreateToast(5.0f, ToastLevel.ERROR, "未知的謎題 (請通知保育員們)");
        //    //    return false;
        //    //}

        //    //var challenge = _challenges[id];
        //    //if (!challenge.Verify(answer))
        //    //{
        //    //    PromptManager.CreateToast(5.0f, ToastLevel.ERROR, $"謎題「{challenge.Title}」猜錯啦！");
        //    //    return false;
        //    //}

        //    //if (OnCommand(challenge.Command) != CommandResult.SUCCESS)
        //    //{
        //    //    return false;
        //    //}

        //    //_userData.Challenges[id] = true;
        //    //RequestSave();
        //    //PromptManager.CreateToast(5.0f, ToastLevel.DONE, $"謎題「{challenge.Title}」已完成！");
        //    //OnChallengeCompleted?.Invoke(challenge);
        //    //return true;
        //}


        //public CommandResult OnCommand(Command command)
        //{
        //    //var intersectedItems = _userData.Items.Intersect(command.ConsumedItems);
        //    //if (intersectedItems.Count() != command.ConsumedItems.Count())
        //    //{
        //    //    PromptManager.CreateToast(5.0f, ToastLevel.WARNING, "你還沒有集齊這個任務需要的道具！");
        //    //    return CommandResult.LACK_OF_ITEMS;
        //    //}

        //    //foreach (var branch in command.RequiredAdvancements)
        //    //{
        //    //    if (!_userData.Advancements.ContainsKey(branch.Key))
        //    //    {
        //    //        PromptManager.CreateToast(5.0f, ToastLevel.WARNING, "你還沒完成這個任務需要的進度！");
        //    //        return CommandResult.LACK_OF_ADVANCEMENTS;
        //    //    }

        //    //    var intersectedAdvancements = _userData.Advancements[branch.Key].Intersect(branch.Value);
        //    //    if (intersectedAdvancements.Count() != branch.Value.Count())
        //    //    {
        //    //        PromptManager.CreateToast(5.0f, ToastLevel.WARNING, "你還沒完成這個任務需要的進度！");
        //    //        return CommandResult.LACK_OF_ADVANCEMENTS;
        //    //    }
        //    //}

        //    ////
        //    ////

        //    //foreach (var consumed in command.ConsumedItems)
        //    //{
        //    //    _userData.Items.Remove(consumed);
        //    //}

        //    //if (command.Challenges != null)
        //    //{
        //    //    foreach (var challenge in command.Challenges)
        //    //    {
        //    //        _userData.Challenges[challenge] = false;
        //    //    }
        //    //}

        //    //int baseDisplayTime = 5 + command.RewardItems.Count;
        //    //foreach (var rewardItem in command.RewardItems)
        //    //{
        //    //    _userData.Items.Add(rewardItem);
        //    //    if (!_items.ContainsKey(rewardItem))
        //    //    {
        //    //        continue;
        //    //    }

        //    //    var item = _items[rewardItem];
        //    //    PromptManager.CreateToast(baseDisplayTime--, ToastLevel.DONE, $"獲得道具：{item.DisplayName}");
        //    //    OnItemReceived?.Invoke(item);
        //    //}

        //    //foreach (var branch in command.Advancements)
        //    //{
        //    //    if (!_userData.Advancements.ContainsKey(branch.Key))
        //    //    {
        //    //        _userData.Advancements[branch.Key] = new List<string>();
        //    //    }
        //    //    _userData.Advancements[branch.Key].AddRange(branch.Value);

        //    //    foreach (var advancement in branch.Value)
        //    //    {
        //    //        OnAdvancementAchieved?.Invoke(branch.Key, advancement);
        //    //    }
        //    //}

        //    //if (command.Reputation != 0)
        //    //{
        //    //    PromptManager.CreateToast(
        //    //        baseDisplayTime,
        //    //        command.Reputation >= 0 ? ToastLevel.DONE : ToastLevel.WARNING,
        //    //        $"信望值 {command.Reputation} ({_userData.Reputation} -> {_userData.Reputation + command.Reputation})"
        //    //    );
        //    //    _userData.Reputation += command.Reputation;
        //    //}

        //    //return CommandResult.SUCCESS;
        //}

      //  public bool OnCodeReceived(string data)
      //  {
            //if (!_commands.ContainsKey(data))
            //{
            //    return false;
            //}

            //if (_userData.ScannedCodes.Contains(data))
            //{
            //    PromptManager.CreateToast(5.0f, ToastLevel.WARNING, "這個進度已經完成過了！");
            //    return true;
            //}

            //var command = _commands[data];

            //if (OnCommand(command) != CommandResult.SUCCESS)
            //{
            //    return true;
            //}

            //_userData.ScannedCodes.Add(data);
            //RequestSave();
            //return true;
       // }


        //public void Ending()
        //{
        //    _commonProcess.SplashIn(
        //        () =>
        //        {
        //            _gameProcess.gameObject.SetActive(false);
        //            _endingProcess.gameObject.SetActive(true);
        //            _endingProcess.Activate();
        //        }
        //    );
        //}


        //public void FinishGame()
        //{
        //    _commonProcess.SplashIn(
        //        () =>
        //        {
        //            _gameProcess.gameObject.SetActive(false);
        //            _resultProcess.gameObject.SetActive(true);
        //            _resultProcess.Initialize();
        //        }
        //    );

        //    if (UserData.FinishTimestamp != -1)
        //    {
        //        UserData.FinishTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
        //    }
        //}

        //public void RequestSave()
        //{
        //    StartCoroutine(Save());
        //}

        //private IEnumerator Save()
        //{
        //    // Wait until the current session is ended
        //    yield return new WaitUntil(() => !_autoSave);
        //    _autoSave = true;
        //    var firestore = FirebaseFirestore.DefaultInstance;
        //    var firebaseUser = FirebaseAuth.DefaultInstance.CurrentUser;
        //    var collection = firestore.Collection(DATABASE_COLLECTION_USERDATA);
        //    var saveReference = collection.Document(firebaseUser.UserId);

        //    int retry = 3;
        //    while (retry != 0)
        //    {
        //        var autoSaveTask = saveReference.SetAsync(_userData, SetOptions.Overwrite);
        //        yield return new WaitUntil(() => autoSaveTask.IsCompleted);
        //        if (!autoSaveTask.IsCompletedSuccessfully)
        //        {
        //            PromptManager.CreateToast(3.0f, ToastLevel.ERROR, "自動存檔失敗！");
        //            Debug.Log($"[AutoSave] Could not upload save {autoSaveTask.Exception}");
        //            retry--;
        //        }
        //        else
        //        {
        //            _autoSave = false;
        //            break;
        //        }
        //    }
        //    _autoSave = false;
        //    yield break;
        //}

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(this);
                Debug.Log("An unknown error occurred while launching the app.");
            }
        }
        public void DisplayUser(string info)
        {
            var parsedUser = StringSerializationAPI.Deserialize(typeof(FirebaseUser), info) as FirebaseUser;
            Debug.Log($"Email: {parsedUser.email}, UserId: {parsedUser.uid}, EmailVerified: {parsedUser.isEmailVerified}");
            commonProcess.SplashIn();
            InitializeGame();
        }
        public void DisplayInfo(string info)
        {
            Debug.Log(info);
        }

        public void DisplayError(string error)
        {
            var parsedError = StringSerializationAPI.Deserialize(typeof(FirebaseError), error) as FirebaseError;
            Debug.Log(parsedError.message);
        }
    }
}

