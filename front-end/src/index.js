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

import '../node_modules/bootstrap/dist/css/bootstrap.min.css';
import '../node_modules/semantic-ui/dist/semantic.min.css';
import './styles/styles.css';

const connectionUrl = 'ws://192.168.2.13:5000/ws';
const socket = new WebSocket(connectionUrl);
const store = configureStore();
store.dispatch(initAuthStatus());
store.dispatch(loadGameLayout());
socket.onmessage = function(event) {
    console.log(event);
    let moveData = JSON.parse(event.data);
    store.dispatch(updateGameStatus(moveData));
    store.dispatch(getCurrentPlayers(moveData.blackSidePlayer, moveData.whiteSidePlayer));
};

render(
    <Provider store={store}>
        <Router history={browserHistory} routes={routes} />
    </Provider>,
    document.getElementById('app')
);
