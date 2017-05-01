import * as types from './actionTypes';

export function getCurrentPlayers(blackSidePlayer, whiteSidePlayer, activePlayer, status) {
    let gamePlayers = [{
        color: 'black',
        name: blackSidePlayer,
        isActive: blackSidePlayer === activePlayer && status === 'Playing',
        isWinner: status === 'BlackSideWon'
    }, {
        color: 'white',
        name: whiteSidePlayer,
        isActive: whiteSidePlayer === activePlayer && status === 'Playing',
        isWinner: status === 'WhiteSideWon'
    }];
    return {
        type: types.GET_CURRENT_PLAYERS,
        gamePlayers: gamePlayers
    };
}
