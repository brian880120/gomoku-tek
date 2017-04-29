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
        if (this.props.isAuthenticated) {
            this.setState({
                isMouseIn: true
            });
        }
    }

    onMouseLeave() {
        this.setState({
            isMouseIn: false
        });
    }

    render() {
        let columnId = parseInt(this.props.unitId.split('-')[0]);
        let unitId = parseInt(this.props.unitId.split('-')[1]);
        let columnStatusData = this.props.columnStatusData;
        let targetUnit = _.find(columnStatusData, function(data) {
            return data.unitId == unitId && data.columnId == columnId;
        });
        let pieceClass = '';
        if (targetUnit) {
            pieceClass = 'piece ' + targetUnit.color;
        }

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
                        <div className={pieceClass}></div> :
                        <div className="piece-hide"></div>
                }
            </div>
        );
    }
}

GameUnit.propTypes = {
    columnId: PropTypes.number.isRequired,
    unitId: PropTypes.string.isRequired,
    columnStatusData: PropTypes.array.isRequired,
    handleUnitClick: PropTypes.func.isRequired,
    isAuthenticated: PropTypes.bool.isRequired
};

export default GameUnit;
