import * as types from '../actions/actionTypes';
import initialState from './initialState';
import * as _ from 'lodash';

function updateGameStatus(moveDataList, stateCopy) {
    _.forEach(moveDataList, function(moveData) {
        let indexToSplit = moveData.columnIndex;
        let rowIndexToSplit = moveData.rowIndex;
        let firstHalf = _.slice(stateCopy, 0, indexToSplit);
        let secondHalf = _.slice(stateCopy, indexToSplit + 1, stateCopy.length);

        let targetColumn = stateCopy[indexToSplit];
        let updatedColumn = [
            ..._.slice(targetColumn, 0, rowIndexToSplit),
            Object.assign({}, targetColumn[rowIndexToSplit], {
                color: moveData.colorInString
            }),
            ..._.slice(targetColumn, rowIndexToSplit + 1, targetColumn.length)
        ];
        stateCopy = [
            ...firstHalf,
            updatedColumn,
            ...secondHalf
        ];
    });
    return stateCopy;
}

function gameStatusReducer(state = initialState.gameStatus, action) {
    switch(action.type) {
        case types.GET_GAME_STATUS_SUCCESS:
            return updateGameStatus(action.gameStatusData.data, Object.assign([], state));

        case types.UPDATE_GAME_STATUS:
            return updateGameStatus([action.gameStatusData], Object.assign([], state));

        case types.DELETE_GAME_STATUS:
            return action.gameStatusData;

        case types.CLEAN_GAME_STATUS:
            return action.gameStatusData;

        default:
            return state;
    }
}

export default gameStatusReducer;
