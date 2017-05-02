import * as types from '../actions/actionTypes';
import initialState from './initialState';
import * as _ from 'lodash';

function gameStatusReducer(state = initialState.gameStatus, action) {
    switch(action.type) {
        case types.GET_GAME_STATUS_SUCCESS:
            return action.gameStatusData.data;

        case types.UPDATE_GAME_STATUS:
            return [
                ...state,
                Object.assign({}, action.gameStatusData)
            ];

        case types.DELETE_GAME_STATUS:
            return action.gameStatusData;

        case types.CLEAN_GAME_STATUS:
            return action.gameStatusData;

        default:
            return state;
    }
}

export default gameStatusReducer;
