import 'babel-polyfill';
import React from 'react';
import { render } from 'react-dom';
import { Router, browserHistory } from 'react-router';
import routes from './routes';
import io from 'socket.io-client';

import { Provider } from 'react-redux';
import configureStore from './store/configureStore';
import { loadGameLayout, updateGameStatus } from './actions/gameActions';
import { initAuthStatus } from './actions/authActions';
import { getCurrentPlayers } from './actions/playerActions';
import { WS_BASE_URL } from './api/apiConfig';

import '../node_modules/bootstrap/dist/css/bootstrap.min.css';
import '../node_modules/semantic-ui/dist/semantic.min.css';
import './styles/styles.css';

const socket = new WebSocket(WS_BASE_URL);
const store = configureStore();
store.dispatch(initAuthStatus());
store.dispatch(loadGameLayout());
socket.onmessage = function(event) {
    let messageData = JSON.parse(event.data);
    let gameData = null;
    let moveData = null;
    if (messageData.type === 'Game') {
        gameData = messageData.payload;
        if (gameData.blackSidePlayer || gameData.whiteSidePlayer) {
            store.dispatch(getCurrentPlayers(gameData.blackSidePlayer, gameData.whiteSidePlayer));
        }
    }
    if (messageData.type === 'GameMove') {
        moveData = messageData.payload;
        store.dispatch(updateGameStatus(moveData));
    }
};

render(
    <Provider store={store}>
        <Router history={browserHistory} routes={routes} />
    </Provider>,
    document.getElementById('app')
);
