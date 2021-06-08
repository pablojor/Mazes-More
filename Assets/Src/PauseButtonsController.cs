using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazesAndMore
{
    public class PauseButtonsController : MonoBehaviour
    {
        [Tooltip("Array de botones que deben ser desactivados al pausar el juego")]
        public ButtonPopUpMenu[] pauseButtons;

        /// <summary>
        /// inicializa los botones del array pasandoles el MenuManager
        /// </summary>
        /// <param name="menuManager"></param>
        public void Init(MenuManager menuManager)
        {
            foreach (ButtonPopUpMenu obj in pauseButtons)
                obj.Init(menuManager);
        }

        /// <summary>
        /// desactiva o activa los botones del array
        /// </summary>
        /// <param name="paused">pausado o no pausado</param>
        public void SetPause(bool paused)
        {
            foreach (ButtonPopUpMenu obj in pauseButtons)
                obj.SetPause(paused);
        }
    }
}