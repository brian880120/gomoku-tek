import * as types from './actionTypes';
import gameApi from '../api/mockGameApi';

function loadGameLayoutSuccess(gameLayoutData) {
    return {
        type: types.LOAD_GAME_LAYOUT_SUCCESS,
        layoutData: gameLayoutData
    };
}

function initializeGameStatusSuccess(gameStatusData) {
    return {
        type: types.INIT_GAME_STATUS_SUCCESS,
        layoutData: gameStatusData
    };
}

function updateGameStatusSuccess(gameStatusData) {
    return {
        type: types.UPDATE_GAME_STATUS,
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

// Todo: call real api
export function initializeGameStatus() {
    return function(dispatch) {
        return gameApi.initializeGameStatus().then(gameStatusData => {
            dispatch(initializeGameStatusSuccess(gameStatusData));
        }).catch(error => {
            throw(error);
        });
    };
}

export function updateGameStatus(moveData) {
    return {
        type: types.UPDATE_GAME_STATUS,
        gameStatusData: moveData
    };
}
