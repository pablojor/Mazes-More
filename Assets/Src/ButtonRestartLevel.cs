using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazesAndMore
{
    public class ButtonRestartLevel : ButtonObject
    {
        /// <summary>
        /// metodo que se llama en onClick()
        /// le dice al MenuManager que reinicie el nivel
        /// </summary>
        public void Restart()
        {
            _menuManager.RestartLevel();
            _menuManager.SetPause(false);
        }
    }
}