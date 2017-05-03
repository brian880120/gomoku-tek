import * as _ from 'lodash';

export const BASE_URL = 'http://localhost:58124/api/'; // 'http://localhost:5000/api/';//'http://172.27.160.34:5000/api/';
export const WS_BASE_URL = 'ws://localhost:58124/ws'; // 'ws://localhost:5000/ws';//'ws://172.27.160.34:5000/ws';

const MAX_ROW_INDEX = 15;
const MAX_COL_INDEX = 18;

export function getInitialGameStatus() {
    let columns = new Array(MAX_COL_INDEX);
    _.forEach(columns, function(column, index) {
        columns[index] = new Array(MAX_ROW_INDEX);
    });

    for (let i = 0; i < MAX_COL_INDEX; i++) {
        for (let j = 0; j < MAX_ROW_INDEX; j++) {
            columns[i][j] = {};
        }
    }
    return columns;
}
