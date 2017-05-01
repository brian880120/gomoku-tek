import React from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Image, Button } from 'semantic-ui-react';
import { BASE_URL } from '../../../api/apiConfig';
import * as gameActions from '../../../actions/gameActions';
import * as authActions from '../../../actions/authActions';

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

function mapStateToProps(state, ownProps) {
    return {};
}

function mapDispatchToProps(dispatch) {
    return {
        gameActions: bindActionCreators(gameActions, dispatch),
        authActions: bindActionCreators(authActions, dispatch)
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(GameHeader);
