import * as types from '../actions/actionTypes';
import initialState from './initialState';
import * as _ from 'lodash';

function gameStatusReducer(state = initialState.gameStatus, action) {
    switch(action.type) {
        case types.INIT_GAME_STATUS_SUCCESS:
            return [];

        case types.UPDATE_GAME_STATUS:
            let statusData = [...state, Object.assign({}, action.gameStatusData)];
            return _.filter(statusData, function(data) {
                return !_.isUndefined(data.columnId);
            });

        default:
            return state;
    }
}

export default gameStatusReducer;
