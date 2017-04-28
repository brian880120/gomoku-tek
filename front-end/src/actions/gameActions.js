import * as types from './actionTypes';
import axios from 'axios-es6';
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
        gameStatusData: gameStatusData
    };
}

function updateGameStatusSuccess(gameStatusData) {
    return {
        type: types.UPDATE_GAME_STATUS,
        gameStatusData: gameStatusData
    };
}

function deleteGameStatusSuccess() {
    return {
        type: types.DELETE_GAME_STATUS,
        gameStatusData: []
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

export function getGameStatus(config) {
    return function(dispatch) {
        return axios(config).then(function(gameStatusData) {
            dispatch(initializeGameStatusSuccess(gameStatusData));
        }, function(err) {
            throw(err);
        });
    };
}

export function updateGameStatus(moveData) {
    return {
        type: types.UPDATE_GAME_STATUS,
        gameStatusData: moveData
    };
}

export function deleteGameStatus(config) {
    return function(dispatch) {
        return axios(config).then(function() {
            dispatch(deleteGameStatusSuccess());
        }).catch(function(error) {
            throw(error);
        });
    };
}
