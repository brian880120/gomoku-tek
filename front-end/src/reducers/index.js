import { combineReducers } from 'redux';
import gameLayout from './gameLayoutReducer';
import gameStatus from './gameStatusReducer';
import gamePlayers from './playerReducer';
import auth from './authReducer';

const rootReducer = combineReducers({
    gameLayout,
    gameStatus,
    gamePlayers,
    auth
});

export default rootReducer;
