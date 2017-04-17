import React from 'react';
import PropTypes from 'prop-types';
import COLOR from './PieceColor';

class GameUnit extends React.Component {
    constructor() {
        super();
        this.state = {
            isOccupied: null
        };
        this.onCellClick = this.onCellClick.bind(this);
        this.onMouseEnter = this.onMouseEnter.bind(this);
        this.onMouseLeave = this.onMouseLeave.bind(this);
    }

    componentWillMount() {
        this.setState({
            isOccupied: false
        });
    }

    onCellClick() {
        console.log('test');
        this.setState({
            isOccupied: true
        });
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
        return (
            <div className="unit"
                onMouseEnter={this.onMouseEnter}
                onMouseLeave={this.onMouseLeave}>
                <div className="cell" onClick={this.onCellClick}>
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
                    this.state.isOccupied ?
                        <div className="piece"></div> :
                        <div className="piece-hide"></div>
                }
                {
                    this.state.isMouseIn ?
                        <div className="piece-shadow"></div> :
                        <div className="piece-shadow-hide"></div>
                }
            </div>
        );
    }
}

export default GameUnit;
