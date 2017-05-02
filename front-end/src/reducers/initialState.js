export default {
    gameStatus: [],
    auth: {
        isFetching: false,
        isAuthenticated: localStorage.getItem('id_token') ? true : false
    },
    players: []
};
