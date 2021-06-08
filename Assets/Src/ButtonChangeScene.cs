using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MazesAndMore
{
    public class ButtonChangeScene : ButtonObject
    {

        [Tooltip("Escena a la que cambia el boton")]
        public string sceneToChange;

        /// <summary>
        /// metodo al que se llama en onClick()
        /// cambiar la escena actual por al especificada
        /// </summary>
        public void ChangeScene()
        {
            GameManager.GetInstance().ChangeScene(sceneToChange);
        }
    }
}