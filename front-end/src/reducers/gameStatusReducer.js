import * as types from '../actions/actionTypes';

function gameStatusReducer(state = [], action) {
    switch(action.type) {
        case types.INIT_GAME_STATUS_SUCCESS:
            return [];

        case types.UPDATE_GAME_STATUS:
            return [
                ...state,
                Object.assign({}, action.gameStatusData)
            ];

        default:
            return state;
    }
}

export default gameStatusReducer;
