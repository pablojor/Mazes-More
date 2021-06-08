using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazesAndMore
{
    public class Arrow : MonoBehaviour
    {
        [Tooltip("Duracion de la animacion")]
        public float animTime = 0.5f;
        [Tooltip("Sprite de la flecha")]
        public SpriteRenderer arrow;
        [Tooltip("Cuanto cambiar el tamaño del sprite en la animacion")]
        public float tamToChange = 0.2f;

        /// <summary>
        /// variables para controlar la animacion
        /// </summary>
        private bool _growing = true;
        private float _timer = 0.0f;

        /// <summary>
        /// si la flecha esta activa, actualizamos su animacion
        /// </summary>
        void Update()
        {
            if (arrow.enabled)
            {
                if (_timer >= animTime)
                {
                    _growing = !_growing;
                    _timer = 0.0f;
                }
                else
                {
                    if (_growing)
                        arrow.transform.localScale = new Vector3(arrow.transform.localScale.x + (tamToChange / animTime * Time.deltaTime), arrow.transform.localScale.y + (tamToChange / animTime * Time.deltaTime),
                                                                        arrow.transform.localScale.z);
                    else
                        arrow.transform.localScale = new Vector3(arrow.transform.localScale.x - (tamToChange / animTime * Time.deltaTime), arrow.transform.localScale.y - (tamToChange / animTime * Time.deltaTime),
                                                                        arrow.transform.localScale.z);
                    _timer += Time.deltaTime;
                }
            }
        }

        /// <summary>
        /// cambiar color de la flecha
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(Color color)
        {
            arrow.color = color;
        }

        /// <summary>
        /// activar la flecha
        /// </summary>
        public void SetEnable()
        {
            arrow.enabled = true;
        }

        /// <summary>
        /// desactivar la flecha
        /// </summary>
        public void SetDisable()
        {
            arrow.enabled = false;
        }
    }
}
