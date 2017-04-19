import * as types from '../actions/actionTypes';

function gameStatusReducer(state = [], action) {
    switch(action.type) {
        case types.GET_GAME_STATUS:
            return [
                ...state,
                Object.assign({}, action.gameStatusData)
            ];

        default:
            return state;
    }
}

export default gameStatusReducer;
