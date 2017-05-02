import * as types from '../actions/actionTypes';
import initialState from './initialState';

function gameStatusReducer(state=[], action) {
    let stateCopy = null;
    switch(action.type) {
        case types.INIT_GAME_STATUS:
            return action.gameStatusData;

        case types.GET_GAME_STATUS_SUCCESS:
            return action.gameStatusData.data;

        case types.UPDATE_GAME_STATUS:
            stateCopy = Object.assign([], state);
            stateCopy[action.gameStatusData.columnIndex][action.gameStatusData.rowIndex].color = action.gameStatusData.colorInString;
            return stateCopy;

        case types.DELETE_GAME_STATUS:
            return action.gameStatusData;

        case types.CLEAN_GAME_STATUS:
            return action.gameStatusData;

        default:
            return state;
    }
}

export default gameStatusReducer;
