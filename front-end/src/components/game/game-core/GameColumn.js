import React from 'react';
import GameUnit from './GameUnit';
import PropTypes from 'prop-types';
import * as _ from 'lodash';

class GameColumn extends React.Component {
    render() {
        let columnIndex = this.props.columnIndex;
        let columnData = this.props.columnData;
        return (
            <div className="column">
                {columnData.map(data =>
                    <GameUnit key={columnData.indexOf(data)}
                            rowIndex={columnData.indexOf(data)}
                            columnIndex={columnIndex}
                            handleUnitClick={this.props.handleUnitClick}
                            isAuthenticated={this.props.isAuthenticated} />
                )}
            </div>
        );
    }
}

GameColumn.propTypes = {
    columnIndex: PropTypes.number.isRequired,
    columnData: PropTypes.array.isRequired,
    handleUnitClick: PropTypes.func.isRequired,
    isAuthenticated: PropTypes.bool.isRequired
};

export default GameColumn;
