import * as types from '../actions/actionTypes';
import initialState from './initialState';

function gameLayoutReducer(state = initialState.gameLayout, action) {
    switch(action.type) {
        case types.LOAD_GAME_LAYOUT_SUCCESS:
            return action.layoutData;

        default:
            return state;
    }
}

export default gameLayoutReducer;
