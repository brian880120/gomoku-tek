import * as types from './actionTypes';

export function getCurrentPlayers(blackSidePlayer, whiteSidePlayer) {
    let gamePlayers = [{
        color: 'black',
        name: blackSidePlayer
    }, {
        color: 'white',
        name: whiteSidePlayer
    }];
    return {
        type: types.GET_CURRENT_PLAYERS,
        gamePlayers: gamePlayers
    };
}
