import * as _ from 'lodash';

export const BASE_URL = 'http://localhost:58124/api/'; //'http://localhost:5000/api/';//  'http://172.27.160.34:5000/api/';
export const WS_BASE_URL = 'ws://localhost:58124/ws';  //'ws://localhost:5000/ws';// 'ws://172.27.160.34:5000/ws';

export const BOARD_SIZE = 19;

export function getInitialGameStatus() {
    let columns = new Array(BOARD_SIZE);
    _.forEach(columns, function(column, index) {
        columns[index] = new Array(BOARD_SIZE);
    });

    for (let i = 0; i < BOARD_SIZE; i++) {
        for (let j = 0; j < BOARD_SIZE; j++) {
            columns[i][j] = {};
        }
    }
    return columns;
}
