const BASE_URL = 'http://localhost:5000/api/';

export default {
    authTokenConfig: {
        method: 'post',
        url: BASE_URL + 'authentication/token',
        data: {},
        header: {
            'Content-Type': 'application/json'
        }
    },
    gameMoveConfig: {
        delete: {
            method: 'delete',
            url: BASE_URL + 'gameMoves',
            headers: {}
        },
        get: {
            method: 'get',
            url: BASE_URL + 'gameMoves',
            headers: {}
        }
    },
    singleMoveConfig: {
        method: 'post',
        url: BASE_URL + 'gamemoves',
        data: {},
        headers: {}
    }
};
