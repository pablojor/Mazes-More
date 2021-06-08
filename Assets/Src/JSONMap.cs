using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script que se usa para deserializar los mapas y devolver los valores deserializados
/// </summary>

namespace MazesAndMore
{
    [Serializable]
    public class JSONPoint
    {
        public int x, y;
    }
    [Serializable]
    public class JSONTwoPoints
    {
        public JSONPoint o, d;
    }

    [Serializable]
    public class JSONMap
    {
        public int r, c;
        public JSONPoint s, f;
        public JSONPoint[] h;
        public JSONTwoPoints[] w;
        public JSONPoint[] i;


        /// <summary>
        /// </summary>
        /// <returns>numero de filas del mapa</returns>
        public int GetRows() { return r; }
        /// <summary>
        /// </summary>
        /// <returns>numero de columnas del mapa</returns>
        public int GetColumns() { return c; }
        /// <summary>
        /// </summary>
        /// <returns>punto de inicio del jugador</returns>
        public JSONPoint GetStart() { return s; }
        /// <summary>
        /// </summary>
        /// <returns>meta del mapa</returns>
        public JSONPoint GetEnd() { return f; }
        /// <summary>
        /// </summary>
        /// <returns>array de pistas</returns>
        public JSONPoint[] GetHints() { return h; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>array con los puntos donde hay muros</returns>
        public JSONTwoPoints[] GetWalls() { return w; }
        /// <summary>
        /// </summary>
        /// <returns>array con los puntos donde hay hielo</returns>
        public JSONPoint[] GetIce() { return i; }

        /// <summary>
        /// deserializa la clase y la devuelve
        /// </summary>
        /// <param name="json"></param>
        /// <returns>esta misma clase</returns>
        public static JSONMap FromJson(string json)
        {
            return JsonUtility.FromJson<JSONMap>(json);
        }
    }
}