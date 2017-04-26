import * as types from './actionTypes';


export function getCurrentPlayers(playersData) {
    return {
        type: types.GET_CURRENT_PLAYERS,
        data: playersData
    };
}
