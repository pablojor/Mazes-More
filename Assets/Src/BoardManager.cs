using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MazesAndMore
{
    public class BoardManager : MonoBehaviour
    {
        [Tooltip("Script de un tile")]
        public Tile tile;
        [Tooltip("Script que maneja la camara")]
        public CameraManager camMan;
        [Tooltip("Script que maneja el jugador")]
        public Player player;

        /// <summary>
        /// Script que maneja los niveles
        /// </summary>
        private LevelManager _levelManager;
        /// <summary>
        /// matriz de tiles que corresponde con el mapa
        /// </summary>
        private Tile[,] _tiles;
        /// <summary>
        /// matriz que lleva el contador de las veces que pasas por una celda
        /// </summary>
        private Trace[,] _traces;
        /// <summary>
        /// medidas de la matriz/mapa del juego
        /// </summary>
        private int _columns, _rows;
        /// <summary>
        /// vector con todas las pistas del mapa actual
        /// </summary>
        private Vector2Int[] _hints;
        /// <summary>
        /// indice de por qué pista vamos en el vector
        /// </summary>
        private int _movesInHints;
        /// <summary>
        /// contador de las pistas que hemos usado
        /// se usa para el uso de pistas
        /// </summary>
        private int _numHint;
        /// <summary>
        /// la posicion de lasiguiente pista que vamos a usar del vector de pistas
        /// </summary>
        private Vector2Int _actualPosHint;
        /// <summary>
        /// posicion de inicio en el mapa actual
        /// </summary>
        private Vector2Int _start;

        /// <summary>
        /// Variables para realizar el contador de veces que pasas por una celda
        /// </summary>
        private struct Trace
        {
            public int right, left, up, down;
        }

        /// <summary>
        /// Incializar el tablero, declarandole el levelManager e incializando el jugador
        /// </summary>
        /// <param name="levelManager"></param>
        public void Init(LevelManager levelManager)
        {
            _levelManager = levelManager;
            player.Init(this);
        }
        /// <summary>
        /// inicializar todo lo referente a un mapa
        /// </summary>
        /// <param name="map"> mapa que se va a usar </param>
        /// <param name="c"> color en este mapa </param>
        public void SetMap(Map map, Color c)
        {
            Color color = c;
            _movesInHints = 0;
            _numHint = 1;
            _actualPosHint = _start = map.GetStart();
            _hints = map.GetHints();
            _rows = map.GetRows();
            _columns = map.GetColumns();
            Map.Cell[,]  adjList = map.GetAdjList();

            _tiles = new Tile[_rows, _columns];
            _traces = new Trace[_rows, _columns];

            for (int i = 0; i < _rows; i++)
                for (int j = 0; j < _columns; j++)
                {
                    Tile aux = Instantiate(tile, new Vector3(j + 0.5f - (float)_columns / 2.0f, -i - 1f + (float)_rows / 2.0f, 0), Quaternion.identity);
                    aux.transform.parent = this.gameObject.transform;

                    bool checkR = false, checkD = false;
                    if (i == map.GetRows() - 1)
                        checkD = true;
                    if (j == map.GetColumns() - 1)
                        checkR = true;
                    aux.EnableWalls(adjList[i, j], checkR, checkD);

                    if (adjList[i, j].ice)
                        aux.EnableIceFloor();
                    aux.SetTracesColor(color);
                    _tiles[i, j] = aux;

                    _traces[i, j].right = 0;
                    _traces[i, j].left = 0;
                    _traces[i, j].up = 0;
                    _traces[i, j].down = 0;
                }
            _tiles[(int)map.GetEnd().y, (int)map.GetEnd().x].EnableGoal();

            if(camMan.GetScaleHeight() < 1.0f)
                camMan.ChangeCameraSizeWidht(((float)_columns / 2.0f) + 0.2f);
            else
                camMan.ChangeCameraSize((16f * ((float)_columns / 2.0f) / 9f) + 0.2f);
            player.SetMap(map);
            player.setColor(color);
        }

        /// <summary>
        /// actualizar la matriz del contador de veces que pasas por uan celda
        /// </summary>
        /// <param name="i"> posicion en la x </param>
        /// <param name="j"> posicion en la y </param>
        /// <param name="remove"> si estamos volviendo por ese camino o no</param>
        /// <param name="move"> movimiento del juagdor </param>
        public void SetTrace(int i, int j, bool remove, Player.Movements move)
        {
            if (remove)
            {
                if (move == Player.Movements.UP)
                {

                    i--;
                    if (i >= 0)
                    {
                        if (_traces[i, j].down > 0)
                            _traces[i, j].down--;
                        if (_traces[i, j].down == 0)
                            _tiles[i, j].DisableCharacterDownTrace();
                    }

                }
                else if (move == Player.Movements.DOWN)
                {
                    i++;
                    if (i < _rows)
                    {
                        if (_traces[i, j].up > 0)
                            _traces[i, j].up--;
                        if (_traces[i, j].up == 0)
                            _tiles[i, j].DisableCharacterUpTrace();
                    }
                }
                else if (move == Player.Movements.RIGHT)
                {
                    j++;
                    if (j < _columns)
                    {
                        if (_traces[i, j].left > 0)
                            _traces[i, j].left--;
                        if (_traces[i, j].left == 0)
                            _tiles[i, j].DisableCharacterLeftTrace();
                    }
                }
                else if (move == Player.Movements.LEFT)
                {
                    j--;
                    if (j >= 0)
                    {
                        if (_traces[i, j].right > 0)
                            _traces[i, j].right--;
                        if (_traces[i, j].right == 0)
                            _tiles[i, j].DisableCharacterRightTrace();
                    }
                }
            }
            else
            {
                if (move == Player.Movements.UP)
                {

                    _traces[i, j].up++;
                    if (_traces[i, j].up == 1)
                        _tiles[i, j].EnableCharacterUpTrace();

                }
                else if (move == Player.Movements.DOWN)
                {
                    _traces[i, j].down++;
                    if (_traces[i, j].down == 1)
                        _tiles[i, j].EnableCharacterDownTrace();
                }
                else if (move == Player.Movements.RIGHT)
                {
                    _traces[i, j].right++;
                    if (_traces[i, j].right == 1)
                        _tiles[i, j].EnableCharacterRightTrace();
                }
                else if (move == Player.Movements.LEFT)
                {
                    _traces[i, j].left++;
                    if (_traces[i, j].left == 1)
                        _tiles[i, j].EnableCharacterLeftTrace();
                }
            }
        }

        /// <summary>
        /// pausar el juego o despausar y lo que conlleva
        /// </summary>
        /// <param name="paused"></param>
        public void SetPause(bool paused)
        {
            player.SetPause(paused);
            for (int i = 0; i < _rows; i++)
                for (int j = 0; j < _columns; j++)
                    _tiles[i, j].SetPause(paused);
        }

        /// <summary>
        /// reiniciar el nivel
        /// </summary>
        public void RestartLevel()
        {
            _movesInHints = 0;
            _numHint = 1;
            _actualPosHint = _start;
            player.RestartLevel();
            for (int i = 0; i < _rows; i++)
                for (int j = 0; j < _columns; j++)
                {
                    _tiles[i, j].RestartLevel();
                    _traces[i, j].right = 0;
                    _traces[i, j].left = 0;
                    _traces[i, j].up = 0;
                    _traces[i, j].down = 0;
                }
        }

        /// <summary>
        /// finalizar el nivel y lo que conlleva
        /// </summary>
        public void LevelFinished()
        {
            ShowLevel(false);
            _levelManager.LevelFinished();
        }

        /// <summary>
        /// mostrar el nivel
        /// </summary>
        /// <param name="show"> si se muestra o no </param>
        public void ShowLevel(bool show)
        {
            player.ShowPlayer(show);
            for (int i = 0; i < _rows; i++)
                for (int j = 0; j < _columns; j++)
                    _tiles[i, j].Show(show);
        }

        /// <summary>
        /// Limpiar el nivel
        /// </summary>
        public void Clean()
        {
            for (int i = 0; i < _rows; i++)
                for (int j = 0; j < _columns; j++)
                    DestroyImmediate(_tiles[i, j]);
        }

        /// <summary>
        /// Mostrar una pista
        /// </summary>
        /// <returns>true si quedan pistas por mostrar, false si no</returns>
        public bool ShowHint()
        {
            int third = (_hints.Length / 3);

            if (third % 2 != 0)
                third++;

            int until = third * _numHint;

            if (_numHint == 3)
                until = _hints.Length;
            else if (_numHint > 3)
                return false;

            while (_movesInHints < until)
            {
                if (_actualPosHint.x == _hints[_movesInHints].x + 1)//left
                {
                    _tiles[_actualPosHint.y, _actualPosHint.x].EnableHintLeftTrace();
                }
                else if (_actualPosHint.x == _hints[_movesInHints].x - 1)//right
                {
                    _tiles[_actualPosHint.y, _actualPosHint.x].EnableHintRightTrace();
                }
                else if (_actualPosHint.y == _hints[_movesInHints].y - 1)//down
                {
                    _tiles[_actualPosHint.y, _actualPosHint.x].EnableHintDownTrace();
                }
                else//up
                {
                    _tiles[_actualPosHint.y, _actualPosHint.x].EnableHintUpTrace();
                }


                _actualPosHint = _hints[_movesInHints];
                _movesInHints++;
            }
            _numHint++;
            return true;
        }
    }

}