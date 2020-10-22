
using System.Collections.Generic;
using _Games.Scripts.Netwotking;
using AureoleCore.Models;
using AureoleCore.UnitAPI;
using AureoleCore.UserAPI;
using UI.ButtonGeneration;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Изменение старого персонажа
    /// </summary>
    public class UICreateCharacterPanel : MonoBehaviour
    {
        //Позиция для спавна новой модели для персонажа (дальше будет использовать для выбора рас). 
        //Персонаж спавнится после выбора модели в выпадающем списке.
        //Выпадащий список предлагаемых моделей (дальше будут расы)
        [SerializeField] private GameObject _characterModelsPanel;
        //Массив моделей для спавна 
        //[SerializeField] private GameObject[] models;

        [SerializeField] private InputField unitName;

        [SerializeField] private GameObject createNewCharacterPanel;
        //Ссылаемся на GameManager

        /// <summary>
        /// Изменяет параметры выбранного персонажа
        /// </summary>
        public void CreateNewCharacter()
        {
            //Отправляем запрос на изменение
           var data = UnitAPI.CreateNewUnit(UIMainMenu.sessingId, "cetus", unitName.text);
           _characterModelsPanel.SetActive(true);
           _characterModelsPanel.GetComponent<UICharacterGeneration>().getUnits();
           createNewCharacterPanel.SetActive(false);
            ////Обновляем информацию о пользователе
        }
    }
}
