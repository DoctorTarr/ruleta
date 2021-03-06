﻿using System;
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
        private int contadorNumeroGanador = 0; // Veces que recibio el numero ganador 
        private int contadorEsperaConfirmacionGanador = 0;
        private bool _isMoving = false, _isCameraOn = false, _isBallPresent = false, _haveNewWinner = false;
        private int _rpm = 0;
        private const int TABLE_CLOSED_TIMEOUT = 8 * 60 * 2; // 8 min * 60 secs * 2 (500 msec)
        private int _WinnerNumber = -1;
        private int _NewWinnerNumber = -1;
        private int _LastWinnerNumber = -1;
        private WINNER_CMD_TYPE _WinnerNumberCmd = WINNER_CMD_TYPE.NO_WINNER_CMD; // 1=Winner number 2=Winner status 0=no cmd


        public ESTADO_JUEGO GetGameState(int rpm, bool IsCameraOn, bool BallFound)
        {
            _isCameraOn = IsCameraOn;
            _rpm = rpm;
            _isMoving = _rpm > 0;
            _isBallPresent = BallFound;
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

        public int GetCurrentWinnerNumber()
        {
            return _WinnerNumber;
        }

        public int GetLastWinnerNumber()
        {
            return _LastWinnerNumber;
        }


        public  WINNER_CMD_TYPE GetCurrentWinnerNumberCmd()
        {
            return this._WinnerNumberCmd;
        }

        public int GetContadorNumeroGanador()
        {
            return this.contadorNumeroGanador;
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
            // After 2 secs (4 * 500 msecs) with cylinder movement, go to PLACE_YOUR_BETS
            if (this.contadorEstadoActual > 4)
            {
                currentState = ESTADO_JUEGO.PLACE_YOUR_BETS;
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

        // Process PLACE_YOUR_BETS state
        public void CheckPlaceYourBetsState()
        {
            if (!this._isMoving)
            {
                this.contadorEstadoActual++;
                if (!this._isCameraOn || (this.contadorEstadoActual > TABLE_CLOSED_TIMEOUT)) // Despues de 8 minutos cierra la mesa
                {
                    currentState = ESTADO_JUEGO.TABLE_CLOSED;
                    this.contadorEstadoActual = 0;
                }
            }
            else
            {
                // If ball is not in pocket => NO MORE BETS (a.k.a. Good Luck)
                if (!this._isBallPresent)
                {
                    this.contadorEstadoActual++;
                    _haveNewWinner = false;
                    if (this.contadorEstadoActual > 10)
                    {
                        currentState = ESTADO_JUEGO.NO_MORE_BETS;
                        this.contadorEstadoActual = 0;
                    }
                }
                else
                {
                    this.contadorEstadoActual = 0;
                }

            }
        }

        public void SetNewWinnerNumber(int winner)
        {
            if (currentState == ESTADO_JUEGO.NO_MORE_BETS)
            {
                // New number is coming
                if (this._NewWinnerNumber != winner)
                {
                    this._NewWinnerNumber = winner;
                    this.contadorNumeroGanador = 1;
                }
                else
                {
                    this.contadorNumeroGanador++;
                    if (this.contadorNumeroGanador > 2)
                    {
                        this._haveNewWinner = true;
                        this._WinnerNumber = _NewWinnerNumber;
                        this._NewWinnerNumber = -1;
                        this.contadorNumeroGanador = 0;
                        CheckNoMoreBetsState();
                    }
                }
            }
        }

        // Process NO_MORE_BETS state (waiting for winning number)
        public void CheckNoMoreBetsState()
        {
            if ((_NewWinnerNumber != -1) && (this.contadorEsperaConfirmacionGanador > 2 * 3))
            {
                this._haveNewWinner = true;
                this._WinnerNumber = _NewWinnerNumber;
                this._NewWinnerNumber = -1;
                this.contadorNumeroGanador = 0;
            }

            if (_haveNewWinner)
            {
                currentState = ESTADO_JUEGO.WINNING_NUMBER;
                this.contadorEsperaConfirmacionGanador = 0;
                this.contadorEstadoActual = 0;
            }
            else
            {
                this.contadorEstadoActual++;
                if (_NewWinnerNumber != -1)
                    this.contadorEsperaConfirmacionGanador++;

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
                        if ((this.contadorEstadoActual > 60) || (!this._isBallPresent)) // Send the winner number cmd 65 times (aprox 30 secs)
                        {
                            currentState = ESTADO_JUEGO.BEFORE_GAME; // Just in case
                            _WinnerNumberCmd = WINNER_CMD_TYPE.NO_WINNER_CMD;
                            _haveNewWinner = false;
                            _LastWinnerNumber = _WinnerNumber;
                            _WinnerNumber = -1;
                            this.contadorEstadoActual = 0;
                        }
                        break;

                }
            }
        }
    }
}
