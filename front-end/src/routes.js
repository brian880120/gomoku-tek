import React from 'react';
import { Route, IndexRoute } from 'react-router';

import App from './components/App';
import GamePage from './components/game/game-core/GamePage';

export default (
    <Route path="/" component={App}>
        <IndexRoute component={GamePage} />
    </Route>
);
