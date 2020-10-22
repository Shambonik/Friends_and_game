using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Games.Scripts.Netwotking;
using AureoleCore.API;
using AureoleCore.Models;
using Cinemachine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UI;
using UnityEngine;
using UnityEngine.Networking.Types;


public class Network : MonoBehaviour
{

    public class ClientObj
    {
        public GameObject CharacterObj;
        public Character CharacterComponent;
    }

    public static readonly ClientObj clientObj = new ClientObj();
    //private InputSystem inputSystem;
    public static GameAPI gameAPI;
    private float tick = 0.01f;

    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _enemy;
    [SerializeField] private GameObject _gold_turtle;

    private readonly Dictionary<int, GameObject> _players = new Dictionary<int, GameObject>();
    private readonly Dictionary<int, GameObject> _enemies = new Dictionary<int, GameObject>();
    private readonly static Dictionary<int, Unit> _playerComponent = new Dictionary<int, Unit>();
    private readonly static Dictionary<int, Unit> _enemyComponent = new Dictionary<int, Unit>();
    private readonly HashSet<int> _eventIDs = new HashSet<int>();
    [SerializeField] private CinemachineFreeLook _camera;
    private MessageScript messageScript;


    //Итемы
    private readonly Dictionary<int, GameObject> _items = new Dictionary<int, GameObject>();

    //private ItemMenu itemMenu;
    [SerializeField] private GameObject itemPrefab;

    ///private InventoryScript inventory;

    void Start()
    {
        gameAPI = new GameAPI();
        GameAPI.gameAPI = gameAPI;
        //inputSystem = FindObjectOfType<InputSystem>();
        _camera = FindObjectOfType<CinemachineFreeLook>();

        messageScript = FindObjectOfType<MessageScript>();

        //itemMenu = FindObjectOfType<ItemMenu>();
        //inventory = FindObjectOfType<InventoryScript>();

        gameAPI.Host = "192.168.1.236:9800";
        gameAPI.Connect(UIMainMenu.sessingId, GameManager.GetGameManager().UnitId);

        StartCoroutine(GameLoop());
    }

    private void UnitsDataSender(UnitsUpdate serverObj, Dictionary<int, Unit> _components)
    {
        if (!_components.ContainsKey((int) serverObj.Id)) return;

        _components[(int) serverObj.Id].Direction = new Vector3(serverObj.Dx, 0, serverObj.Dy);
        _components[(int) serverObj.Id].NextPosition = new Vector3(serverObj.X, 0, serverObj.Y);
        _components[(int) serverObj.Id].Health = serverObj.Hp;
    }

    private void EventHandler()
    {
        if (gameAPI.WorldUpdate.Events == null) return;

        foreach (var serverEvent in gameAPI.WorldUpdate.Events.Where(serverEvent =>
            !_eventIDs.Contains((int) serverEvent.Id)))
        {
            _eventIDs.Add((int) serverEvent.Id);

            switch (serverEvent.Method)
            {
                case "base_attack":
                    var attackPlayer =
                        JsonConvert.DeserializeObject<UnitsUpdate>(JObject.Parse(serverEvent.Info).ToString());
                    Debug.Log("Server Attack " + _playerComponent);
                    if (_playerComponent.ContainsKey((int) attackPlayer.Id))
                        _playerComponent[(int) attackPlayer.Id].AttackAnimation();
                    break;
                case "base_attack_unit":
                    var attackUnit =
                        JsonConvert.DeserializeObject<UnitsUpdate>(JObject.Parse(serverEvent.Info).ToString());

                    if (_enemyComponent.ContainsKey((int) attackUnit.Id))
                        _enemyComponent[(int) attackUnit.Id].AttackAnimation();

                    break;
                case "server_message":
                    Debug.Log("Message");
                    string textMessage = serverEvent.Info;
                    messageScript.addMessage(textMessage);
                    break;
            }
        }
    }

    private void TargetForCam(GameObject target)
    {
        //Debug.Log("Target cam " + _camera);
        _camera.Follow = target.transform;
        _camera.LookAt = target.transform;
        //Debug.Log(_camera.Follow + " " + _camera.LookAt);
    }

