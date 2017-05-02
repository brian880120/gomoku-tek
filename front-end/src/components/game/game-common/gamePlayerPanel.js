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
            currentUser = this.props.auth.user.username;
        }
        function displayPlayer(player) {
            let isCurrentPlayer = false;
            if (currentUser === player.name) {
                isCurrentPlayer = true;
            }
            if (player.name) {
                return (
                    <PlayerCard key={player.name}
                            playerName={player.name}
                            isActive={player.isActive}
                            isWinner={player.isWinner}
                            playerSide={player.color}
                            isCurrentPlayer={isCurrentPlayer} />
                );
            }
        }
        return (
            <div className="card-panel">
                <Card.Group>
                    {this.props.players.map(displayPlayer, this)}
                </Card.Group>
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
