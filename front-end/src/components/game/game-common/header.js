import React from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Image, Button } from 'semantic-ui-react';
import { BASE_URL } from '../../../api/apiConfig';
import * as gameActions from '../../../actions/gameActions';

class GameHeader extends React.Component {
    constructor() {
        super();
        this.resetGame = this.resetGame.bind(this);
    }

    resetGame() {
        let config = {
            method: 'delete',
            url: BASE_URL + 'games'
        };
        this.props.gameActions.deleteGameStatus(config);
    }

    render() {
        return (
            <div className="header-area">
                <div className="header-icon"></div>
                <h1>Gomoku</h1>
                <Button basic color="red" className="reset-button" onClick={this.resetGame}>Reset Game</Button>
            </div>
        );
    }
}

GameHeader.propTypes = {
    gameActions: PropTypes.object.isRequired
};

function mapStateToProps(state, ownProps) {
    return {};
}

function mapDispatchToProps(dispatch) {
    return {
        gameActions: bindActionCreators(gameActions, dispatch)
    };
}

export default connect(mapStateToProps, mapDispatchToProps)(GameHeader);
