import * as types from './actionTypes';
import gameApi from '../api/mockGameApi';

function loadGameLayoutSuccess(gameLayoutData) {
    return {
        type: types.LOAD_GAME_LAYOUT_SUCCESS,
        layoutData: gameLayoutData
    };
}

function getGameStatusSuccess(gameStatusData) {
    return {
        type: types.GET_GAME_STATUS,
        gameStatusData: gameStatusData
    };
}

export function loadGameLayout() {
    return function(dispatch) {
        return gameApi.getGameLayoutData().then(gameLayoutData => {
            dispatch(loadGameLayoutSuccess(gameLayoutData));
        }).catch(error => {
            throw(error);
        });
    };
}

export function getGameStatus() {
    return function(dispatch) {
        return gameApi.getGameStatus().then(gameStatusData => {
            dispatch(getGameStatusSuccess(gameStatusData));
        }).catch(error => {
            throw(error);
        });
    };
}
