using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazesAndMore
{
    public class ButtonShowLevelFinished : ButtonObject
    {
        [Tooltip("Bool que dice si el boton mostrara o no el nivel terminado")]
        public bool show;

        /// <summary>
        /// metodo que se llama en onClick()
        /// le dice que al MenuManager que muestre o no el nivel finalizado
        /// </summary>
        public void Show()
        {
            _menuManager.ShowLevelFinished(show);
        }
    }
}