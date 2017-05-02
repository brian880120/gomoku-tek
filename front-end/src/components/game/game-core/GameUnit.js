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
        let columnIndex = this.props.columnIndex;
        let rowIndex = this.props.rowIndex;
        let cellClass = 'cell';
        if (columnIndex === 0) {
            cellClass = 'cell first-column-cells';
        }
        if (columnIndex === 17) {
            cellClass = 'cell last-column-cells';
        }
        if (rowIndex === 0) {
            cellClass = 'cell first-row-cells';
        }
        if (rowIndex === 14) {
            cellClass = 'cell last-row-cells';
        }
        if (columnIndex === 0 && rowIndex === 0) {
            cellClass = 'cell first-column-cells first-row-cells';
        }
        if (columnIndex === 0 && rowIndex === 14) {
            cellClass = 'cell first-column-cells last-row-cells';
        }
        if (columnIndex === 17 && rowIndex === 0) {
            cellClass = 'cell last-column-cells first-row-cells';
        }
        if (columnIndex === 17 && rowIndex === 14) {
            cellClass = 'cell last-column-cells last-row-cells';
        }
        let columnStatusData = this.props.columnStatusData;
        let targetUnit = _.find(columnStatusData, function(data) {
            return data.rowIndex == rowIndex && data.columnIndex == columnIndex;
        });
        let pieceClass = '';
        if (targetUnit) {
            pieceClass = 'piece ' + targetUnit.colorInString;
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
                    targetUnit ?
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
    columnStatusData: PropTypes.array.isRequired,
    handleUnitClick: PropTypes.func.isRequired,
    isAuthenticated: PropTypes.bool.isRequired
};

export default GameUnit;
