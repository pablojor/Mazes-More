using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace MazesAndMore
{
    public class ButtonPopUpMenu : ButtonObject
    {
        [Tooltip("Menu Pop Up que se va a activar o no")]
        public PopUpMenusController menu;
        [Tooltip("Referencia a mi boton, para desactivarlo cuando estemos en pausa")]
        public Button myButton;
        [Tooltip("Bool que expresa si se activa el menu Pop Up o no")]
        public bool activate;

        /// <summary>
        /// metodo que se llama en onClick()
        /// activa el menu Pop Up
        /// </summary>
        public void PopUp()
        {
            menu.SetActive(activate);
            _menuManager.SetPause(activate);
        }

        /// <summary>
        /// inactiva el boton cuando estamos en pausa
        /// </summary>
        /// <param name="paused"></param>
        public void SetPause(bool paused)
        {
            myButton.enabled = !paused;
            if (!paused)
                menu.SetActive(paused);
        }
    }
}