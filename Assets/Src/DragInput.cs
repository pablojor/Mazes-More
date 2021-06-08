using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazesAndMore
{
    public class DragInput : MonoBehaviour
    {
        /// <summary>
        /// controla si el raton esta pulsado o no
        /// </summary>
        private bool _mousePressed = false;
        /// <summary>
        /// posicion del raton
        /// </summary>
        private Vector2 _mousePosition;
        /// <summary>
        /// direccion del scroll
        /// </summary>
        private Vector3 _velocity;
        /// <summary>
        /// velocidad a la que se movera el scroll
        /// </summary>
        private float _speed = 10;
        /// <summary>
        /// alto de una fila
        /// </summary>
        private float _rowHeight = 0;
        /// <summary>
        /// numero de filas que hay en el scroll
        /// </summary>
        private int _numRows;

        /// <summary>
        /// controla el scroll, detectando cuando hay que realizarlo y cuando parar
        /// </summary>
        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                if (!_mousePressed)
                {
                    _mousePosition = Input.mousePosition;
                    _mousePressed = true;
                }
                else
                {
                    Vector2 moveDir;

                    moveDir = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - _mousePosition;
                    _mousePosition = Input.mousePosition;

                    transform.localPosition += new Vector3(0, moveDir.y, 0);
                }
            }
            else
            {
                if (_mousePressed)
                {
                    _velocity = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - _mousePosition;
                    _velocity.x = 0;
                    _speed = 10;
                    _mousePressed = false;
                }
                transform.localPosition += _velocity * _speed * Time.deltaTime;
                _speed -= Time.deltaTime * _speed;
                if (_speed < 0.5)
                    _speed = 0;

            }
            if (transform.localPosition.y < 0)
                transform.localPosition = new Vector3(0, 0, 0);
            if (transform.localPosition.y > (float)_numRows * _rowHeight)
                transform.localPosition = new Vector3(0, (float)_numRows * _rowHeight, 0);

        }

        /// <summary>
        /// establece el numero de filas
        /// </summary>
        /// <param name="rows">numero de filas</param>
        public void SetNumLevels(int rows)
        {
            _numRows = rows;
            if (_numRows < 0)
                _numRows = 0;
        }

        /// <summary>
        /// estabelce el alto de una fila
        /// </summary>
        /// <param name="height">alto de una fila</param>
        public void SetRowHeight(float height)
        {
            _rowHeight = height;
        }
    }

}