using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace MazesAndMore
{
    public class GenerateLevelButtons : MonoBehaviour
    {
        [Tooltip("Script de los botones que empiezan niveles")]
        public ButtonStartLevel button;
        [Tooltip("Script de los botones bloqueados")]
        public ButtonLockedLevel buttonLocked;
        [Tooltip("Paquete de niveles de este boton")]
        public LevelPackage levels;
        [Tooltip("Script del texto del tipo")]
        public TextController myText;
        [Tooltip("Script del texto del porcentaje")]
        public TextController percentageText;
        [Tooltip("RectTransform del boton a generar (mi boton)")]
        public RectTransform myRect;
        [Tooltip("Boton que realiza la accion (mi boton)")]
        public Button myButton;

        /// <summary>
        /// texto que se va a modificar con el nombre del tipo
        /// (para reciclar)
        /// </summary>
        private TextController _nextText;
        /// <summary>
        /// canvas en el que se crearan los botones
        /// </summary>
        private GameObject _canvasLevels;
        /// <summary>
        /// el canvas de este boton
        /// </summary>
        private GameObject _myCanvas;
        /// <summary>
        /// Scroll de la pantalla
        /// </summary>
        private DragInput _buttonsHolder;
        /// <summary>
        /// nombre este boton (nombre del tipo)
        /// </summary>
        private string _name;
        /// <summary>
        /// numero del paquete que controla este boton
        /// </summary>
        private int _packageNumber;
        /// <summary>
        /// color de este tipo de niveles
        /// </summary>
        private Color _color;
        /// <summary>
        /// numero del nivel hasta el que ha llegado el jugador en este paquete
        /// </summary>
        private int _limit;
        /// <summary>
        /// numero de niveles de este paquete
        /// </summary>
        private int _lenght;

        private int _separation = 45, _xOffset = -90, _yOffset = 140;

        /// <summary>
        /// inicializa este Script segun la informacion pasada.
        /// </summary>
        /// <param name="canvasLevels"></param>
        /// <param name="myCanvas"></param>
        /// <param name="buttonsHolder"></param>
        /// <param name="name"></param>
        /// <param name="image"></param>
        /// <param name="pressedImage"></param>
        /// <param name="package"></param>
        /// <param name="color"></param>
        /// <param name="nextText"></param>
        public void Init(GameObject canvasLevels, GameObject myCanvas, DragInput buttonsHolder, string name, Sprite image, Sprite pressedImage, int package, Color color, TextController nextText, int limit)
        {
            _canvasLevels = canvasLevels;
            _myCanvas = myCanvas;
            _buttonsHolder = buttonsHolder;

            SpriteState spriteState = new SpriteState();
            spriteState = myButton.spriteState;
            spriteState.pressedSprite = pressedImage;
            myButton.image.sprite = image;
            myButton.spriteState = spriteState;

            _nextText = nextText;
            _color = color;
            _packageNumber = package;
            _name = name;
            myText.ChangeText(_name);

            _lenght = limit;
            ProgessManager.Save save = GameManager.GetInstance().GetProgess();
            
            _limit = name == "Clasico"? save.c : save.i;
            percentageText.ChangeText(((int)(((float)_limit / (float)_lenght) * 100)).ToString() + "%");
        }

        /// <summary>
        /// metodo que se llama en el onClick()
        /// genera los botones de los niveles de este paquete segun el avance del jugador
        /// </summary>
        public void Generate()
        {
            _myCanvas.SetActive(false);
            _canvasLevels.SetActive(true);
            _nextText.ChangeText(_name);

            int i = 0;
            float x, y;

            for (; i < _limit; i++)
            {
                x = _xOffset + (_separation * (i % 5));
                y = _yOffset + (-_separation * (i / 5));
                ButtonStartLevel aux1 = Instantiate(button, _buttonsHolder.transform);

                aux1.SetRect(x, y);
                aux1.SetLevel(i);
                aux1.SetPackageNumber(_packageNumber);
                aux1.ChangeText(i + 1);
                aux1.SetColorUnlocked(_color);
            }

            x = _xOffset + (_separation * (i % 5));
            y = _yOffset + (-_separation * (i / 5));
            ButtonStartLevel aux = Instantiate(button, _buttonsHolder.transform);

            aux.SetRect(x, y);
            aux.SetLevel(i);
            aux.SetPackageNumber(_packageNumber);
            aux.ChangeText(i + 1);
            i++;

            for (; i < _lenght; i++)
            {
                x = _xOffset + (_separation * (i % 5));
                y = _yOffset + (-_separation * (i / 5));
                ButtonLockedLevel aux1 = Instantiate(buttonLocked, _buttonsHolder.transform);

                aux1.SetRect(x, y);
            }

            _buttonsHolder.SetNumLevels((_lenght / 5) - 8);
            _buttonsHolder.SetRowHeight(45f);
        }

        /// <summary>
        /// cambia la posicion del boton
        /// </summary>
        /// <param name="x">nuevo x</param>
        /// <param name="y">nuevo y</param>
        public void SetRect(float x, float y)
        {
            myRect.localPosition = new Vector2(x, y);
        }
    }
}