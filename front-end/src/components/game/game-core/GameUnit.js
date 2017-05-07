import React from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import * as _ from 'lodash';
import { BOARD_SIZE } from '../../../api/apiConfig';

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
        let columnIndex = this.props.columnIndex;
        let rowIndex = this.props.rowIndex;
        let cellClass = 'cell';
        if (columnIndex === 0) {
            cellClass = 'cell first-column-cells';
        }
        if (columnIndex === BOARD_SIZE - 1) {
            cellClass = 'cell last-column-cells';
        }
        if (rowIndex === 0) {
            cellClass = 'cell first-row-cells';
        }
        if (rowIndex === BOARD_SIZE - 1) {
            cellClass = 'cell last-row-cells';
        }
        if (columnIndex === 0 && rowIndex === 0) {
            cellClass = 'cell first-column-cells first-row-cells';
        }
        if (columnIndex === 0 && rowIndex === BOARD_SIZE - 1) {
            cellClass = 'cell first-column-cells last-row-cells';
        }
        if (columnIndex === BOARD_SIZE - 1 && rowIndex === 0) {
            cellClass = 'cell last-column-cells first-row-cells';
        }
        if (columnIndex === BOARD_SIZE - 1 && rowIndex === BOARD_SIZE - 1) {
            cellClass = 'cell last-column-cells last-row-cells';
        }
        let unitStatus = this.props.unitStatus;
        let pieceClass = '';
        if (unitStatus.color) {
            pieceClass = 'piece ' + unitStatus.color;
        }

        return (
            <div className="unit"
                onMouseEnter={this.onMouseEnter}
                onMouseLeave={this.onMouseLeave}>
                <div className={cellClass}>
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
                    unitStatus ?
                        <div className={pieceClass}></div> :
                        <div className="piece-hide"></div>
                }
            </div>
        );
    }
}

GameUnit.propTypes = {
    rowIndex: PropTypes.number.isRequired,
    columnIndex: PropTypes.number.isRequired,
    unitStatus: PropTypes.object.isRequired,
    handleUnitClick: PropTypes.func.isRequired,
    isAuthenticated: PropTypes.bool.isRequired
};

function mapStateToProps(state, ownProps) {
    return {
        unitStatus: state.gameStatus[ownProps.columnIndex][ownProps.rowIndex]
    };
}

export default connect(mapStateToProps)(GameUnit);
