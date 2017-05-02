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
        let pieceClass = '';
        let unitData = this.props.unitData;
        let columnIndex = this.props.columnIndex;
        let rowIndex = this.props.rowIndex;
        if (unitData.color) {
            pieceClass = 'piece ' + unitData.color;
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
                        <div className="piece-shadow" onClick={this.props.handleUnitClick.bind(this, columnIndex, rowIndex)}></div> :
                        <div className="piece-hide"></div>
                }
                {
                    pieceClass ?
                        <div className={pieceClass}></div> :
                        <div className="piece-hide"></div>
                }
            </div>
        );
    }
}

GameUnit.propTypes = {
    unitData: PropTypes.object.isRequired,
    columnIndex: PropTypes.number.isRequired,
    rowIndex: PropTypes.number.isRequired,
    handleUnitClick: PropTypes.func.isRequired,
    isAuthenticated: PropTypes.bool.isRequired
};

export default GameUnit;
