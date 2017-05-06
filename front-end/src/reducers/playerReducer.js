import * as types from '../actions/actionTypes';
import initialState from './initialState';

function gamePlayerReducer(state = initialState.players, action) {
    switch(action.type) {
        case types.GET_CURRENT_PLAYERS:
            return Object.assign([], action.gamePlayers);

        default:
            return state;
    }
}

export default gamePlayerReducer;
