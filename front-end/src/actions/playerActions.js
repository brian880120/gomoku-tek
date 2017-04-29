import * as types from './actionTypes';

export function getCurrentPlayers(blackSidePlayer, whiteSidePlayer) {
    let gamePlayers = [blackSidePlayer, whiteSidePlayer];
    return {
        type: types.GET_CURRENT_PLAYERS,
        gamePlayers: gamePlayers
    };
}
