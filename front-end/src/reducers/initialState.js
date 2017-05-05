import { getInitialGameStatus } from '../api/apiConfig';

export default {
    gameStatus: getInitialGameStatus(),
    auth: {
        isFetching: false,
        isAuthenticated: localStorage.getItem('id_token') ? true : false
    },
    players: []
};
