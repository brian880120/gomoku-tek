import { combineReducers } from 'redux';
import gameLayout from './gameLayoutReducer';
import gameStatus from './gameStatusReducer';

const rootReducer = combineReducers({
    gameLayout,
    gameStatus
});

export default rootReducer;
