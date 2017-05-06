import React from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import * as playerActions from '../../../actions/playerActions';
import PlayerCard from './playerCard';
import { Card } from 'semantic-ui-react';

class PlayerPanel extends React.Component {
    render() {
        let currentUser = null;
        if (this.props.auth.user) {
            currentUser = this.props.auth.user;
        }
        function displayPlayer(player) {
            let isCurrentPlayer = false;
            let copyPlayer = Object.assign({}, player);
            if (currentUser === player.name) {
                copyPlayer.name = 'You';
                isCurrentPlayer = true;
            }
            if (player.name) {
                return (
                    <PlayerCard key={copyPlayer.name}
                            playerName={copyPlayer.name}
                            isActive={copyPlayer.isActive}
                            isWinner={copyPlayer.isWinner}
                            playerSide={copyPlayer.color}
                            isCurrentPlayer={isCurrentPlayer} />
                );
            }
        }
        return (
            <div className="card-panel">
                {this.props.players.map(displayPlayer, this)}
            </div>
        );
    }
}

PlayerPanel.propTypes = {
    players: PropTypes.array.isRequired,
    auth: PropTypes.object.isRequired
};

function mapStateToProps(state, ownProps) {
    return {
        auth: state.auth,
        players: state.gamePlayers
    };
}


export default connect(mapStateToProps)(PlayerPanel);
