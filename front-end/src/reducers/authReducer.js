import * as types from '../actions/actionTypes';
import initialState from './initialState';

function authReducer(state = initialState.auth, action) {
    switch (action.type) {
        case types.INIT_AUTH_STATUS:
            return {
                isAuthenticated: action.isAuthenticated,
                id_token: action.id_token
            };

        case types.LOGIN_REQUEST:
            return Object.assign({}, state, {
                isFetching: true,
                isAuthenticated: false,
                user: action.creds.username
            });

        case types.LOGIN_SUCCESS:
            return Object.assign({}, state, {
                isFetching: false,
                isAuthenticated: true,
                id_token: action.id_token,
                errorMessage: ''
            });

        case types.LOGIN_FAILURE:
            return Object.assign({}, state, {
                isFetching: false,
                isAuthenticated: false,
                errorMessage: action.message
            });

        case types.LOGOUT_SUCCESS:
            return Object.assign({}, state, {
                isFetching: true,
                isAuthenticated: false
            });

        case types.RESET_GAME:
            return Object.assign({}, state, {
                isAuthenticated: false,
                isFetching: false
            });
        default:
            return state;
    }
}

export default authReducer;
