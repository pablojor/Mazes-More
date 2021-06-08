using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MazesAndMore
{
    public class TextController : MonoBehaviour
    {
        [Tooltip("Texto que se va a modificar")]
        public Text text;

        /// <summary>
        /// cambia el texto por el nuevo
        /// </summary>
        /// <param name="tx">nuevo texto</param>
        public void ChangeText(string tx)
        {
            text.text = tx;
        }

        /// <summary>
        /// cambia el color del texto por el nuevo
        /// </summary>
        /// <param name="co">nuevo color del texto</param>
        public void ChangeColor(Color co)
        {
            text.color = co;
        }
    }
}