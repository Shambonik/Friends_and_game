//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;
//using UnityEngine.UI;
//using _Games.Scripts.Netwotking;

//namespace UI.ButtonGeneration
//{
//    /// <summary>
//    /// Генерация предметов персонажа.
//    /// </summary>
//    public class UIInventoryGeneration : UIButtonGeneration
//    {
//        //Текстовое поле для вывода информации о предмете
//        [SerializeField] private Text _itmeInfoTextBox;

//        //Скролл панель инвенторя 
//        [SerializeField] private RectTransform _inventoryContent;

//        //Массив предметов в инвентаре
//        //private readonly Dictionary<long, Item> inventory = new Dictionary<long, Item>();

//        ////Массив преметов на складе
//        //private Dictionary<long, Item> storage = new Dictionary<long, Item>();

//        ////Выбранная шмотка
//        //private Item selectedItem;

//        /// <summary>
//        /// Генерируем все шмотки, полученные с сервера
//        /// </summary>
//        public void GetUnitItems()
//        {
//            ////Считываем id персонажа
//            //long unitId = GameManager.GetGameManager().SelectedUnit.Id;
//            ////Получаем все шмотки с сервера
//            //storage = UnitAPI.GetUnitItems(unitId).ToDictionary(item => item.Id, item => item);

//            ////Запускаем генератор
//            //OnReceivedModels(storage, ClickAction, СhangesButton);
//        }

//        /// <summary>
//        /// Надеть шмотку
//        /// </summary>
//        public void PutOnItem()
//        {
//            //Inventory inventory = new Inventory {Items = this.inventory, ContentPanel = _inventoryContent};
//            //Inventory storage = new Inventory {Items = this.storage, ContentPanel = _contentPanel};

//            //DraggindItem(inventory, storage);
//        }

//        /// <summary>
//        /// Снять шмотку
//        /// </summary>
//        public void TakeOffItem()
//        {
//            //Inventory inventory = new Inventory {Items = this.inventory, ContentPanel = _inventoryContent};
//            //Inventory storage = new Inventory {Items = this.storage, ContentPanel = _contentPanel};

//            //DraggindItem(storage, inventory);
//        }

//        /// <summary>
//        /// Метод, который выводит на кнопке название шмотки.
//        /// </summary>
//        /// <param name="buttonModel">Модель кнопки</param>
//        /// <param name="itemModel">Модель шмотки</param>
//        private void СhangesButton(TestItemView buttonModel, Item itemModel)
//        {
//            //Меняем имя кнопки
//            buttonModel.clickButton.name = $"{itemModel.Code}_{itemModel.Id}";
//            //Получаем тектсовое поле у кнопки
//            Text itemTitle = buttonModel.clickButton.GetComponentInChildren<Text>();
//            //Передаем в текстовое поле название шмотки        
//            itemTitle.text = itemModel.Title;
//        }

//        /// <summary>
//        /// Метод, который выводит информации о шмотке при нажатии на кнопку
//        /// </summary>
//        /// <param name="itemModel">Параметры шмотки</param>
//        private void ClickAction(Item itemModel)
//        {
//            selectedItem = itemModel;
//            //Передаем в текстовое поле название шмотки
//            _itmeInfoTextBox.text = $"{itemModel.Title}\n";
//        }

//        /// <summary>
//        /// Метод, который перетаскивает шмотки из хранилища в инвентарь и наоборот
//        /// </summary>
//        /// <param name="placePut">Куда кладем шмотку</param>
//        /// <param name="itemTake">Откуда берем шмотку</param>
//        private void DraggindItem(Inventory placePut, Inventory itemTake)
//        {
//            //Проверка, есть ли в инвенторе этот предмет 
//            if (!placePut.Items.ContainsKey((int) selectedItem.Id))
//            {
//                GameObject needfullButton = null;

//                //Хуевый способ нахождения кнопки по названию самой кнопки
//                for (int i = 0; i < itemTake.ContentPanel.childCount; i++)
//                {
//                    if (itemTake.ContentPanel.GetChild(i).name ==
//                        $"{placePut.Items[(int) selectedItem.Id].Code}_{placePut.Items[(int) selectedItem.Id].Id}")
//                        needfullButton = itemTake.ContentPanel.GetChild(i).gameObject;
//                }

//                //Перемещаем в массив выбранный предмет
//                placePut.Items[selectedItem.Id] = selectedItem;
//                //Переносим кнопку в панель
//                needfullButton.transform.SetParent(placePut.ContentPanel, false);
//                //Удаляем из другого массива выбранный объект 
//                itemTake.Items.Remove(selectedItem.Id);
//            }
//            else
//                Debug.Log($"\"{selectedItem.Title}\" нельзя перенести.");
//        }
//    }

////Модель для инвенторя 
//    public class Inventory
//    {
//        //Массив предметов
//        public Dictionary<long, Item> Items;

//        //Панель, в которой находятся предметы
//        public RectTransform ContentPanel;
//    }
//}
