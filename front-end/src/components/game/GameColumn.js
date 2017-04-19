import React from 'react';
import GameUnit from './GameUnit';
import PropTypes from 'prop-types';
import * as _ from 'lodash';

class GameColumn extends React.Component {
    render() {
        var statusData = this.props.statusData;
        var columnId = this.props.columnData.id;
        var columnStatusData = _.filter(statusData, function(data) {
            return columnId === data.columnId;
        });

        return (
            <div className="column">
                {this.props.columnData.data.map(data =>
                    <GameUnit key={data.id}
                            unitId={data.id}
                            columnStatusData={columnStatusData}
                            columnId={this.props.columnData.id}
                            handleUnitClick={this.props.handleUnitClick} />
                )}
            </div>
        );
    }
}

GameColumn.propTypes = {
    columnData: PropTypes.object.isRequired,
    statusData: PropTypes.array.isRequired,
    handleUnitClick: PropTypes.func.isRequired
};

export default GameColumn;
