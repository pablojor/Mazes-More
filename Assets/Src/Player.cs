using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazesAndMore
{
    public class Player : MonoBehaviour
    {
        [Tooltip("Velocidad de movimiento del jugador")]
        public float velocity = 5;
        [Tooltip("Sprite del jugador")]
        public SpriteRenderer mySprite;

        [Tooltip("Flechas de movimiento del jugador")]
        public Arrow up, down, right, left;

        /// <summary>
        /// movimientos que puede realizar el jugador
        /// </summary>
        public enum Movements
        {
            LEFT, RIGHT, UP, DOWN, NOMOVE
        }

        /// <summary>
        /// instancia del BoardManager
        /// </summary>
        private BoardManager _boardManager;
        /// <summary>
        /// matriz de adyacencia
        /// </summary>
        private Map.Cell[,] _adjList;
        /// <summary>
        /// dimensiones del mapa
        /// </summary>
        private int _rows, _columns;
        /// <summary>
        /// indice de la celda actual
        /// </summary>
        private Vector2Int _currentCell; 
        /// <summary>
        /// posiciones clave para el juagdor
        /// inicio y final
        /// </summary>
        private Vector2Int _start, _end;
        /// <summary>
        /// posicion real de la celda actual
        /// </summary>
        private Vector2 _currentCellPos;
        /// <summary>
        /// booleano que marca si el boton del raton esta pulsado
        /// </summary>
        private bool _mousePressed = false;
        /// <summary>
        /// booleano que marca si hemos empezado a mover el raton manteniendo pulsado
        /// </summary>
        private bool _moved = false;
        /// <summary>
        /// posicion del raton cuando realizamos el input
        /// </summary>
        private Vector2 _mousePosition;
        /// <summary>
        /// booleano que marca si nos estamos moviendo o no
        /// </summary>
        private bool _moving;
        /// <summary>
        /// booleano que marca si vamos marcha atras o no
        /// </summary>
        private bool _movingBackwards = false;
        /// <summary>
        /// direccion que sigue el jugador
        /// </summary>
        private Vector2Int _moveDir;
        /// <summary>
        /// movimiento actual del jugador
        /// </summary>
        private Movements _actualMove = Movements.NOMOVE;
        /// <summary>
        /// booleano que marca si estamos en pausa o no
        /// </summary>
        private bool _paused = false;
        /// <summary>
        /// booleano que marca si estamos en hielo o no 
        /// </summary>
        private bool _inIce = false;

        /// <summary>
        /// acciones que realiza el jugador
        /// </summary>
        private struct Order
        {
            public Movements move;
            public Vector2Int pos;
        }
        /// <summary>
        /// pila de acciones que hemos realizado
        /// </summary>
        private Stack<Order> _lastMoves;

        /// <summary>
        /// inicializa al jugador, estableciendo el BoardManager y creando de cero la pila de movimientos
        /// </summary>
        /// <param name="boardManager"></param>
        public void Init(BoardManager boardManager)
        {
            _boardManager = boardManager;

            _lastMoves = new Stack<Order>();
        }

        /// <summary>
        /// establece el color del jugador y de sus flechas de movimiento
        /// </summary>
        /// <param name="color">color nuevo</param>
        public void setColor(Color color)
        {
            mySprite.color = color;
            up.SetColor(color);
            down.SetColor(color);
            right.SetColor(color);
            left.SetColor(color);
        }

        /// <summary>
        /// almacena todos los datos que necesita del mapa actual
        /// </summary>
        /// <param name="map">mapa del nivel</param>
        public void SetMap(Map map)
        {
            _adjList = map.GetAdjList();
            _rows = map.GetRows();
            _columns = map.GetColumns();
            _start = map.GetStart();
            _currentCell = _start;
            _end = map.GetEnd();
            this.transform.position = new Vector3(_currentCell.x + 0.5f - (float)_columns / 2.0f, -_currentCell.y - 1f + (float)_rows / 2.0f, 0);

            _currentCellPos = this.transform.position;

            _inIce = _adjList[_currentCell.y, _currentCell.x].ice;
            RestartLevel();
        }

        /// <summary>
        /// controla el movimiento del jugador en cada tick, si no estamos pausados
        /// </summary>
        void Update()
        {
            if (!_paused)
            {

                if (!_moving)
                {
                    Vector2Int moveDir = CheckIfMove();
                    _moveDir = moveDir;
                    if (_moveDir.x != 0 || _moveDir.y != 0)
                        CheckDirection();

                }
                else
                {

                    Move();
                }
            }
        }

        /// <summary>
        /// activa las flechas de movimiento del jugador segun el movimiento que pueda realizar el jugador
        /// </summary>
        private void EnableArrows()
        {
            if (_adjList[_currentCell.y, _currentCell.x].up)
                up.SetEnable();
            if (_adjList[_currentCell.y, _currentCell.x].down)
                down.SetEnable();
            if (_adjList[_currentCell.y, _currentCell.x].right)
                right.SetEnable();
            if (_adjList[_currentCell.y, _currentCell.x].left)
                left.SetEnable();
        }

        /// <summary>
        /// desactiva todas las flechas de movimiento del jugador
        /// </summary>
        private void DisableArrows()
        {
            up.SetDisable();
            down.SetDisable();
            right.SetDisable();
            left.SetDisable();
        }

        /// <summary>
        /// comprueba si el movimiento que realizamos es posible y, si lo es, lo registra
        /// </summary>
        private void CheckDirection()
        {
            if (_actualMove == Movements.RIGHT && _adjList[_currentCell.y, _currentCell.x].right)
            {
                _movingBackwards = CheckBackwards(Movements.LEFT);
                AddMove();
            }

            else if (_actualMove == Movements.LEFT && _adjList[_currentCell.y, _currentCell.x].left)
            {
                _movingBackwards = CheckBackwards(Movements.RIGHT);
                AddMove();
            }

            else if (_actualMove == Movements.UP && _adjList[_currentCell.y, _currentCell.x].up)
            {
                _movingBackwards = CheckBackwards(Movements.DOWN);
                AddMove();
            }

            else if (_actualMove == Movements.DOWN && _adjList[_currentCell.y, _currentCell.x].down)
            {
                _movingBackwards = CheckBackwards(Movements.UP);
                AddMove();
            }
        }

        /// <summary>
        /// añade un movimiento a la pila de acciones que hemos hecho (si no estamos moviendonos hacia atras)
        /// </summary>
        private void AddMove()
        {
            _moving = true;
            DisableArrows();

            _boardManager.SetTrace(_currentCell.y, _currentCell.x, _movingBackwards, _actualMove);
            if (!_movingBackwards)
            {
                Order ord = new Order();
                ord.move = _actualMove;
                ord.pos.x = _currentCell.x;
                ord.pos.y = _currentCell.y;
                _lastMoves.Push(ord);
            }
        }

        /// <summary>
        /// comprueba si estamos yendo hacia atras
        /// </summary>
        /// <param name="checkMove">movimiento realizado</param>
        /// <returns></returns>
        private bool CheckBackwards(Movements checkMove)
        {
            if (_lastMoves.Count > 0)
            {
                Order move = _lastMoves.Pop();
                if ((move.pos.x == _currentCell.x && move.pos.y == _currentCell.y))
                {
                    if (_lastMoves.Count > 0)
                    {
                        move = _lastMoves.Pop();
                    }
                    else
                        return false;
                }

                if (move.move == checkMove && !(move.pos.x == _currentCell.x && move.pos.y == _currentCell.y))
                {
                    _lastMoves.Push(move);
                    return true;
                }
                else if (move.pos.x != _currentCell.x || move.pos.y != _currentCell.y)
                    _lastMoves.Push(move);
            }
            return false;
        }

        /// <summary>
        /// realiza el movimiento del jugador
        /// comprueba en que direccion vamos y realiza las comprobaciones necesarias segun esa direccion y si hemos llegado a la 
        /// siguiente celda o no
        /// </summary>
        private void Move()
        {
            transform.position = new Vector3(transform.position.x + (_moveDir.x * velocity * Time.deltaTime), transform.position.y + (_moveDir.y * velocity * Time.deltaTime), 0);

            if (_actualMove == Movements.RIGHT || _actualMove == Movements.LEFT)
            {

                if (_actualMove == Movements.RIGHT)
                {

                    if (transform.position.x >= _currentCellPos.x + _moveDir.x)
                    {
                        int posibleDirs = UpdateCell();
                        if (!_inIce)
                            SetHoriMovement(!_adjList[_currentCell.y, _currentCell.x].right, posibleDirs);
                        else
                            SetIceMovement(!_adjList[_currentCell.y, _currentCell.x].right);

                    }
                }
                else
                {
                    if (transform.position.x <= _currentCellPos.x + _moveDir.x)
                    {
                        int posibleDirs = UpdateCell();
                        if (!_inIce)
                            SetHoriMovement(!_adjList[_currentCell.y, _currentCell.x].left, posibleDirs);
                        else
                            SetIceMovement(!_adjList[_currentCell.y, _currentCell.x].left);
                    }
                }
            }
            else
            {

                if (_moveDir.y > 0)
                {
                    if (transform.position.y >= _currentCellPos.y + _moveDir.y)
                    {
                        int posibleDirs = UpdateCell();
                        if (!_inIce)
                            SetVerticalMovement(!_adjList[_currentCell.y, _currentCell.x].up, posibleDirs);
                        else
                            SetIceMovement(!_adjList[_currentCell.y, _currentCell.x].up);
                    }
                }
                else
                {
                    if (transform.position.y <= _currentCellPos.y + _moveDir.y)
                    {
                        int posibleDirs = UpdateCell();
                        if (!_inIce)
                            SetVerticalMovement(!_adjList[_currentCell.y, _currentCell.x].down, posibleDirs);
                        else
                            SetIceMovement(!_adjList[_currentCell.y, _currentCell.x].down);
                    }
                }
            }
            if (CheckIfFinished())
                _boardManager.LevelFinished();
        }


        /// <summary>
        /// mueve al jugador segun los posibles movimientos que tenga
        /// si puede elegir porque esta en una encrucijada o en un callejon sin salida, vuelve a tener el jugador el control
        /// si no puede elegir, comprueba cual es el siguiente posible camino 
        /// </summary>
        /// <param name="move">si puedo seguir moviendome en la direccion que iba o no</param>
        /// <param name="posibleDirs">numero de posibles movimientos en esta casilla</param>
        private void SetHoriMovement(bool move, int posibleDirs)
        {
            if (posibleDirs == 1 || posibleDirs >= 3)
            {
                _moving = false;
                EnableArrows();
                _actualMove = Movements.NOMOVE;
                transform.position = _currentCellPos;
            }
            else
            {

                if (move)
                {
                    if (_adjList[_currentCell.y, _currentCell.x].up)
                    {
                        _actualMove = Movements.UP;

                        _moveDir = new Vector2Int(0, 1);
                        _movingBackwards = CheckBackwards(Movements.DOWN);
                        AddMove();
                    }
                    else
                    {
                        _actualMove = Movements.DOWN;

                        _moveDir = new Vector2Int(0, -1);
                        _movingBackwards = CheckBackwards(Movements.UP);
                        AddMove();
                    }
                }
                else
                    _boardManager.SetTrace(_currentCell.y, _currentCell.x, _movingBackwards, _actualMove);
            }
        }

        /// <summary>
        /// mueve al jugador segun los posibles movimientos que tenga
        /// si puede elegir porque esta en una encrucijada o en un callejon sin salida, vuelve a tener el jugador el control
        /// si no puede elegir, comprueba cual es el siguiente posible camino 
        /// </summary>
        /// <param name="move">si puedo seguir moviendome en la direccion que iba o no</param>
        /// <param name="posibleDirs">numero de posibles movimientos en esta casilla</param>
        private void SetVerticalMovement(bool move, int posibleDirs)
        {
            if (posibleDirs == 1 || posibleDirs >= 3)
            {
                _moving = false;
                EnableArrows();
                _actualMove = Movements.NOMOVE;
                transform.position = _currentCellPos;
            }
            else
            {
                if (move)
                {
                    if (_adjList[_currentCell.y, _currentCell.x].right)
                    {
                        _actualMove = Movements.RIGHT;

                        _moveDir = new Vector2Int(1, 0);
                        _movingBackwards = CheckBackwards(Movements.LEFT);
                        AddMove();
                    }
                    else
                    {
                        _actualMove = Movements.LEFT;

                        _moveDir = new Vector2Int(-1, 0);
                        _movingBackwards = CheckBackwards(Movements.RIGHT);
                        AddMove();
                    }
                }
                else
                    _boardManager.SetTrace(_currentCell.y, _currentCell.x, _movingBackwards, _actualMove);
            }
        }

        /// <summary>
        /// realiza el movimiento del jugador cuandoe este se encuentra en el hielo
        /// </summary>
        /// <param name="move">se esta moviendo por el jugador o no</param>
        private void SetIceMovement(bool move)
        {
            if (move)
            {
                _moving = false;
                EnableArrows();
                _actualMove = Movements.NOMOVE;
                transform.position = _currentCellPos;
            }
            else
            {
                bool lastBackwards = _movingBackwards;
                if (_actualMove == Movements.DOWN)
                    _movingBackwards = CheckBackwards(Movements.UP);
                else if (_actualMove == Movements.UP)
                    _movingBackwards = CheckBackwards(Movements.DOWN);
                else if (_actualMove == Movements.RIGHT)
                    _movingBackwards = CheckBackwards(Movements.LEFT);
                else if (_actualMove == Movements.LEFT)
                    _movingBackwards = CheckBackwards(Movements.RIGHT);
                _boardManager.SetTrace(_currentCell.y, _currentCell.x, _movingBackwards, _actualMove);

                if (_movingBackwards != lastBackwards && !_movingBackwards)
                {
                    Order ord = new Order();
                    ord.move = _actualMove;
                    ord.pos.x = _currentCell.x;
                    ord.pos.y = _currentCell.y;
                    _lastMoves.Push(ord);
                }
            }
        }

        /// <summary>
        /// actualiza la celda actual del jugador y todo lo que esto conlleva
        /// </summary>
        /// <returns>numero de posibles movimientos en la nueva celda actual</returns>
        private int UpdateCell()
        {
            _currentCell.x += _moveDir.x;
            _currentCell.y -= _moveDir.y;
            _currentCellPos.x = _currentCell.x + 0.5f - (float)_columns / 2.0f;
            _currentCellPos.y = -_currentCell.y - 1f + (float)_rows / 2.0f;

            _inIce = _adjList[_currentCell.y, _currentCell.x].ice;

            this.transform.position = new Vector3(_currentCellPos.x, _currentCellPos.y, 0);

            return (_adjList[_currentCell.y, _currentCell.x].down ? 1 : 0) + (_adjList[_currentCell.y, _currentCell.x].up ? 1 : 0) +
                            (_adjList[_currentCell.y, _currentCell.x].right ? 1 : 0) + (_adjList[_currentCell.y, _currentCell.x].left ? 1 : 0);
        }

        /// <summary>
        /// comprueba si esta en la celda del final
        /// </summary>
        /// <returns>true si ha llegado, false si no</returns>
        private bool CheckIfFinished()
        {
            return _currentCell.x == _end.x && _currentCell.y == _end.y;
        }

        /// <summary>
        /// controla el input del jugador y sus consecuencias
        /// </summary>
        /// <returns>direccion a la que se mueve el jugador. (0,0) si no se mueve</returns>
        private Vector2Int CheckIfMove()
        {

            //si el boton izquierdo del raton esta pausado y no se esta moviendo
            if (Input.GetMouseButton(0) && !_moving)
            {
                if (!_mousePressed)
                {
                    _mousePosition = Input.mousePosition;
                    _mousePressed = true;
                }
                else if (!_moved)
                {
                    Vector2 moveDir;

                    moveDir = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - _mousePosition;
                    if (Mathf.Abs(moveDir.x) > 1 || Mathf.Abs(moveDir.y) > 1)
                    {
                        _moved = true;
                        if (Mathf.Abs(moveDir.x) > Mathf.Abs(moveDir.y))
                        {
                            if (moveDir.x > 0)
                            {
                                _actualMove = Movements.RIGHT;
                                return new Vector2Int(1, 0);
                            }
                            else
                            {
                                _actualMove = Movements.LEFT;
                                return new Vector2Int(-1, 0);
                            }
                        }
                        else
                        {
                            if (moveDir.y > 0)
                            {
                                _actualMove = Movements.UP;
                                return new Vector2Int(0, 1);
                            }
                            else
                            {
                                _actualMove = Movements.DOWN;
                                return new Vector2Int(0, -1);
                            }
                        }
                    }
                    else
                        return new Vector2Int(0, 0);
                }
            }
            else
            {
                _mousePressed = false;
                _moved = false;
            }
            return new Vector2Int(0, 0);
        }

        /// <summary>
        /// marca que hay que pausar o no al jugador
        /// </summary>
        /// <param name="paused">pausar o no</param>
        public void SetPause(bool paused)
        {
            _paused = paused;
        }

        /// <summary>
        /// hace todo lo necesario para reiniciar al jugador
        /// </summary>
        public void RestartLevel()
        {
            _currentCell = _start;
            this.transform.position = new Vector3(_currentCell.x + 0.5f - (float)_columns / 2.0f, -_currentCell.y - 1f + (float)_rows / 2.0f, 0);
            _currentCellPos = this.transform.position;
            _moving = false;
            _movingBackwards = false;
            _moved = false;
            _actualMove = Movements.NOMOVE;
            _lastMoves.Clear();
            _inIce = _adjList[_currentCell.y, _currentCell.x].ice;
            DisableArrows();
            EnableArrows();
            ShowPlayer(true);
        }

        /// <summary>
        /// avisa si hay que mostrar o no al jugador y sus flechas
        /// </summary>
        /// <param name="show">mostrar o no</param>
        public void ShowPlayer(bool show)
        {
            mySprite.enabled = show;
            if (!show)
                DisableArrows();
            else
                EnableArrows();
        }
    }

}