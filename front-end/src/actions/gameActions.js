import * as types from './actionTypes';
import axios from 'axios-es6';

function getGameStatusSuccess(gameStatusData) {
    return {
        type: types.GET_GAME_STATUS_SUCCESS,
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

export function getGameStatus(config) {
    return function(dispatch) {
        return axios(config).then(function(gameStatusData) {
            dispatch(getGameStatusSuccess(gameStatusData));
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

export function cleanGameStatus() {
    return {
        type: types.CLEAN_GAME_STATUS,
        gameStatusData: []
    };
}
