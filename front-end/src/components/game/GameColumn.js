import React from 'react';
import GameUnit from './GameUnit';
import PropTypes from 'prop-types';

class GameColumn extends React.Component {
    render() {
        return (
            <div className="column">
                {this.props.columnData.map(data =>
                    <GameUnit key={data.id} />
                )}
            </div>
        );
    }
}

GameColumn.propTypes = {
    columnData: PropTypes.array.isRequired
};

export default GameColumn;