    private void UnitsDrawing(long unitID, List<UnitsUpdate> list, Dictionary<int, Unit> _component,
        Dictionary<int, GameObject> units, GameObject prefab)
    {
        var serverUnits = list.ToDictionary(a => a.Id, a => a);

        var serverUnit = serverUnits[(int) unitID];
        var spawnPosition = new Vector3((float) serverUnit.X, 0, (float) serverUnit.Y);
        var id = (int) serverUnit.Id;

        var unit = Instantiate(original: prefab, position: spawnPosition, rotation: Quaternion.identity);

        units[id] = unit;
        _component[id] = unit.GetComponent<Unit>();

        if (GameAPI.GameBuffer.Id == serverUnit.Id)
        {
            TargetForCam(unit);
            clientObj.CharacterObj = unit;
            clientObj.CharacterComponent = unit.GetComponent<Character>();
            InputSystem.setPlayer(clientObj.CharacterComponent);
            Debug.Log("Player added");
        }
    }

    private void WorldUpdate(List<UnitsUpdate> list, Dictionary<int, GameObject> units,
        Dictionary<int, Unit> unitComponents /*, GameObject prefab*/)
    {
        if (list != null)
        {
            var exist = new HashSet<int>();
            foreach (var id in units.Keys)
            {
                exist.Add(id);
            }

            foreach (var unit in list)
            {
                if (!units.ContainsKey((int) unit.Id))
                {
                    GameObject prefab = null;
                    switch (unit.ModelInfo)
                    {
                        case "caetus":
                            prefab = _player;
                            break;
                        case "baby_shark":
                            prefab = _enemy;
                            break;
                        case "gold_turtle":
                            Debug.Log("CHEREPAHA");
                            prefab = _gold_turtle;
                            break;
                    }
                    
                    if (prefab != null) UnitsDrawing(unit.Id, list, unitComponents, units, prefab);
                }
                else
                    exist.Remove((int) unit.Id);
            }

            foreach (var id in exist)
            {
                Destroy(units[id]);
                units.Remove(id);
                unitComponents.Remove(id);
            }

            foreach (var serverUnit in list)
            {
                UnitsDataSender(serverUnit, unitComponents);
            }
        }
    }


    private void ItemDrawing(long itemID, List<ItemUpdate> list, GameObject prefab)
    {
        var serverItems = list.ToDictionary(a => a.Id, a => a);

        var serverItem = serverItems[(int) itemID];
        var spawnPosition = new Vector3((float) serverItem.X, 0, (float) serverItem.Y);
        var id = (int) serverItem.Id;

        var item = Instantiate(original: prefab, position: spawnPosition, rotation: Quaternion.identity);

        _items[id] = item;
    }

    private void ItemUpdate(List<ItemUpdate> list, Dictionary<int, GameObject> items)
    {
        if (list != null)
        {

            var exist = new HashSet<int>();
            foreach (var id in items.Keys)
            {
                exist.Add(id);
            }


            ItemMenu.newList();
            foreach (var item in list)
            {
                ItemMenu.addItem(item.Id, item.ModelInfo + " " + item.Count);
                if (!items.ContainsKey((int) item.Id))
                {
                    //item.ModelInfo;
                    //item.Count
                    GameObject prefab = itemPrefab;
                    ItemDrawing(item.Id, list, prefab);
                }
                else
                    exist.Remove((int) item.Id);
            }

            foreach (var id in exist)
            {
                Destroy(items[id]);
                items.Remove(id);
            }

            /*foreach (var serverItem in list)
            {
                UnitsDataSender(serverItem);
            }*/
        }
    }
    
    

    
    //без использования Network (напрямую с GameAPI)
    /*public static void getInventory()
    {
        gameAPI.getInventory();
    }

    public void sendTakeItem(long id)
    {
        gameAPI.takeItem(id);
    }*/


    private IEnumerator GameLoop()
    {
        while (true)
        {
            if (GameAPI.GameBuffer.Key != 0)
            {
                gameAPI.Ping();
                gameAPI.Key(InputSystem.GetMovement().x, InputSystem.GetMovement().y);
                GameAPI.GameBuffer.Aid = 0;

                //WorldUpdate(gameAPI.WorldUpdate.Players, _players, _playerComponent);
                WorldUpdate(gameAPI.WorldUpdate.Units, _enemies, _enemyComponent);

                //Итемы
                ItemUpdate(gameAPI.WorldUpdate.Items, _items);
                //InventoryScript.setText(gameAPI.inventory);

                /*WorldUpdate(gameAPI.WorldUpdate.Players, _players, _playerComponent, _player);
                WorldUpdate(gameAPI.WorldUpdate.Units, _enemies, _enemyComponent, _enemy);*/

                EventHandler();
            }

            yield return new WaitForSeconds(tick);
        }

    }

}
