import delay from './delay';
import * as _ from 'lodash';

const MAX_ROW_INDEX = 15;
const MAX_COL_INDEX = 18;

const gameLayoutData = [{
    id: 1,
    data: [{
        id: '1-1'
    }, {
        id: '1-2'
    }, {
        id: '1-3'
    }, {
        id: '1-4'
    }, {
        id: '1-5'
    }, {
        id: '1-6'
    }, {
        id: '1-7'
    }, {
        id: '1-8'
    }, {
        id: '1-9'
    }, {
        id: '1-10'
    }, {
        id: '1-11'
    }, {
        id: '1-12'
    }, {
        id: '1-13'
    }, {
        id: '1-14'
    }, {
        id: '1-15'
    }]
}, {
    id: 2,
    data: [
        {
            id: '2-1'
        },
        {
            id: '2-2'
        },
        {
            id: '2-3'
        },
        {
            id: '2-4'
        },
        {
            id: '2-5'
        },
        {
            id: '2-6'
        },
        {
            id: '2-7'
        },
        {
            id: '2-8'
        },
        {
            id: '2-9'
        },
        {
            id: '2-10'
        },
        {
            id: '2-11'
        },
        {
            id: '2-12'
        },
        {
            id: '2-13'
        },
        {
            id: '2-14'
        },
        {
            id: '2-15'
        }
    ]
}, {
    id: 3,
    data: [
        {
            id: '3-1'
        },
        {
            id: '3-2'
        },
        {
            id: '3-3'
        },
        {
            id: '3-4'
        },
        {
            id: '3-5'
        },
        {
            id: '3-6'
        },
        {
            id: '3-7'
        },
        {
            id: '3-8'
        },
        {
            id: '3-9'
        },
        {
            id: '3-10'
        },
        {
            id: '3-11'
        },
        {
            id: '3-12'
        },
        {
            id: '3-13'
        },
        {
            id: '3-14'
        },
        {
            id: '3-15'
        }
    ]
}, {
    id: 4,
    data: [
        {
            id: '4-1'
        },
        {
            id: '4-2'
        },
        {
            id: '4-3'
        },
        {
            id: '4-4'
        },
        {
            id: '4-5'
        },
        {
            id: '4-6'
        },
        {
            id: '4-7'
        },
        {
            id: '4-8'
        },
        {
            id: '4-9'
        },
        {
            id: '4-10'
        },
        {
            id: '4-11'
        },
        {
            id: '4-12'
        },
        {
            id: '4-13'
        },
        {
            id: '4-14'
        },
        {
            id: '4-15'
        }
    ]
}, {
    id: 5,
    data: [
        {
            id: '5-1'
        },
        {
            id: '5-2'
        },
        {
            id: '5-3'
        },
        {
            id: '5-4'
        },
        {
            id: '5-5'
        },
        {
            id: '5-6'
        },
        {
            id: '5-7'
        },
        {
            id: '5-8'
        },
        {
            id: '5-9'
        },
        {
            id: '5-10'
        },
        {
            id: '5-11'
        },
        {
            id: '5-12'
        },
        {
            id: '5-13'
        },
        {
            id: '5-14'
        },
        {
            id: '5-15'
        }
    ]
}, {
    id: 6,
    data: [
        {
            id: '6-1'
        },
        {
            id: '6-2'
        },
        {
            id: '6-3'
        },
        {
            id: '6-4'
        },
        {
            id: '6-5'
        },
        {
            id: '6-6'
        },
        {
            id: '6-7'
        },
        {
            id: '6-8'
        },
        {
            id: '6-9'
        },
        {
            id: '6-10'
        },
        {
            id: '6-11'
        },
        {
            id: '6-12'
        },
        {
            id: '6-13'
        },
        {
            id: '6-14'
        },
        {
            id: '6-15'
        }
    ]
}, {
    id: 7,
    data: [
        {
            id: '7-1'
        },
        {
            id: '7-2'
        },
        {
            id: '7-3'
        },
        {
            id: '7-4'
        },
        {
            id: '7-5'
        },
        {
            id: '7-6'
        },
        {
            id: '7-7'
        },
        {
            id: '7-8'
        },
        {
            id: '7-9'
        },
        {
            id: '7-10'
        },
        {
            id: '7-11'
        },
        {
            id: '7-12'
        },
        {
            id: '7-13'
        },
        {
            id: '7-14'
        },
        {
            id: '7-15'
        }
    ]
}, {
    id: 8,
    data: [
        {
            id: '8-1'
        },
        {
            id: '8-2'
        },
        {
            id: '8-3'
        },
        {
            id: '8-4'
        },
        {
            id: '8-5'
        },
        {
            id: '8-6'
        },
        {
            id: '8-7'
        },
        {
            id: '8-8'
        },
        {
            id: '8-9'
        },
        {
            id: '8-10'
        },
        {
            id: '8-11'
        },
        {
            id: '8-12'
        },
        {
            id: '8-13'
        },
        {
            id: '8-14'
        },
        {
            id: '8-15'
        }
    ]
}, {
    id: 9,
    data: [
        {
            id: '9-1'
        },
        {
            id: '9-2'
        },
        {
            id: '9-3'
        },
        {
            id: '9-4'
        },
        {
            id: '9-5'
        },
        {
            id: '9-6'
        },
        {
            id: '9-7'
        },
        {
            id: '9-8'
        },
        {
            id: '9-9'
        },
        {
            id: '9-10'
        },
        {
            id: '9-11'
        },
        {
            id: '9-12'
        },
        {
            id: '9-13'
        },
        {
            id: '9-14'
        },
        {
            id: '9-15'
        }
    ]
}, {
    id: 10,
    data: [
        {
            id: '10-1'
        },
        {
            id: '10-2'
        },
        {
            id: '10-3'
        },
        {
            id: '10-4'
        },
        {
            id: '10-5'
        },
        {
            id: '10-6'
        },
        {
            id: '10-7'
        },
        {
            id: '10-8'
        },
        {
            id: '10-9'
        },
        {
            id: '10-10'
        },
        {
            id: '10-11'
        },
        {
            id: '10-12'
        },
        {
            id: '10-13'
        },
        {
            id: '10-14'
        },
        {
            id: '10-15'
        }
    ]
}, {
    id: 11,
    data: [
        {
            id: '11-1'
        },
        {
            id: '11-2'
        },
        {
            id: '11-3'
        },
        {
            id: '11-4'
        },
        {
            id: '11-5'
        },
        {
            id: '11-6'
        },
        {
            id: '11-7'
        },
        {
            id: '11-8'
        },
        {
            id: '11-9'
        },
        {
            id: '11-10'
        },
        {
            id: '11-11'
        },
        {
            id: '11-12'
        },
        {
            id: '11-13'
        },
        {
            id: '11-14'
        },
        {
            id: '11-15'
        }
    ]
}, {
    id: 12,
    data: [
        {
            id: '12-1'
        },
        {
            id: '12-2'
        },
        {
            id: '12-3'
        },
        {
            id: '12-4'
        },
        {
            id: '12-5'
        },
        {
            id: '12-6'
        },
        {
            id: '12-7'
        },
        {
            id: '12-8'
        },
        {
            id: '12-9'
        },
        {
            id: '12-10'
        },
        {
            id: '12-11'
        },
        {
            id: '12-12'
        },
        {
            id: '12-13'
        },
        {
            id: '12-14'
        },
        {
            id: '12-15'
        }
    ]
}, {
    id: 13,
    data: [
        {
            id: '13-1'
        },
        {
            id: '13-2'
        },
        {
            id: '13-3'
        },
        {
            id: '13-4'
        },
        {
            id: '13-5'
        },
        {
            id: '13-6'
        },
        {
            id: '13-7'
        },
        {
            id: '13-8'
        },
        {
            id: '13-9'
        },
        {
            id: '13-10'
        },
        {
            id: '13-11'
        },
        {
            id: '13-12'
        },
        {
            id: '13-13'
        },
        {
            id: '13-14'
        },
        {
            id: '13-15'
        }
    ]
}, {
    id: 14,
    data: [
        {
            id: '14-1'
        },
        {
            id: '14-2'
        },
        {
            id: '14-3'
        },
        {
            id: '14-4'
        },
        {
            id: '14-5'
        },
        {
            id: '14-6'
        },
        {
            id: '14-7'
        },
        {
            id: '14-8'
        },
        {
            id: '14-9'
        },
        {
            id: '14-10'
        },
        {
            id: '14-11'
        },
        {
            id: '14-12'
        },
        {
            id: '14-13'
        },
        {
            id: '14-14'
        },
        {
            id: '14-15'
        }
    ]
}, {
    id: 15,
    data: [
        {
            id: '15-1'
        },
        {
            id: '15-2'
        },
        {
            id: '15-3'
        },
        {
            id: '15-4'
        },
        {
            id: '15-5'
        },
        {
            id: '15-6'
        },
        {
            id: '15-7'
        },
        {
            id: '15-8'
        },
        {
            id: '15-9'
        },
        {
            id: '15-10'
        },
        {
            id: '15-11'
        },
        {
            id: '15-12'
        },
        {
            id: '15-13'
        },
        {
            id: '15-14'
        },
        {
            id: '15-15'
        }
    ]
}, {
    id: 16,
    data: [
        {
            id: '16-1'
        },
        {
            id: '16-2'
        },
        {
            id: '16-3'
        },
        {
            id: '16-4'
        },
        {
            id: '16-5'
        },
        {
            id: '16-6'
        },
        {
            id: '16-7'
        },
        {
            id: '16-8'
        },
        {
            id: '16-9'
        },
        {
            id: '16-10'
        },
        {
            id: '16-11'
        },
        {
            id: '16-12'
        },
        {
            id: '16-13'
        },
        {
            id: '16-14'
        },
        {
            id: '16-15'
        }
    ]
}, {
    id: 17,
    data: [
        {
            id: '17-1'
        },
        {
            id: '17-2'
        },
        {
            id: '17-3'
        },
        {
            id: '17-4'
        },
        {
            id: '17-5'
        },
        {
            id: '17-6'
        },
        {
            id: '17-7'
        },
        {
            id: '17-8'
        },
        {
            id: '17-9'
        },
        {
            id: '17-10'
        },
        {
            id: '17-11'
        },
        {
            id: '17-12'
        },
        {
            id: '17-13'
        },
        {
            id: '17-14'
        },
        {
            id: '17-15'
        }
    ]
}, {
    id: 18,
    data: [
        {
            id: '18-1'
        },
        {
            id: '18-2'
        },
        {
            id: '18-3'
        },
        {
            id: '18-4'
        },
        {
            id: '18-5'
        },
        {
            id: '18-6'
        },
        {
            id: '18-7'
        },
        {
            id: '18-8'
        },
        {
            id: '18-9'
        },
        {
            id: '18-10'
        },
        {
            id: '18-11'
        },
        {
            id: '18-12'
        },
        {
            id: '18-13'
        },
        {
            id: '18-14'
        },
        {
            id: '18-15'
        }
    ]
}];

