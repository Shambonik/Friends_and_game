using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Net.Mime;
using AureoleCore.API;
//using _Games.Scripts.Netwotking;
using AureoleCore.UserAPI;
using UI.ButtonGeneration;
using UnityEngine.SceneManagement;
using GameObject = UnityEngine.GameObject;

namespace UI
{
    public class UIMainMenu : MonoBehaviour
    {
        //Позиция спавна модели (дальше будут расы) игровго героя
        [SerializeField] protected Vector3 playerPosition;
        //Массив моделей (дальше будут расы)
        [SerializeField] protected GameObject[] models;
        //Текстовое поле для вывода никнейма игрока
        [SerializeField] private Text _nickname;
        
        [SerializeField] private Text _registrationNickname;
        //Текстовое поле для ввода никнейма игрока
        //[SerializeField] private Text _registrationTextBox;
        //[SerializeField] private InputField _changedNickname;

        [SerializeField] private GameObject _registrationPanel; 
        [SerializeField] private GameObject _mainMenu;

        public static string sessingId;
        //private static MatchAPI matchAPI;
        private void Start()
        {
            ////Передаем никнейм с базы данных
            //SetNickname(UserAPI.GetUser().Nickname);
            ////Спавним игрового героя
            //CharacterSpawn();
            
            //var userReg = UserAPI.CreateUser(new DeviceInfo().AnroidID());
            //var userLogin = UserAPI.LoginUser(new DeviceInfo().AnroidID());
            
            /*sessingId = userLogin.result.SessionId;
            Debug.Log("Session ID " + sessingId);
            _mainMenu.GetComponent<UICharacterGeneration>().getUnits();
            if (userReg.error != null)
            {
                SetNickname(userReg.result.nickname);
                
                _registrationPanel.SetActive(false);
                _mainMenu.SetActive(true);
            }*/
            var x = UserAPI.CheckRegistered(DeviceInfo.AnroidID());
            if (UserAPI.CheckRegistered(DeviceInfo.AnroidID()).data.exists)
            {
                Debug.Log("Already registered");
                
                _registrationPanel.SetActive(false);
                _mainMenu.SetActive(true);
                _mainMenu.GetComponent<UICharacterGeneration>().getUnits();
                SceneManager.LoadScene("Character");
            }
        }

        /// <summary>
        /// Метод, который отправляет запрос на сервер о том, что клиент хочет найти комнату для игры
        /// </summary>
        public void FindGame()
        {
            SceneManager.LoadScene("DemoScene");
            
        }
        
        /// <summary>
        /// Метод, который отправляет запрос на сервер о том, что клиент перестал искать комнату для игры
        /// </summary>
        public void CancelGame()
        {
            //matchAPI.Cancel();
        }

        /// Метод, который отправляет запрос на сервер о том, что клиент поменял себе никнейм
        /// </summary>
        public void ChangeNickname(string input)
        {
            //Debug.Log("Change nickname " + input);
            //var userReg = UserAPI.CreateUser(new DeviceInfo().AnroidID());
            var name = UserAPI.CreateUser(DeviceInfo.AnroidID(), input).data.user_id;
            SetNickname(name.ToString());
            Debug.Log(name);
        }

        /// <summary>
        /// Метод, который выводит в текстовое поле никнейм игрока
        /// </summary>
        /// <param name="nickname">Сам никнейм</param>
        private void SetNickname(string nickname)
        {
            _nickname.text = nickname;
        }
        
        /// <summary>
        /// Метод, который спавнит игрового героя
        /// </summary>
        //private void CharacterSpawn()
        //{
        //    GameManager manager = new GameManager();
        //    foreach (var t in models)
        //    {
        //        foreach (var unit in UserAPI.GetUser().Units.Where(unit => t.name == unit.Race))
        //        {
        //            manager.MenuCharacter = Instantiate(t, playerPosition, Quaternion.Euler(0, 180, 0));
        //        }
        //    }
        //}

        /// <summary>
        /// Метод, который отключает панель через 0.3 секунды. Нужен для того, чтобы спокойно отобразить анимацию панели, если таковая имеется.
        /// </summary>
        /// <param name="panel">Панель, которая должна отключиться</param>
        public void CollapsingPanel(GameObject panel)
        {
            StartCoroutine(CollapsingTimer(panel));
        }

        public void OpenCharactersPanel()
        {
            
        }
        
        public void UserRegistration()
        {
            ChangeNickname(_registrationNickname.text);
            _registrationPanel.SetActive(false);
            _mainMenu.SetActive(true);
            _mainMenu.GetComponent<UICharacterGeneration>().getUnits();
            _mainMenu.GetComponent<UICharacterGeneration>().getUnits();
            SceneManager.LoadScene("Character");
        }
        /// <summary>
        /// Таймер для отключения панели
        /// </summary>
        /// <param name="panel">Панель, которая должна отключиться</param>
        private static IEnumerator CollapsingTimer(GameObject panel)
        {
            yield return new WaitForSeconds(0.3f);
            panel.SetActive(false);
        }
    }
}
