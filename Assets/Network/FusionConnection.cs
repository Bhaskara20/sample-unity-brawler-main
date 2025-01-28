using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using Unity.VisualScripting;
using UnityEngine.UI;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;


namespace baskara.FusionBites
{
    public class FusionConnection : MonoBehaviour, INetworkRunnerCallbacks
    {
        public static FusionConnection Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<FusionConnection>();
                }
                return _instance;
            }
        }

        private static FusionConnection _instance;
        public bool connectOnAwake = false;
        public NetworkRunner runner;

        [SerializeField] NetworkObject playerPrefab;

        public string _playerName;

        [Header("Session List")]
        public GameObject roomListCanvas;
        private List<SessionInfo> _sessions = new List<SessionInfo>();
        public Button refreshButton;
        public Transform sessionListContent;
        public GameObject sessionEntryPrefab;
        public string currentRoomName;

        public GameObject Cube;
        public GameObject ObjectActivator;

        public List<NetworkObject> spawnedObjects = new List<NetworkObject>();

        public Animator panelAnimator;
        public Animator endAnimator;

        private bool isRestarting = false;

        private void Awake()
        {
            //if (Instance == null) { Instance = this; }
            if (_instance == null)
            {
                _instance = this;
                //DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject); // Hindari duplikasi
            }

        }

        public void ConnectToLobby(string playerName)
        {
            roomListCanvas.SetActive(true);
            _playerName = playerName;
            if (runner == null)
            {
                runner = gameObject.AddComponent<NetworkRunner>();
            }

            runner.JoinSessionLobby(SessionLobby.Shared);
        }

        public async void ConnectToSession(string sessionName)
        {
            //_playerName = playerName;
            roomListCanvas.SetActive(false);
            if (runner == null)
            {
                runner = gameObject.AddComponent<NetworkRunner>();
            }

            await runner.StartGame(new StartGameArgs()
            {
                GameMode = GameMode.Shared,
                SessionName = sessionName,
                PlayerCount = 2,
            });

            //menuOVR.SetActive(false);
        }

        public async void CreateSession()
        {
            roomListCanvas.SetActive(false);
            int randomInt = UnityEngine.Random.Range(1000, 9999);
            string randomSessionName = "Room-" + randomInt.ToString();

            if (runner == null)
            {
                runner = gameObject.AddComponent<NetworkRunner>();
            }

            await runner.StartGame(new StartGameArgs()
            {
                GameMode = GameMode.Shared,
                SessionName = randomSessionName,
                PlayerCount = 2,
            });
            //menuOVR.SetActive(false);
            //NetworkObject sb = runner.Spawn(ScoreBoardManager, new Vector3(-0.7877298f, 1.613353f, 2.165967f));
            //NetworkObject sbCanvas = runner.Spawn(CanvasScoreboard, new Vector3(6.99f, 5.01f, 6.93f));
            //NetworkObject trig = runner.Spawn(ScoreTrigger, new Vector3(0.708f, 0.1558f, 0.878f));
            //NetworkObject gelas = runner.Spawn(glass, new Vector3(3.58f, 0.8044f, 2.721f));

            //NetworkObject kubus = runner.Spawn(Cube, new Vector3(497.169f, 5.634837f, 496.1804f));
            //spawnedObjects.Add(kubus);
            //NetworkObject kubus1 = runner.Spawn(Cube, new Vector3(496.540009f, 6.09700012f, 502.329987f));
            //NetworkObject kubus2 = runner.Spawn(Cube, new Vector3(504.109985f, 6.09700012f, 501.48999f));
            //NetworkObject kubus3 = runner.Spawn(Cube, new Vector3(501.26001f, 6.09700012f, 494.600006f));
            //NetworkObject kubus4 = runner.Spawn(Cube, new Vector3(504.399994f, 6.09700012f, 497.799988f));
            NetworkObject ObjectAct = runner.Spawn(ObjectActivator, new Vector3(500.38f, 9.45f, 499.33f));
            //spawnedObjects.Add(ObjectAct);

        }

        public void RefreshSessionListUI()
        {
            //cegah duplicate session
            foreach (Transform child in sessionListContent)
            {
                Destroy(child.gameObject);
            }

            foreach (SessionInfo session in _sessions)
            {
                if (session.IsVisible)
                {
                    GameObject entry = GameObject.Instantiate(sessionEntryPrefab, sessionListContent);
                    SessionEntryPrefab script = entry.GetComponent<SessionEntryPrefab>();
                    script.sessionName.text = session.Name;
                    script.playerCount.text = session.PlayerCount + "/" + session.MaxPlayers;

                    if (session.IsOpen == false || session.PlayerCount >= session.MaxPlayers)
                    {
                        script.joinButton.interactable = false;
                    }
                    else
                    {
                        script.joinButton.interactable = true;
                    }
                }
            }
        }
        public async void ConnectToRunner(string playerName)
        {
            _playerName = playerName;   
            if (runner == null)
            {
                runner = gameObject.AddComponent<NetworkRunner>();
            }

            await runner.StartGame(new StartGameArgs()
            {
                GameMode = GameMode.Shared,
                SessionName = "roomName",
                //Scene = 3,
                PlayerCount = 2,
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
            });
        }

        public void OnConnectedToServer(NetworkRunner runner)
        {
            Debug.Log("Connected to Server!");
            NetworkObject playerObject = runner.Spawn(playerPrefab, new Vector3(496.27f, 5.530123f, 499.737f), Quaternion.identity, runner.LocalPlayer);
            spawnedObjects.Add(playerObject);
            runner.SetPlayerObject(runner.LocalPlayer, playerObject);

            currentRoomName = runner.SessionInfo.Name;
        }


        public async void RestartRoom()
        {
            if (runner != null && runner.IsRunning)
            {
                string currentRoomName = runner.SessionInfo.Name;
                isRestarting = true;
                await runner.Shutdown();
                
                
                Destroy(runner);
                StartCoroutine(RestartRoomCoroutine());

                runner = gameObject.AddComponent<NetworkRunner>();
                await runner.StartGame(new StartGameArgs()
                {
                    GameMode = GameMode.Shared,
                    SessionName = currentRoomName,
                    PlayerCount = 2
                });
                isRestarting = false;
                Debug.Log("Room restarted successfully!");
                NetworkObject ObjectAct = runner.Spawn(ObjectActivator, new Vector3(500.38f, 9.45f, 499.33f));
                panelAnimator.Play("New State");
                endAnimator.Play("New State");
            }
            else
            {
                Debug.LogWarning("Runner is not running!");
            }
            
        }

        private IEnumerator RestartRoomCoroutine()
        {
            yield return new WaitForSeconds(1f);
        }




        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
        {

        }

        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
        {

        }

        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
        {

        }

        public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
        {

        }

        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {

        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {

        }

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
        {

        }

        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {

        }

        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {

        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            Debug.Log("Player Joined!");
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {

        }

        public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
        {

        }

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
        {

        }

        public void OnSceneLoadDone(NetworkRunner runner)
        {

        }

        public void OnSceneLoadStart(NetworkRunner runner)
        {

        }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
            Debug.Log("Session List Updated!");
            _sessions.Clear();
            _sessions = sessionList;
        }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
            if (isRestarting == true)
            {
                // Memastikan ada instance FusionConnection yang valid
                if (FusionConnection.Instance != null)
                {
                    FusionConnection.Instance.RestartRoom();
                }
                else
                {
                    Debug.LogWarning("FusionConnection instance is null!");
                }
            }
            else
            {
                
                
            }
            
        }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
        {

        }

        public async void LeaveRoom()
        {
            if (runner != null && runner.IsRunning)
            {
                Debug.Log($"Leaving room: {runner.SessionInfo.Name}");

                // Hentikan game dan runner
                await runner.Shutdown();
                //Destroy(runner);

                // Kosongkan daftar objek yang di-spawn
                //spawnedObjects.Clear();

                //runner = null;
                //Scene
                // Tampilkan kembali UI daftar room
                //roomListCanvas.SetActive(true);

                Debug.Log("Left the room successfully and returned to the room list!");
                SceneManager.LoadScene(0);
            }
            else
            {
                Debug.LogWarning("No active room to leave!");
            }
        }






    }


}

