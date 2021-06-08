using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MazesAndMore
{
    public class ButtonStartLevel : ButtonObject
    {
        [Tooltip("Texto del boton")]
        public TextController myText;
        [Tooltip("RectTransform del boton")]
        public RectTransform myRect;
        [Tooltip("Imagen del boton")]
        public Image myImage;

        /// <summary>
        /// numero del nivel que se va a cargar
        /// </summary>
        private int _levelToLoad;
        /// <summary>
        /// indice del lote de niveles que se van a cargar
        /// </summary>
        private int _packageNumber;

        /// <summary>
        /// metodo que se llama en onCLick()
        /// cambia a la escena de juego y dice que se carge el nivel de este boton
        /// </summary>
        public void StartLevel()
        {
            GameManager.GetInstance().SetLevelToLoad(_levelToLoad, _packageNumber);
            GameManager.GetInstance().ChangeScene("Game");
        }

        /// <summary>
        /// establece el numero del nivel de este boton
        /// </summary>
        /// <param name="lvl">nivel</param>
        public void SetLevel(int lvl)
        {
            _levelToLoad = lvl;
        }

        /// <summary>
        /// establece el indice del paquete de este boton
        /// </summary>
        /// <param name="packageNumber">indice del paquete</param>
        public void SetPackageNumber(int packageNumber)
        {
            _packageNumber = packageNumber;
        }

        /// <summary>
        /// cambia el texto del boton para escribir el numero del nivel
        /// </summary>
        /// <param name="num"></param>
        public void ChangeText(int num)
        {
            myText.ChangeText((num).ToString());
        }

        /// <summary>
        /// cambia el RectTransform del boton
        /// </summary>
        /// <param name="x">nueva x</param>
        /// <param name="y">nueva y</param>
        public void SetRect(float x, float y)
        {
            myRect.localPosition = new Vector2(x, y);
        }

        /// <summary>
        /// cambia el color de la imagen del boton
        /// </summary>
        /// <param name="color"></param>
        public void SetColorUnlocked(Color color)
        {
            myImage.color = color;
            myText.ChangeColor(Color.white);
        }
    }
}