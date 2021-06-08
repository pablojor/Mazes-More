using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazesAndMore
{
    public class Tile : MonoBehaviour
    {
        [Tooltip("Sprite que indica que la celda es de hielo")]
        public SpriteRenderer iceFloor;
        [Tooltip("Sprite que indica que la celda es de objetivo")]
        public SpriteRenderer goal;

        [Tooltip("Sprite que indica la pared de abajo")]
        public SpriteRenderer down_wall;
        [Tooltip("Sprite que indica la pared de arriba")]
        public SpriteRenderer top_wall;
        [Tooltip("Sprite que indica la pared de derecha")]
        public SpriteRenderer right_wall;
        [Tooltip("Sprite que indica la pared de izquierda")]

        public SpriteRenderer left_wall;
        [Tooltip("Sprite que indica el trazo derecho del jugador")]
        public SpriteRenderer character_rightTrace;
        [Tooltip("Sprite que indica el trazo izquierdo del jugador")]
        public SpriteRenderer character_leftTrace;
        [Tooltip("Sprite que indica el trazo de arrbia del jugador")]
        public SpriteRenderer character_upTrace;
        [Tooltip("Sprite que indica el trazo de abajo del jugador")]
        public SpriteRenderer character_downTrace;

        [Tooltip("Sprite que indica el trazo derecho del hint")]
        public SpriteRenderer hint_rightTrace;
        [Tooltip("Sprite que indica el trazo izquierdo del hint")]
        public SpriteRenderer hint_leftTrace;
        [Tooltip("Sprite que indica el trazo de arrbia del hint")]
        public SpriteRenderer hint_upTrace;
        [Tooltip("Sprite que indica el trazo de abajo del hint")]
        public SpriteRenderer hint_downTrace;

        [Tooltip("Maximo ancho del trazo")]
        public float traceMaxWidht = 1.2f;
        [Tooltip("Minimo ancho del trazo")]
        public float traceMinWidht = 0.2f;
        [Tooltip("Velocidad a la que se va a realizar la animacion")]
        public float velocity = 3f;

        /// <summary>
        /// ratio del ancho de un trazo
        /// </summary>
        private float _ratioWidhtPos;
        /// <summary>
        /// booleanos que expresan si hay que desactivar un trazo o no
        /// </summary>
        private bool _disableLeft = false, _disableRight = false, _disableUp = false, _disableDown = false;
        /// <summary>
        /// controla la pausa de los trazos
        /// </summary>
        private bool _paused;

        /// <summary>
        /// inicializa cosas del tile
        /// mas especificamente, calcula el ratio del ancho de un trazo
        /// </summary>
        private void Start()
        {
            _ratioWidhtPos = 0.5f / (traceMaxWidht - traceMinWidht);
        }

        /// <summary>
        /// si no esta en apusa, comprueba y realiza las animaciones de cada trazo
        /// </summary>
        private void Update()
        {
            if (!_paused)
            {
                CheckHoriTrace(_disableRight, character_rightTrace, 0.5f, 1);
                CheckHoriTrace(_disableLeft, character_leftTrace, -0.5f, -1);
                CheckVerticalTrace(_disableUp, character_upTrace, 0.5f, 1);
                CheckVerticalTrace(_disableDown, character_downTrace, -0.5f, -1);
            }
        }

        /// <summary>
        /// realiza las animaciones de los trazos horizontales dependiendo de si estamos desactivandolos o activandolos
        /// </summary>
        /// <param name="disable">hay que desactivar o activar</param>
        /// <param name="sprite">sprite a modificar</param>
        /// <param name="limit">limite de posicion</param>
        /// <param name="ori">orientacion en la que mover el sprite</param>
        private void CheckHoriTrace(bool disable, SpriteRenderer sprite, float limit, int ori)
        {
            if (!disable && sprite.enabled && sprite.size.x < traceMaxWidht)
            {
                sprite.size = new Vector2(sprite.size.x + (velocity * Time.deltaTime), sprite.size.y);
                if (sprite.size.x > traceMaxWidht)
                    sprite.size = new Vector2(traceMaxWidht, sprite.size.y);
                if (Mathf.Abs(sprite.transform.localPosition.x) < Mathf.Abs(limit))
                {
                    sprite.transform.localPosition = new Vector3(sprite.transform.localPosition.x + (velocity * _ratioWidhtPos * Time.deltaTime) * ori, sprite.transform.localPosition.y, 0);
                    if (Mathf.Abs(sprite.transform.localPosition.x) >= Mathf.Abs(limit))
                        sprite.transform.localPosition = new Vector3(limit, sprite.transform.localPosition.y, 0);
                }
            }

            else if (disable)
            {
                sprite.size = new Vector2(sprite.size.x - (velocity * Time.deltaTime), sprite.size.y);
                if (sprite.size.x < traceMinWidht)
                    sprite.size = new Vector2(traceMinWidht, sprite.size.y);
                if (Mathf.Abs(sprite.transform.localPosition.x) > 0.0)
                {
                    sprite.transform.localPosition = new Vector3(sprite.transform.localPosition.x - (velocity * _ratioWidhtPos * Time.deltaTime) * ori, sprite.transform.localPosition.y, 0);
                    if ((ori == 1 && sprite.transform.localPosition.x <= 0.0) || (ori == -1 && sprite.transform.localPosition.x >= 0.0))
                    {
                        sprite.transform.localPosition = new Vector3(0.0f, sprite.transform.localPosition.y, 0);
                        if (ori == 1)
                            _disableRight = false;
                        else
                            _disableLeft = false;
                        sprite.enabled = false;
                    }
                }
            }
        }

        /// <summary>
        /// realiza las animaciones de los trazos verticales dependiendo de si estamos desactivandolos o activandolos
        /// </summary>
        /// <param name="disable">hay que desactivar o activar</param>
        /// <param name="sprite">sprite a modificar</param>
        /// <param name="limit">limite de posicion</param>
        /// <param name="ori">orientacion en la que mover el sprite</param>
        private void CheckVerticalTrace(bool disable, SpriteRenderer sprite, float limit, int ori)
        {
            if (!disable && sprite.enabled && sprite.size.x < traceMaxWidht)
            {
                sprite.size = new Vector2(sprite.size.x + (velocity * Time.deltaTime), sprite.size.y);
                if (sprite.size.x > traceMaxWidht)
                    sprite.size = new Vector2(traceMaxWidht, sprite.size.y);
                if (Mathf.Abs(sprite.transform.localPosition.y) < Mathf.Abs(limit))
                {
                    sprite.transform.localPosition = new Vector3(sprite.transform.localPosition.x, sprite.transform.localPosition.y + (velocity * _ratioWidhtPos * Time.deltaTime) * ori, 0);
                    if (Mathf.Abs(sprite.transform.localPosition.y) >= Mathf.Abs(limit))
                        sprite.transform.localPosition = new Vector3(sprite.transform.localPosition.x, limit, 0);
                }
            }

            else if (disable)
            {
                sprite.size = new Vector2(sprite.size.x - (velocity * Time.deltaTime), sprite.size.y);
                if (sprite.size.x < traceMinWidht)
                    sprite.size = new Vector2(traceMinWidht, sprite.size.y);
                if (Mathf.Abs(sprite.transform.localPosition.y) > 0.0)
                {
                    sprite.transform.localPosition = new Vector3(sprite.transform.localPosition.x, sprite.transform.localPosition.y - (velocity * _ratioWidhtPos * Time.deltaTime) * ori, 0);
                    if ((ori == 1 && sprite.transform.localPosition.y <= 0.0) || (ori == -1 && sprite.transform.localPosition.y >= 0.0))
                    {
                        sprite.transform.localPosition = new Vector3(sprite.transform.localPosition.x, 0.0f, 0);
                        if (ori == 1)
                            _disableUp = false;
                        else
                            _disableDown = false;
                        sprite.enabled = false;
                    }
                }
            }
        }

        /// <summary>
        /// estabelce el color de los trazos
        /// </summary>
        /// <param name="color">color de los trazos</param>
        public void SetTracesColor(Color color)
        {
            character_rightTrace.color = color;
            character_leftTrace.color = color;
            character_upTrace.color = color;
            character_downTrace.color = color;
            goal.color = color;
        }

        /// <summary>
        /// activa el sprite de hielo
        /// </summary>
        public void EnableIceFloor()
        {
            iceFloor.enabled = true;
        }

        /// <summary>
        /// activa el sprite del final
        /// </summary>
        public void EnableGoal()
        {
            goal.enabled = true;
        }

        /// <summary>
        /// activa los sprites de las paredes dependiendo de la informacion que se le pase
        /// normalmente solo se activan las paredes de la izquierda y de arriba salvo cuando se especifique
        /// </summary>
        /// <param name="cell">contiene los booleanos que dicen que lados debe tener paredes y cuales no</param>
        /// <param name="checkRight">expresa si tenemos que poner la pared de la derecha</param>
        /// <param name="checkDown">expresa si tenemos que poner la pared de abajo</param>
        public void EnableWalls(Map.Cell cell, bool checkRight, bool checkDown)
        {
            if (cell.up == false)
                top_wall.enabled = true;
            if (cell.left == false)
                left_wall.enabled = true;

            if (checkDown && cell.down == false)
                down_wall.enabled = true;
            if (checkRight && cell.right == false)
                right_wall.enabled = true;

        }

        /// <summary>
        /// activa el trazo de pista de la derecha
        /// </summary>
        public void EnableHintRightTrace()
        {
            hint_rightTrace.enabled = true;
        }
        /// <summary>
        /// activa el trazo de pista de la izquierda
        /// </summary>
        public void EnableHintLeftTrace()
        {
            hint_leftTrace.enabled = true;
        }
        /// <summary>
        /// activa el trazo de pista de arriba
        /// </summary>
        public void EnableHintUpTrace()
        {
            hint_upTrace.enabled = true;
        }
        /// <summary>
        /// activa el trazo de pista de abajo
        /// </summary>
        public void EnableHintDownTrace()
        {
            hint_downTrace.enabled = true;
        }

        /// <summary>
        /// activa el trazo del jugador de la derecha
        /// </summary>
        public void EnableCharacterRightTrace()
        {
            character_rightTrace.enabled = true;
        }
        /// <summary>
        /// avisa de que hay que desactivar el trazo del jugador de la derecha
        /// </summary>
        public void DisableCharacterRightTrace()
        {
            _disableRight = true;
        }
        /// <summary>
        /// activa el trazo del jugador de la izquierda
        /// </summary>
        public void EnableCharacterLeftTrace()
        {
            character_leftTrace.enabled = true;
        }
        /// <summary>
        /// avisa de que hay que desactivar el trazo del jugador de la izquierda
        /// </summary>
        public void DisableCharacterLeftTrace()
        {
            _disableLeft = true;
        }
        /// <summary>
        /// activa el trazo del jugador de arriba
        /// </summary>
        public void EnableCharacterUpTrace()
        {
            character_upTrace.enabled = true;
        }
        /// <summary>
        /// avisa de que hay que desactivar el trazo del jugador de arriba
        /// </summary>
        public void DisableCharacterUpTrace()
        {
            _disableUp = true;
        }
        /// <summary>
        /// activa el trazo del jugador de abajo
        /// </summary>
        public void EnableCharacterDownTrace()
        {
            character_downTrace.enabled = true;
        }
        /// <summary>
        /// avisa de que hay que desactivar el trazo del jugador de abajo
        /// </summary>
        public void DisableCharacterDownTrace()
        {
            _disableDown = true;
        }

        /// <summary>
        /// establece que hay que estamos en pausa
        /// </summary>
        /// <param name="paused">pausa o no</param>
        public void SetPause(bool paused)
        {
            _paused = paused;
        }

        /// <summary>
        /// reinica los trazos
        /// </summary>
        public void RestartLevel()
        {
            character_rightTrace.enabled = false;
            character_rightTrace.size = new Vector2(traceMinWidht, traceMinWidht);
            character_rightTrace.transform.localPosition = new Vector3(0.0f, character_rightTrace.transform.localPosition.y, 0);
            _disableRight = false;

            character_leftTrace.enabled = false;
            character_leftTrace.size = new Vector2(traceMinWidht, traceMinWidht);
            character_leftTrace.transform.localPosition = new Vector3(0.0f, character_leftTrace.transform.localPosition.y, 0);
            _disableLeft = false;

            character_upTrace.enabled = false;
            character_upTrace.size = new Vector2(traceMinWidht, traceMinWidht);
            character_upTrace.transform.localPosition = new Vector3(character_upTrace.transform.localPosition.x, 0.0f, 0);
            _disableUp = false;

            character_downTrace.enabled = false;
            character_downTrace.size = new Vector2(traceMinWidht, traceMinWidht);
            character_downTrace.transform.localPosition = new Vector3(character_downTrace.transform.localPosition.x, 0.0f, 0);
            _disableDown = false;

            hint_rightTrace.enabled = false;
            hint_leftTrace.enabled = false;
            hint_upTrace.enabled = false;
            hint_downTrace.enabled = false;
        }

        /// <summary>
        /// muestra los sprites del tile o no
        /// </summary>
        /// <param name="show">mostrar o no mostrar</param>
        public void Show(bool show)
        {
            this.gameObject.SetActive(show);
        }
    }
}