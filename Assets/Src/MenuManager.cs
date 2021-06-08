using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MazesAndMore
{
    public class MenuManager : MonoBehaviour
    {
        [Tooltip("Controlador de los botones de pausa")]
        public PauseButtonsController pauseButtons;
        [Tooltip("Controlador de los botones que muestran los menus PopUp al final del juego")]
        public PopUpMenusController nextLevelMenu, backToNextLevel;
        [Tooltip("Botones que muestran los menus PopUp")]
        public PopUpMenusController[] menuButtons;
        [Tooltip("Canvas principal del juego")]
        public GameObject canvas;
        [Tooltip("Textos que debe modificar")]
        public TextController text, textHint, textHint2;

        /// <summary>
        /// instancia del LevelManager
        /// </summary>
        private LevelManager _levelManager;

        /// <summary>
        /// Inicializa los botones y algunos de los textos
        /// </summary>
        /// <param name="levelManager"></param>
        public void Init(LevelManager levelManager)
        {
            _levelManager = levelManager;
            pauseButtons.Init(this);

            nextLevelMenu.Init(this);
            backToNextLevel.Init(this);
            foreach (PopUpMenusController obj in menuButtons)
                obj.Init(this);

            ProgessManager.Save save = _levelManager.GetProgess();
            textHint.ChangeText ((save.h).ToString());
            textHint2.ChangeText((save.h).ToString());
        }

        /// <summary>
        /// avisa a los botones de pausa y al LevelManager de que el juego esta pausado
        /// </summary>
        /// <param name="paused">pausado o no</param>
        public void SetPause(bool paused)
        {

            pauseButtons.SetPause(paused);
            _levelManager.SetPause(paused);
        }

        /// <summary>
        /// avisa al LevelManager de que reincie el nivel
        /// </summary>
        public void RestartLevel()
        {
            _levelManager.RestartLevel();
        }

        /// <summary>
        /// pone en pausa los botones, desactiva el canvas principal y activa los botones del final
        /// </summary>
        public void LevelFinished()
        {
            SetPause(true);
            nextLevelMenu.SetActive(true);
            canvas.SetActive(false);
        }

        /// <summary>
        /// le dice al LevelManager que muestre el nivel terminado y oculta los botones del final
        /// </summary>
        /// <param name="show"></param>
        public void ShowLevelFinished(bool show)
        {
            nextLevelMenu.SetActive(!show);
            backToNextLevel.SetActive(show);
            canvas.SetActive(show);
            _levelManager.ShowLevelFinished(show);

        }

        /// <summary>
        /// avisa al LevelManager de que empiece el siguiente nivel y quita la pausa puesta anteriormente
        /// (ademas de ocultar los botones del final)
        /// </summary>
        public void NextLevel()
        {
            SetPause(false);
            nextLevelMenu.SetActive(false);
            canvas.SetActive(true);
            _levelManager.NextLevel();
        }

        /// <summary>
        /// establece el nombre del nivel
        /// </summary>
        /// <param name="name">nombre del tipo de nivel</param>
        /// <param name="number">numero del nivel</param>
        public void SetLevelName(string name, int number)
        {
            text.ChangeText(name+ " - " + (number + 1));
        }

        /// <summary>
        /// avisa al LevelManager de que utilizamos una pista y actualiza al información visual de las pistas
        /// </summary>
        public void UseHint()
        {
            _levelManager.UseHint();

            ProgessManager.Save save = _levelManager.GetProgess();
            textHint.ChangeText((save.h).ToString());
            textHint2.ChangeText((save.h).ToString());
        }
    }
}