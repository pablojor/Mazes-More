using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MazesAndMore
{
    public class CameraManager : MonoBehaviour
    {
        [Tooltip("Array de canvas que se van a reescalar")]
        public CanvasScaler[] myCanvas;
        [Tooltip("Camara de la escena")]
        public Camera camera;

        /// <summary>
        /// valor de escalado
        /// </summary>
        private float _scaleheight;

        /// <summary>
        /// metodos que realizan el reescalado
        /// </summary>
        private void Awake()
        {
            Rescale();
        }
        void Update()
        {
            Rescale();
        }

        /// <summary>
        /// reescala los canvas registrados segun el aspect ratio que queremos, 9/16
        /// </summary>
        private void Rescale()
        {
            float targetaspect = 9.0f / 16.0f;

            float windowaspect = (float)Screen.width / (float)Screen.height;

            _scaleheight = windowaspect / targetaspect;

            if (_scaleheight < 1.0f)
            {
                foreach (CanvasScaler can in myCanvas)
                    can.matchWidthOrHeight = 0;
            }
            else
            {
                foreach (CanvasScaler can in myCanvas)
                    can.matchWidthOrHeight = 1;
            }
        }

        /// <summary>
        /// cambia el tamaño de la camara
        /// </summary>
        /// <param name="newTam">nuevo tamaño</param>
        public void ChangeCameraSize(float newTam)
        {
            camera.orthographicSize = newTam;
        }

        /// <summary>
        /// Cambia el tamaño de la camara segun el ancho
        /// </summary>
        /// <param name="newTam">nuevo tamaño</param>
        public void ChangeCameraSizeWidht(float newTam)
        {
            float windowaspect = (float)Screen.width / (float)Screen.height;
            float camHalfHeight = newTam / windowaspect;

            camera.orthographicSize = camHalfHeight;
        }

        /// <summary>
        /// </summary>
        /// <returns>valor de escalado</returns>
        public float GetScaleHeight()
        {
            return _scaleheight;
        }

    }
}