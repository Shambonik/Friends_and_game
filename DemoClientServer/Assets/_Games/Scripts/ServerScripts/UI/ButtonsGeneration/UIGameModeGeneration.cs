//using System.Collections.Generic;
//using System.Linq;
//using _Games.Scripts.Netwotking;
//using UnityEngine;
//using UnityEngine.UI;

//namespace UI.ButtonGeneration
//{
//    /// <summary>
//    /// Генерация всех игровых режимов, которые находятся на сервере
//    /// </summary>
//    public class UIGameModeGeneration : UIButtonGeneration
//    {
//        //Панель с информацией о игрвом режиме
//        [SerializeField] private GameObject _infoPanel;

//        //Текстовое поле для название игрового режима
//        [SerializeField] private Text _modeNameTextBox;

//        //Текстовое поле для описание игрового режима
//        [SerializeField] private Text _modeDiscriptionTextBox;

//        //Панели основного экрана и экарана игровых режимов
//        [SerializeField] private GameObject mainMenu, modeMenu;

//        /// <summary>
//        /// Генерация кнопок игровых режимов, которые приходят с сервера
//        /// </summary>
//        public void GameModesGeneration()
//        {
//            //Создаем список клиентских игровых режимов
//            //Dictionary<string, Mode> clientModes = MatchAPI.Modes().ToDictionary(mode => mode.Code, mode => mode);

//            //Запускаем генератор 
//            OnReceivedModels(clientModes, ClickAction, СhangesButton);
//        }

//        /// <summary>
//        /// Изменяем параметры полученной кнопки
//        /// </summary>
//        /// <param name="buttonModel">Модель кнопки</param>
//        /// <param name="gameMode">Игровой режим</param>
//        private void СhangesButton(TestItemView buttonModel, Mode gameMode)
//        {
//            //Получаем текстовое поле кнопки
//            var textTitle = buttonModel.clickButton.GetComponentInChildren<Text>();
//            //Получаем дочернюю кнопку для вывода информации о игровом режиме
//            var infoButton = buttonModel.clickButton.GetComponentsInChildren<Button>();

//            //Передаем в событие нажатия на кнопку информации метод ModeInfo
//            infoButton.Last().onClick.AddListener(() => ModeInfo(gameMode));
//            //Выводим на кнопку название игрового режима
//            textTitle.text = gameMode.Title;
//        }

//        /// <summary>
//        /// Метод, который срабатывает при нажатии на кнопку
//        /// </summary>
//        /// <param name="gameMode">Игровой режим</param>
//        private void ClickAction(Mode gameMode)
//        {
//            //Ссылаемся на GameManager
//            GameManager manager = GameManager.GetGameManager();
            
//            //Передаем кодовое значние игрового режима
//            manager.GameModeId = gameMode.Code;
//            //Переключаем экраны меню, после выбора игрового режима
//            modeMenu.gameObject.SetActive(false);
//            mainMenu.SetActive(true);
//            //Активируем модель персонажа в главном меню
//            manager.MenuCharacter.SetActive(true);
//        }

//        /// <summary>
//        /// Метод, который срабатывает при нажатии на кнопку информации
//        /// </summary>
//        /// <param name="gameMode">Игровой режим</param>
//        private void ModeInfo(Mode gameMode)
//        {
//            //Активируем информационную панель
//            _infoPanel.SetActive(true);
//            //Передаем в текстовое поле название игрового режима
//            _modeNameTextBox.text = $"{gameMode.Title}";
//            //Передаем в текстовое поле описание игрового режима
//            _modeDiscriptionTextBox.text = $"{gameMode.Description}";
//        }
//    }
//}




