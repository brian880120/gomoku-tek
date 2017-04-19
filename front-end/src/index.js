import 'babel-polyfill';
import React from 'react';
import { render } from 'react-dom';
import { Router, browserHistory } from 'react-router';
import routes from './routes';

import { Provider } from 'react-redux';
import configureStore from './store/configureStore';
import { loadGameLayout, initializeGameStatus } from './actions/gameActions';

import './styles/styles.css';
import '../node_modules/bootstrap/dist/css/bootstrap.min.css';

const store = configureStore();
store.dispatch(loadGameLayout());
store.dispatch(initializeGameStatus());

render(
    <Provider store={store}>
        <Router history={browserHistory} routes={routes} />
    </Provider>,
    document.getElementById('app')
);
