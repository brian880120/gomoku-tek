import * as types from './actionTypes';
import axios from 'axios-es6';

const BASE_URL = 'http://localhost:5000/api/';

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

export function loginUser(creds) {
    let config = {
        method: 'post',
        url: BASE_URL + 'authentication/token',
        data: {
            userName: creds.username
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
                    dispatch(receiveLogin(res.data.token));
                } else {
                    dispatch(loginInError(res.data));
                    return Promise.reject();
                }
            }
        }, function(err) {
            console.log(err);
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

export function logoutUser() {
    return dispatch => {
        dispatch(requestLogout());
        localStorage.removeItem('id_token');
        dispatch(receiveLogout());
    };
}
