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
            STARTING_APP,   // 0
            BEFORE_GAME,    // 1
            PLACE_YOUR_BETS, // 2
            FINISH_BETTING, // 3
            NO_MORE_BETS,   // 4
            WINNING_NUMBER, // 5
            TABLE_CLOSED    // 6
        }

        private ESTADO_JUEGO currentState = ESTADO_JUEGO.STARTING_APP;

        public ESTADO_JUEGO GetNextStatus()
        {
            switch (currentState)
            {
                case ESTADO_JUEGO.STARTING_APP:
                    currentState = ESTADO_JUEGO.TABLE_CLOSED;
                    break;
                case ESTADO_JUEGO.TABLE_CLOSED:
                    currentState = ESTADO_JUEGO.BEFORE_GAME;
                    break;
                case ESTADO_JUEGO.BEFORE_GAME:
                    currentState = ESTADO_JUEGO.PLACE_YOUR_BETS;
                    break;
                case ESTADO_JUEGO.PLACE_YOUR_BETS:
                    currentState = ESTADO_JUEGO.NO_MORE_BETS;
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
    }
}
