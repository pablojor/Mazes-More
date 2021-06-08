using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazesAndMore
{
    /// <summary>
    /// clase padre de todos los botones
    /// </summary>
    public class ButtonObject : MonoBehaviour
    {
        /// <summary>
        /// Script que maneja el comportamiento de la UI con el resto de objetos
        /// </summary>
        protected MenuManager _menuManager;

        /// <summary>
        /// inicializar el boton, estableciendo el MenuManager
        /// </summary>
        /// <param name="menuManager"></param>
        public void Init(MenuManager menuManager)
        {
            _menuManager = menuManager;
        }
    }
}