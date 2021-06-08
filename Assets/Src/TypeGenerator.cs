using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazesAndMore
{
    public class TypeGenerator : MonoBehaviour
    {
        [Tooltip("Canvas en el que se van a generar los niveles")]
        public GameObject canvasLevels;
        [Tooltip("Canvas en el que se van a generar los botones de los tipos")]
        public GameObject myCanvas;
        [Tooltip("Instancia del scroll del canvas de los niveles")]
        public DragInput buttonsHolder;
        [Tooltip("Instancia del scroll")]
        public DragInput myButtonsHolder;
        [Tooltip("Texto que se va a cambiar despues por el nombre del tipo del nivel")]
        public TextController nextText;
        [Tooltip("Script de los botones que generan los botones de niveles")]
        public GenerateLevelButtons button;

        /// <summary>
        /// incializa el generador, creando los botones pertinentes
        /// </summary>
        void Start()
        {
            LevelPackage[] packages = GameManager.GetInstance().GetPackages();

            float x, y;
            for (int i = 0; i < packages.Length; i++)
            {
                x = 0;
                y = 140 + (-75 * i);
                GenerateLevelButtons aux = Instantiate(button, myButtonsHolder.transform);

                aux.Init(canvasLevels, myCanvas, buttonsHolder, packages[i].name, packages[i].buttonImage, packages[i].buttonImagePressed, packages[i].packageNumber, packages[i].packageColor, nextText, packages[i].levels.Length);
                aux.SetRect(x, y);
            }
            myButtonsHolder.SetNumLevels(packages.Length - 5);
            myButtonsHolder.SetRowHeight(75);
        }

    }
}