function findMatcher(occupiedPositions, currentMove, matchString) {
    return _.find(occupiedPositions, function(pos) {
        return pos.unitId === matchString && pos.color === currentMove.color;
    });
}

function checkHorizontalConsecutive(columnIndex, rowIndex, occupiedPositions, currentMove) {
    let count = 1;
    for (let i = columnIndex + 1; i <= MAX_COL_INDEX; i++) {
        let matchString = i + '-' + rowIndex;
        let hasFound = findMatcher(occupiedPositions, currentMove, matchString);
        if (hasFound) {
            count++;
        } else {
            break;
        }
    }

    for (let i = columnIndex - 1; i >= 1; i--) {
        let matchString = i + '-' + rowIndex;
        let hasFound = findMatcher(occupiedPositions, currentMove, matchString);
        if (hasFound) {
            count++;
        } else {
            break;
        }
    }
    if (count === 5) {
        return true;
    }
    return false;
}

function checkVerticalConsecutive(columnIndex, rowIndex, occupiedPositions, currentMove) {
    let count = 1;
    for (let i = rowIndex + 1; i <= MAX_ROW_INDEX; i++) {
        let matchString = columnIndex + '-' + i;
        let hasFound = findMatcher(occupiedPositions, currentMove, matchString);
        if (hasFound) {
            count++;
        } else {
            break;
        }
    }

    for (let i = rowIndex - 1; i >= 1; i--) {
        let matchString = columnIndex + '-' + i;
        let hasFound = findMatcher(occupiedPositions, currentMove, matchString);
        if (hasFound) {
            count++;
        } else {
            break;
        }
    }
    if (count === 5) {
        return true;
    }
    return false;
}

