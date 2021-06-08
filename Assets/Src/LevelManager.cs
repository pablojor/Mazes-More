using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazesAndMore
{
    public class LevelManager : MonoBehaviour
    {
        [Tooltip("Script que maneja el BoardManager")]
        public BoardManager boardManager;
        [Tooltip("Script que maneja el MenuManager")]
        public MenuManager menuManager;

        /// <summary>
        /// nivel actual de la partida
        /// </summary>
        private int _currentLevel;
        /// <summary>
        /// paquete de niveles que se usa
        /// </summary>
        private LevelPackage _levelPackage;

        /// <summary>
        /// inicializa el BoardManager y el MenuManager pasandoles el mismo
        /// </summary>
        public void Init()
        {
            boardManager.Init(this);
            menuManager.Init(this);
        }

        /// <summary>
        /// carga el nivel especificado, diciendoselo a quienes necesiten saberlo
        /// </summary>
        /// <param name="mapNumber">nivel a cargar</param>
        /// <param name="package">paquete que se va a usar</param>
        public void LoadMap(int mapNumber, LevelPackage package)
        {
            _currentLevel = mapNumber;
            _levelPackage = package;
            Map map = new Map();
            map.Init(JSONMap.FromJson(_levelPackage.levels[_currentLevel].ToString()));

            boardManager.SetMap(map, package.packageColor);
            menuManager.SetLevelName(_levelPackage.name, mapNumber);
        }

        /// <summary>
        /// le dice al BoardManager que estamos en pausa o no
        /// </summary>
        /// <param name="paused">pausados o no pausados</param>
        public void SetPause(bool paused)
        {
            boardManager.SetPause(paused);
        }

        /// <summary>
        /// le dice al BoardManager que vamos a reinciar el nivel
        /// </summary>
        public void RestartLevel()
        {
            boardManager.RestartLevel();
        }

        /// <summary>
        /// actualiza el progreso y le anuncia al MenuManager que el nivel se ha terminado
        /// </summary>
        public void LevelFinished()
        {
            ProgessManager.Save save = GameManager.GetInstance().GetProgess();

            if (_levelPackage.name == "Clasico" && _currentLevel == save.c)
                save.c++;
            else if (_levelPackage.name == "Piso De Hielo" && _currentLevel == save.i)
                save.i++;
            GameManager.GetInstance().UpdateProgess(save);
            menuManager.LevelFinished();
        }

        /// <summary>
        /// le dice al BoardManager que muestre o no el nivel
        /// </summary>
        /// <param name="show">mostrar o no mostrar</param>
        public void ShowLevelFinished(bool show)
        {
            boardManager.ShowLevel(show);
        }

        /// <summary>
        /// avisa al GameManager que vamos a pasar al siguiente nivel
        /// </summary>
        public void NextLevel()
        {
            GameManager.GetInstance().NextLevel(_currentLevel);
        }

        /// <summary>
        /// Usa una pista y actualiza la informacion del progreso
        /// </summary>
        public void UseHint()
        {
            ProgessManager.Save save = GameManager.GetInstance().GetProgess();
            save.h--;
            if (save.h >= 0 && boardManager.ShowHint())
            {
                GameManager.GetInstance().UpdateProgess(save);
            }
        }

        /// <summary>
        /// devuelve el progreso
        /// </summary>
        /// <returns></returns>
        public ProgessManager.Save GetProgess()
        {
            return GameManager.GetInstance().GetProgess();
        }
    }
}