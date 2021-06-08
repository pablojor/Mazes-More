using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MazesAndMore
{
    public class GameManager : MonoBehaviour
    {
        [Tooltip("Script del Level Manager")]
        public LevelManager levelManger;
        [Tooltip("Array de los paquetes de niveles que hay")]
        public LevelPackage[] packages;

        /// <summary>
        /// Script del Progress Manager
        /// </summary>
        private ProgessManager _progessManager;
        /// <summary>
        /// nivel actual
        /// </summary>
        private int _levelToLoad;
        /// <summary>
        /// paquete actual de niveles
        /// </summary>
        private int _package;

        /// <summary>
        /// instancia del propio Game Manager
        /// </summary>
        private static GameManager _instance;

        /// <summary>
        /// comprueba si ya existe. Si es asi, se destruye a el mismo
        /// si la escena actual tiene Level Manager, lo guarda y carga el nivel actual
        /// </summary>
        private void Awake()
        {
            if (_instance != null)
            {
                _instance.levelManger = levelManger;
                if (_instance.levelManger != null)
                    _instance.StartNewScene();

                DestroyImmediate(this.gameObject);
                return;
            }
            _progessManager = new ProgessManager();
            _progessManager.ReadProgress();
            _instance = this;
            DontDestroyOnLoad(this);
        }

        /// <summary>
        /// si el Level Manager existe, se inicializa y carga el nuevo nivel
        /// </summary>
        private void StartNewScene()
        {
            if (levelManger != null)
            {
                levelManger.Init();
                levelManger.LoadMap(_levelToLoad, packages[_package]);
            }
        }

        /// <summary>
        /// devuelve su instancia
        /// </summary>
        /// <returns>GameManager si existe, sino null</returns>
        public static GameManager GetInstance()
        {
            if (_instance != null)
            {
                return _instance;
            }
            else
                return null;
        }

        /// <summary>
        /// cambia al escena por la especificada
        /// </summary>
        /// <param name="scene">escena por la que se cambia</param>
        public void ChangeScene(string scene)
        {
            SceneManager.LoadScene(scene);
        }

        /// <summary>
        /// especifica el nivel que se va a cargar y el paquete de niveles
        /// </summary>
        /// <param name="lvl">nivel</param>
        /// <param name="package">paquete de niveles</param>
        public void SetLevelToLoad(int lvl, int package)
        {
            _levelToLoad = lvl;
            _package = package;
        }

        /// <summary>
        /// cambia al siguiente nivel
        /// </summary>
        /// <param name="lastLevel">numero del nivel actual, no el siguiente</param>
        public void NextLevel(int lastLevel)
        {
            if (lastLevel + 1 < packages[_package].levels.Length)
                levelManger.LoadMap(lastLevel + 1, packages[_package]);
            else
                ChangeScene("Menu");
        }

        /// <summary>
        /// devuelve el progreso del jugador
        /// </summary>
        /// <returns>progreso del jugador</returns>
        public ProgessManager.Save GetProgess()
        {
            return _progessManager.GetProgess();
        }

        /// <summary>
        /// actualiza al informacion del progreso del jugador
        /// </summary>
        /// <param name="newSave"></param>
        public void UpdateProgess(ProgessManager.Save newSave)
        {
            _progessManager.UpdateProgess(newSave);
        }

        /// <summary>
        /// devuelve el array de los paquetes de niveles que tiene
        /// </summary>
        /// <returns>array de paquetes de niveles</returns>
        public LevelPackage[] GetPackages()
        {
            return packages;
        }
    }

}