function checkForwardSlashConsecutive(columnIndex, rowIndex, occupiedPositions, currentMove) {
    let count = 1;
    for (let i = columnIndex + 1, j = rowIndex + 1; i <= MAX_COL_INDEX, j <= MAX_ROW_INDEX; i++, j++) {
        let matchString = i + '-' + j;
        let hasFound = findMatcher(occupiedPositions, currentMove, matchString);
        if (hasFound) {
            count++;
        } else {
            break;
        }
    }

    for (let i = columnIndex - 1, j = rowIndex - 1; i >= 1, j >= 1; i--, j--) {
        let matchString = i + '-' + j;
        let hasFound = findMatcher(occupiedPositions, currentMove, matchString);
        if (hasFound) {
            count++;
        } else {
            break;
        }
    }
    if (count === 5) {
        return true;
    }
    return false;
}

function checkBackwardSlashConsecutive(columnIndex, rowIndex, occupiedPositions, currentMove) {
    let count = 1;
    for (let i = columnIndex + 1, j = rowIndex - 1; i <= MAX_COL_INDEX, j >= 1; i++, j--) {
        let matchString = i + '-' + j;
        let hasFound = findMatcher(occupiedPositions, currentMove, matchString);
        if (hasFound) {
            count++;
        } else {
            break;
        }
    }

    for (let i = columnIndex - 1, j = rowIndex + 1; i >= 1, j <= MAX_ROW_INDEX; i--, j++) {
        let matchString = i + '-' + j;
        let hasFound = findMatcher(occupiedPositions, currentMove, matchString);
        if (hasFound) {
            count++;
        } else {
            break;
        }
    }
    if (count === 5) {
        return true;
    }
    return false;
}

