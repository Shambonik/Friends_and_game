using System;
using System.Collections.Generic;
using System.Linq;
using _Games.Scripts.Netwotking;
using AureoleCore.Models;
using AureoleCore.Responses;
using AureoleCore.UserAPI;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ButtonGeneration
{
    /// <summary>
    /// Генерация всех игровых героев игрока, которые хранятся в базе данных
    /// </summary>
    public class UICharacterGeneration : MonoBehaviour
    {
        //Текстовое поле для вывода имени персонажа
        public Dictionary<long, UnitResponseData> units;
        [SerializeField] private Button button;
        [SerializeField] private GameObject CreateCharacter;

        /// <summary>
        /// Генерируем всех игровых героев
        /// </summary>
        public void getUnits()
        {
            /*units = UserAPI.GetUserUnits(UIMainMenu.sessingId).result
                .ToDictionary(unitResponseData => unitResponseData.unit.UnitId, unitResponseData => unitResponseData);
            if (units.Count > 0)
            {
                Debug.Log(button.GetComponentInChildren<Text>().text);
                button.GetComponentInChildren<Text>().text = units[1].unit.UnitName;
            }*/
        }

        /// <summary>
        /// Изменение кнопок игровых героев
        /// </summary>
        /// <param name="buttonModel">Модель кнопки</param>
        /// <param name="unitModel">Параметры героя</param>\
        ///// <summary>
        ///// Метод, который запоминает id выбранного перса
        ///// </summary>
        ///// <param name="model"></param>
        public void ClickAction()
        {
            //Передаем id выбранного героя
            if(units.Count>0){
                GameManager.GetGameManager().UnitId = units[1].unit.UnitId;
                FindObjectOfType<UIMainMenu>().FindGame();
            }
        }

        public void createOnClick()
        {
            CreateCharacter.SetActive(true);
            transform.gameObject.SetActive(false);
        }
    }
}
