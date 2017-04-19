import React from 'react';
import PropTypes from 'prop-types';
import * as _ from 'lodash';

class GameUnit extends React.Component {
    constructor() {
        super();
        this.state = {
            isMouseIn: false
        };
        this.onMouseEnter = this.onMouseEnter.bind(this);
        this.onMouseLeave = this.onMouseLeave.bind(this);
    }

    onMouseEnter() {
        this.setState({
            isMouseIn: true
        });
    }

    onMouseLeave() {
        this.setState({
            isMouseIn: false
        });
    }

    render() {
        var unitId = this.props.unitId;
        var columnStatusData = this.props.columnStatusData;
        var targetUnit = _.find(columnStatusData, function(data) {
            return data.unitId === unitId;
        });

        return (
            <div className="unit"
                onMouseEnter={this.onMouseEnter}
                onMouseLeave={this.onMouseLeave}>
                <div className="cell">
                    <div>
                        <div className="rect"></div>
                        <div className="rect"></div>
                    </div>
                    <div>
                        <div className="rect"></div>
                        <div className="rect"></div>
                    </div>
                </div>
                {
                    this.state.isMouseIn ?
                        <div className="piece-shadow" onClick={this.props.handleUnitClick.bind(this, this.props.columnId, this.props.unitId)}></div> :
                        <div className="piece-hide"></div>
                }
                {
                    targetUnit ?
                        <div className="piece"></div> :
                        <div className="piece-hide"></div>
                }
            </div>
        );
    }
}

GameUnit.propTypes = {
    columnId: PropTypes.string.isRequired,
    unitId: PropTypes.string.isRequired,
    columnStatusData: PropTypes.array.isRequired,
    handleUnitClick: PropTypes.func.isRequired
};

export default GameUnit;
