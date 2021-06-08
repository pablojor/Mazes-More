using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazesAndMore
{
    public class ButtonUseHint : ButtonObject
    {
        [Tooltip("Menu Pop Up que va a ocultarse")]
        public PopUpMenusController menu;

        /// <summary>
        /// metodo que se llama en onClick()
        /// usar una pista
        /// </summary>
        public void UseHint()
        {
            _menuManager.UseHint();
            menu.SetActive(false);
            _menuManager.SetPause(false);
        }
    }
}