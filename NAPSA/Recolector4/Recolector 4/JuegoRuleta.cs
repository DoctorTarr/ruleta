using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoRecolector
{
    class JuegoRuleta
    {
        public enum ESTADO_JUEGO : int
        {
            STATE_0,   // 0
            BEFORE_GAME,    // 1
            PLACE_YOUR_BETS, // 2
            FINISH_BETTING, // 3
            NO_MORE_BETS,   // 4
            WINNING_NUMBER, // 5
            TABLE_CLOSED    // 6
        }

        public enum WINNER_CMD_TYPE : int
        {
            NO_WINNER_CMD,  // 0
            WINNER_NUMBER_CMD,   // 1
            WINNER_STATUS_CMD   // 2
        }

        private ESTADO_JUEGO currentState = ESTADO_JUEGO.STATE_0;
        private int contadorEstadoActual = 0;
        private bool _isMoving = false, _isCameraOn = false, _isBallPresent = false, _haveNewWinner = false;
        private const int TABLE_CLOSED_TIMEOUT = 1 * 60 * 2;
        private int _WinnerNumber = -1;
        private WINNER_CMD_TYPE _WinnerNumberCmd = WINNER_CMD_TYPE.NO_WINNER_CMD; // 1=Winner number 2=Winner status 0=no cmd


        public ESTADO_JUEGO GetGameState(bool cylinderIsMoving, bool IsCameraOn, bool BallFound, int WinnerNumber)
        {
            _isCameraOn = IsCameraOn;
            _isMoving = cylinderIsMoving;
            _isBallPresent = BallFound;
            if (WinnerNumber != -1)
            {
                if (WinnerNumber != _WinnerNumber)
                {
                    _haveNewWinner = true;
                    _WinnerNumber = WinnerNumber;
                }
            }
            else
                _haveNewWinner = false;

            switch (currentState)
            {
                // State for starting the app only
                case ESTADO_JUEGO.STATE_0:
                    currentState = ESTADO_JUEGO.TABLE_CLOSED;
                    break;
                case ESTADO_JUEGO.TABLE_CLOSED:
                    //currentState = ESTADO_JUEGO.BEFORE_GAME;
                    CheckTableClosedState();
                    break;
                case ESTADO_JUEGO.BEFORE_GAME:
                    //currentState = ESTADO_JUEGO.PLACE_YOUR_BETS;
                    CheckBeforeGameState();
                    break;
                case ESTADO_JUEGO.PLACE_YOUR_BETS:
                    //currentState = ESTADO_JUEGO.NO_MORE_BETS;
                    CheckPlaceYourBetsState();
                    break;
                case ESTADO_JUEGO.NO_MORE_BETS:
                    //currentState = ESTADO_JUEGO.WINNING_NUMBER;
                    CheckNoMoreBetsState();
                    break;
                case ESTADO_JUEGO.WINNING_NUMBER:
                    //currentState = ESTADO_JUEGO.BEFORE_GAME;
                    CheckWinnerNumberState();
                    break;
            }
            return currentState;
        }

        public ESTADO_JUEGO GetCurrentState()
        {
            return currentState;
        }

        public void SetCurrentState(ESTADO_JUEGO estado)
        {
            currentState = estado;
        }

        // Process TABLE_CLOSED state
        public void CheckTableClosedState()
        {
            // If cylinder is moving then go to BEFORE_GAME
            if (this._isMoving)
            {
                currentState = ESTADO_JUEGO.BEFORE_GAME;
                this.contadorEstadoActual = 0;
            }
        }

        // Process BEFORE_GAME state
        public void CheckBeforeGameState()
        {
            this.contadorEstadoActual++;
            if (this._isMoving)
            {
                // After 2 secs (4 * 500 msecs) with cylinder movement, go to PLACE_YOUR_BETS
                if (this.contadorEstadoActual > 4)
                {
                    currentState = ESTADO_JUEGO.PLACE_YOUR_BETS;
                    this.contadorEstadoActual = 0;
                }
            }
            else
            {
                if (!this._isCameraOn || (this.contadorEstadoActual > TABLE_CLOSED_TIMEOUT)) // Despues de 8 minutos cierra la mesa
                {
                    currentState = ESTADO_JUEGO.TABLE_CLOSED;
                    this.contadorEstadoActual = 0;
                }
            }
        }

        // Process PLACE_YOUR_BETS state
        public void CheckPlaceYourBetsState()
        {
            this.contadorEstadoActual++;
            if (this._isMoving)
            {
                // If ball is not in pocket => NO MORE BETS (a.k.a. Good Luck)
                if (!this._isBallPresent)
                {
                    if (this.contadorEstadoActual > 10)
                    {
                        currentState = ESTADO_JUEGO.NO_MORE_BETS;
                        this.contadorEstadoActual = 0;
                    }
                }
            }
            else
            {
                if (!this._isCameraOn || (this.contadorEstadoActual > TABLE_CLOSED_TIMEOUT)) // Despues de 8 minutos cierra la mesa
                {
                    currentState = ESTADO_JUEGO.TABLE_CLOSED;
                    this.contadorEstadoActual = 0;
                }
            }
        }

        // Process NO_MORE_BETS state (waiting for winning number)
        public void CheckNoMoreBetsState()
        {
            this.contadorEstadoActual++;
            if (_haveNewWinner)
            {
                currentState = ESTADO_JUEGO.WINNING_NUMBER;
                this.contadorEstadoActual = 0;
            }
            else
            {
                if (!this._isCameraOn || (this.contadorEstadoActual > TABLE_CLOSED_TIMEOUT)) // Despues de 8 minutos cierra la mesa
                {
                    currentState = ESTADO_JUEGO.TABLE_CLOSED;
                    this.contadorEstadoActual = 0;
                }
            }
        }

        // Process WINNER_NUMBER state (send winner number and then send winner number state)
        public void CheckWinnerNumberState()
        {
            this.contadorEstadoActual++;
            // To keep sending the winner number before winner state
            if (_haveNewWinner)
            {
                switch (this._WinnerNumberCmd)
                {
                    case WINNER_CMD_TYPE.NO_WINNER_CMD:
                        currentState = ESTADO_JUEGO.WINNING_NUMBER; // Just in case
                        _WinnerNumberCmd = WINNER_CMD_TYPE.WINNER_NUMBER_CMD;
                        this.contadorEstadoActual = 0;
                        break;

                    case WINNER_CMD_TYPE.WINNER_NUMBER_CMD:
                        if (this.contadorEstadoActual > 4) // Send the winner number cmd 4 times
                        {
                            currentState = ESTADO_JUEGO.WINNING_NUMBER; // Just in case
                            _WinnerNumberCmd = WINNER_CMD_TYPE.WINNER_STATUS_CMD;
                            this.contadorEstadoActual = 0;
                        }
                        break;

                    case WINNER_CMD_TYPE.WINNER_STATUS_CMD:
                        if (this.contadorEstadoActual > 20) // Send the winner number cmd 10 times and then back to before game
                        {
                            currentState = ESTADO_JUEGO.BEFORE_GAME; // Just in case
                            _WinnerNumberCmd = WINNER_CMD_TYPE.NO_WINNER_CMD;
                            _haveNewWinner = false;
                            this.contadorEstadoActual = 0;
                        }
                        break;

                }
            }
        }
    }
}
