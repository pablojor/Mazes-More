using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazesAndMore
{
    public class PopUpMenusController : MonoBehaviour
    {
        [Tooltip("Array de los botones que abren menus PopUp")]
        public ButtonObject[] standartButtons;

        /// <summary>
        /// inicializa el array de los bootnes pasandoles el MenuManager
        /// </summary>
        /// <param name="menuManager"></param>
        public void Init(MenuManager menuManager)
        {
            foreach (ButtonObject obj in standartButtons)
                obj.Init(menuManager);
        }

        /// <summary>
        /// activa o desactiva este objeto
        /// </summary>
        /// <param name="active">activado o desactivado</param>
        public void SetActive(bool active)
        {
            this.gameObject.SetActive(active);
        }
    }
}