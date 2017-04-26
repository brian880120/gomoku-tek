import * as types from '../actions/actionTypes';
import initialState from './initialState';

function gameLayoutReducer(state = initialState.players, action) {
    switch(action.type) {
        case types.GET_CURRENT_PLAYERS:
            return action.data;

        default:
            return state;
    }
}
