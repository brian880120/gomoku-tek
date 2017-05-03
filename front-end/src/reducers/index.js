import { combineReducers } from 'redux';
import gameStatus from './gameStatusReducer';
import gamePlayers from './playerReducer';
import auth from './authReducer';

const rootReducer = combineReducers({
    gameStatus,
    gamePlayers,
    auth
});

export default rootReducer;
