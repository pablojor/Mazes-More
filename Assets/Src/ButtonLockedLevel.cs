using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MazesAndMore
{
    public class ButtonLockedLevel : ButtonObject
    {
        [Tooltip("RectTransform que se va a modificar")]
        public RectTransform myRect;

        /// <summary>
        /// cambiar el rect del objeto
        /// </summary>
        /// <param name="x"> nueva x </param>
        /// <param name="y"> nueva y </param>
        public void SetRect(float x, float y)
        {
            myRect.localPosition = new Vector2(x, y);
        }
    }
}