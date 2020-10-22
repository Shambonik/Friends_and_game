using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.ButtonGeneration
{
    /// <summary>
    /// Класс с базовыми методами для генераций различных кнопок с информацией, полученной с сервера
    /// </summary>
    public abstract class UIButtonGeneration : MonoBehaviour
    {
        //Префаб кнопки, которая будет использовать для генерации
        [SerializeField] protected RectTransform _buttonPrefab;

        //Панель, в которой будут генерироваться кнопки
        [SerializeField] protected RectTransform _contentPanel;

        /// <summary>
        /// Генерация кнопок
        /// </summary>
        /// <param name="models">Массив моделей. которые хранят в себе какие-то данные</param>
        /// <param name="clickAction">Метод, который будет срабатывать при нажатии на сгенерируемую кнопку</param>
        /// <param name="changeButtonParams">Метод, который меняет параметры кнопки (используя значения модели)</param>
        /// <typeparam name="K">Шаблон ключа</typeparam>
        /// <typeparam name="V">Шаблон значений</typeparam>
        protected void OnReceivedModels<K, V>(Dictionary<K, V> models, UnityAction<V> clickAction,
            UnityAction<TestItemView, V> changeButtonParams)
        {
            //Удаляем страые кнопки из панели
            foreach (Transform child in _contentPanel)
            {
                Destroy(child.gameObject);
            }
            
            //Спавним новые кнопки 
            foreach (var model in models)
            {
                //Спавн кнопки
                var instance = Instantiate(_buttonPrefab.gameObject);
                //Делаем кнопку дочерним обхектом панели
                instance.transform.SetParent(_contentPanel, false);
                //Инициализация кнопки
                InitializeItemView(instance, model, clickAction, changeButtonParams);
            }
        }

        /// <summary>
        /// Инициализация сгененрируемой кнопки
        /// </summary>
        /// <param name="button">Сама кнопка</param>
        /// <param name="model">Модель параметров для кнопки</param>
        /// <param name="clickAction">Метод, который срабатывает при нажатии</param>
        /// <param name="changeButtonParams">Метод, который срабатывает при инициализации кнопки</param>
        /// <typeparam name="K">Шаблон ключа</typeparam>
        /// <typeparam name="V">Шаблон значений</typeparam>
        protected void InitializeItemView<K, V>(GameObject button, KeyValuePair<K, V> model,
            UnityAction<V> clickAction, UnityAction<TestItemView, V> changeButtonParams)
        {
            //Находим в кнопке компонент Button
            TestItemView buttonComponent = new TestItemView(button);
            //Меняем параметры кнопки при инициализации
            changeButtonParams(buttonComponent, model.Value);
            //Передаем метод, который сработает при нажатии на кнопку
            buttonComponent.clickButton.onClick.AddListener(() => clickAction(model.Value));
        }

        /// <summary>
        /// Конструктор для обработки кнопок
        /// </summary>
        public class TestItemView
        {
            public Button clickButton;
            public TestItemView(GameObject view)
            {
                clickButton = view.GetComponent<Button>();
            }
        }
    }
}
