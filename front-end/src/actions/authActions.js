import * as types from './actionTypes';
import axios from 'axios-es6';
import { BASE_URL } from '../api/apiConfig';

function requestLogin(creds) {
    return {
        type: types.LOGIN_REQUEST,
        isFetching: true,
        isAuthenticated: false,
        creds: creds
    };
}

function receiveLogin(token) {
    return {
        type: types.LOGIN_SUCCESS,
        isFetching: false,
        isAuthenticated: true,
        id_token: token
    };
}

function loginInError(message) {
    return {
        type: types.LOGIN_FAILURE,
        isFetching: false,
        isAuthenticated: false,
        message: message
    };
}

function receiveLogout() {
    return {
        type: types.LOGOUT_SUCCESS,
        isFetching: false,
        isAuthenticated: false
    };
}

export function initAuthStatus() {
    let token = localStorage.getItem('id_token');
    let auth = {
        type: types.INIT_AUTH_STATUS,
        isAuthenticated: false
    };
    if (token) {
        auth.isAuthenticated = true;
        auth.id_token = token;
    }
    return auth;
}

export function loginUser(creds) {
    let config = {
        method: 'post',
        url: BASE_URL + 'authentication/token',
        data: {
            userName: creds.username,
            gameMode: creds.gamemode
        },
        headers: {
            'Content-Type': 'application/json'
        }
    };

    return function(dispatch) {
        dispatch(requestLogin(creds));
        return axios(config).then(function(res) {
            if (res) {
                if (res.statusText === 'OK') {
                    localStorage.setItem('id_token', res.data.token);
                    localStorage.setItem('username', creds.username);
                    dispatch(receiveLogin(res.data.token));
                } else {
                    dispatch(loginInError(res.data));
                    return Promise.reject();
                }
            }
        }, function(err) {
            throw(err);
        });
    };
}

function requestLogout() {
    return {
        type: types.LOGOUT_REQUEST,
        isFetching: true,
        isAuthenticated: true
    };
}

export function logoutUser(token) {
    let config = {
        method: 'delete',
        url: BASE_URL + 'authentication/token',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': 'bearer ' + token
        }
    };
    return dispatch => {
        dispatch(requestLogout());
        return axios(config).then(function(res) {
            localStorage.removeItem('id_token');
            localStorage.removeItem('username');
            dispatch(receiveLogout());
        }, function(err) {
            throw(err);
        });
    };
}

export function resetGame() {
    localStorage.removeItem('id_token');
    localStorage.removeItem('username');
    return {
        type: types.RESET_GAME
    };
}