class GameApi {
    static getGameLayoutData() {
        return new Promise((resolve, reject) => {
            setTimeout(() => {
                resolve(Object.assign([], gameLayoutData));
            }, delay);
        });
    }

    static initializeGameStatus() {
        return new Promise((resolve, reject) => {
            setTimeout(() => {
                resolve([]);
            }, delay);
        });
    }

    static updateGameStatus(columnId, unitId) {
        return new Promise((resolve, reject) => {
            setTimeout(() => {
                resolve({
                    columnId: columnId,
                    unitId: unitId,
                    color: 'BLACK'
                });
            }, delay);
        });
    }

    static checkWinner(occupiedPositions, currentMove) {
        let columnIndex = parseInt(currentMove.unitId.split('-')[0]);
        let rowIndex = parseInt(currentMove.unitId.split('-')[1]);
        if (
            checkHorizontalConsecutive(columnIndex, rowIndex, occupiedPositions, currentMove) ||
            checkVerticalConsecutive(columnIndex, rowIndex, occupiedPositions, currentMove) ||
            checkForwardSlashConsecutive(columnIndex, rowIndex, occupiedPositions, currentMove) ||
            checkBackwardSlashConsecutive(columnIndex, rowIndex, occupiedPositions, currentMove)
        ) {
            alert(currentMove.color + ' wins');
        }
    }
}

export default GameApi;
