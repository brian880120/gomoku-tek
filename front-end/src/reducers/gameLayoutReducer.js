import * as types from '../actions/actionTypes';

function gameLayoutReducer(state = [], action) {
    switch(action.type) {
        case types.LOAD_GAME_LAYOUT_SUCCESS:
            return action.layoutData;

        default:
            return state;
    }
}

export default gameLayoutReducer;
