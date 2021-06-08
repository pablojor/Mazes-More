using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazesAndMore
{
    public class Map
    {
        /// <summary>
        /// struct para representar las celdas.
        /// cada celda tiene cuatro booleanos que expresan si se puede avanzar en esa direccion o no
        /// se usa para la matriz de adyacencia
        /// </summary>
        public class Cell
        {
            public bool up = true, down = true, right = true, left = true, ice = false;

        }

        /// <summary>
        /// matriz de celdas que expresa la matriz de adyacencia
        /// </summary>
        private Cell[,] _adjList;
        /// <summary>
        /// dimensiones de la amtriz/mapa
        /// </summary>
        private int _rows, _columns;
        /// <summary>
        /// puntos de incio y final del mapa
        /// </summary>
        private Vector2Int _start, _end;
        /// <summary>
        /// vector con las pistas
        /// </summary>
        private Vector2Int[] _hints;

        /// <summary>
        /// inicializa el mapa con la informacion que se le pasa
        /// </summary>
        /// <param name="map">json del mapa serializado</param>
        public void Init(JSONMap map)
        {
            _rows = map.GetRows();
            _columns = map.GetColumns();

            JSONTwoPoints[] walls = map.GetWalls();

            _start = new Vector2Int(map.GetStart().x, _rows - 1 - map.GetStart().y);
            _end = new Vector2Int(map.GetEnd().x, _rows - 1 - map.GetEnd().y);

            _adjList = new Cell[_rows, _columns];

            for (int i = 0; i < _rows; i++)
                for (int j = 0; j < _columns; j++)
                    _adjList[i, j] = new Cell();

            for (int i = 0; i < walls.Length; i++)
            {
                walls[i].o.y = _rows - walls[i].o.y;
                walls[i].d.y = _rows - walls[i].d.y;

                JSONTwoPoints wall = walls[i];
                if (wall.o.x == wall.d.x)
                {
                    int realRow = Mathf.Min(wall.o.y, wall.d.y);

                    if (wall.o.x - 1 >= 0)
                        _adjList[realRow, wall.o.x - 1].right = false;
                    if (wall.o.x < _columns)
                        _adjList[realRow, wall.o.x].left = false;
                }
                else
                {
                    int realColumn = Mathf.Min(wall.o.x, wall.d.x);
                    if (wall.o.y - 1 >= 0)
                        _adjList[wall.o.y - 1, realColumn].down = false;
                    if (wall.o.y < _rows)
                        _adjList[wall.o.y, realColumn].up = false;
                }
            }

            JSONPoint[] ice = map.GetIce();

            for (int i = 0; i < ice.Length; i++)
            {
                _adjList[_rows - 1 - ice[i].y, ice[i].x].ice = true;
            }

            JSONPoint[] hints = map.GetHints();
            _hints = new Vector2Int[hints.Length];
            for (int i = 0; i < hints.Length; i++)
            {
                _hints[i] = new Vector2Int(hints[i].x, _rows - 1 - hints[i].y);
            }
        }

        /// <summary>
        /// </summary>
        /// <returns>matriz de adyacencia</returns>
        public Cell[,] GetAdjList() { return _adjList; }

        /// <summary>
        /// </summary>
        /// <returns>numero de filas del mapa</returns>
        public int GetRows() { return _rows; }

        /// <summary>
        /// </summary>
        /// <returns>numero de columnas del mapa</returns>
        public int GetColumns() { return _columns; }

        /// <summary>
        /// </summary>
        /// <returns>punto de inicio</returns>
        public Vector2Int GetStart() { return _start; }

        /// <summary>
        /// </summary>
        /// <returns>punto de la meta</returns>
        public Vector2Int GetEnd() { return _end; }

        /// <summary>
        /// </summary>
        /// <returns>array con las pistas</returns>
        public Vector2Int[] GetHints() { return _hints; }
    }
}