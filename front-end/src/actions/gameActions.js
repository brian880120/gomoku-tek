import * as types from './actionTypes';
import gameApi from '../api/mockGameApi';

export function loadGameLayoutSuccess(gameLayoutData) {
    return {
        type: types.LOAD_GAME_LAYOUT_SUCCESS ,
        layoutData: gameLayoutData
    };
}

export function loadGameLayout() {
    return function(dispatch) {
        return gameApi.getGameLayoutData().then(gameLayoutData => {
            dispatch(loadGameLayoutSuccess(gameLayoutData));
        }).catch(error => {
            throw(error);
        });
    };
}
