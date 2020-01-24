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

        private ESTADO_JUEGO currentState = ESTADO_JUEGO.STATE_0;
        private int contadorEstadoActual = 0;
        private bool _isMoving = false, _isCameraOn = false, _isBallPresent = false;
        private const int TABLE_CLOSED_TIMEOUT = 1 * 60 * 2;


        public ESTADO_JUEGO GetGameState(bool cylinderIsMoving, bool IsCameraOn, bool BallFound)
        {
            _isCameraOn = IsCameraOn;
            _isMoving = cylinderIsMoving;
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
                    CheckPlaceYourBets();
                    break;
                case ESTADO_JUEGO.NO_MORE_BETS:
                    currentState = ESTADO_JUEGO.WINNING_NUMBER;
                    break;
                case ESTADO_JUEGO.WINNING_NUMBER:
                    currentState = ESTADO_JUEGO.BEFORE_GAME;
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

        public void CheckTableClosedState()
        {
            // If cylinder is moving then go to BEFORE_GAME
            if (this._isMoving)
            {
                currentState = ESTADO_JUEGO.BEFORE_GAME;
                this.contadorEstadoActual = 0;
            }
        }

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

        public void CheckPlaceYourBets()
        {
            if (!this._isBallPresent && this._isCameraOn && this._isMoving)
            {
                currentState = ESTADO_JUEGO.NO_MORE_BETS;
                this.contadorEstadoActual = 0; // Just in case
            }
        }

    }
}
