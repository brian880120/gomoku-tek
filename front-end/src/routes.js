import React from 'react';
import { Route, IndexRoute } from 'react-router';

import App from './components/App';
import LoginPage from './components/login/LoginPage';
import GamePage from './components/game/GamePage';

export default (
    <Route path="/" component={App}>
        <IndexRoute component={GamePage} />
    </Route>
);
