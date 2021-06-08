using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MazesAndMore
{
    public class ButtonNextLevel : ButtonObject
    {
        /// <summary>
        /// metodo que se llama en onClick()
        /// le dice al MenuManager que pasamos al siguiente nivel
        /// </summary>
        public void NextLevel()
        {
            _menuManager.NextLevel();
        }
    }
}