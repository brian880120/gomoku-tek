import React from 'react';
import GameUnit from './GameUnit';
import PropTypes from 'prop-types';

class GameColumn extends React.Component {
    render() {
        let columnData = this.props.columnData;
        return (
            <div className="column">
                {columnData.map(data =>
                    <GameUnit key={columnData.indexOf(data)}
                            unitData={data}
                            columnIndex={this.props.columnIndex}
                            rowIndex={columnData.indexOf(data)}
                            handleUnitClick={this.props.handleUnitClick}
                            isAuthenticated={this.props.isAuthenticated} />
                )}
            </div>
        );
    }
}

GameColumn.propTypes = {
    columnData: PropTypes.array.isRequired,
    columnIndex: PropTypes.number.isRequired,
    handleUnitClick: PropTypes.func.isRequired,
    isAuthenticated: PropTypes.bool.isRequired
};

export default GameColumn;
