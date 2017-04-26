import 'babel-polyfill';
import React from 'react';
import { render } from 'react-dom';
import { Router, browserHistory } from 'react-router';
import routes from './routes';
import io from 'socket.io-client';

import { Provider } from 'react-redux';
import configureStore from './store/configureStore';
import { loadGameLayout, initializeGameStatus, updateGameStatus } from './actions/gameActions';

import './styles/styles.css';
import '../node_modules/bootstrap/dist/css/bootstrap.min.css';
import '../node_modules/semantic-ui/dist/semantic.min.css';

const connectionUrl = 'ws://localhost:5000/ws';
const socket = new WebSocket(connectionUrl);
const store = configureStore();
store.dispatch(loadGameLayout());
store.dispatch(initializeGameStatus());

socket.onmessage = function(event) {
    let moveData = JSON.parse(event.data);
    let parsedMoveData = {
        columnId: moveData.columnIndex,
        unitId: moveData.rowIndex,
        color: moveData.colorInString
    };
    store.dispatch(updateGameStatus(parsedMoveData));
};

render(
    <Provider store={store}>
        <Router history={browserHistory} routes={routes} />
    </Provider>,
    document.getElementById('app')
);
