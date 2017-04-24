import { combineReducers } from 'redux';
import gameLayout from './gameLayoutReducer';
import gameStatus from './gameStatusReducer';
import auth from './authReducer';

const rootReducer = combineReducers({
    gameLayout,
    gameStatus,
    auth
});

export default rootReducer;
