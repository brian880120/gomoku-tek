import React from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import * as playerActions from '../../../actions/playerActions';
import PlayerCard from './playerCard';

class PlayerPanel extends React.Component {
    render() {
        return (
            <div>
                {this.props.players.map(playerName =>
                    <PlayerCard key={playerName}
                            playerName={playerName} />
                )}
            </div>
        );
    }
}

PlayerPanel.propTypes = {
    players: PropTypes.array.isRequired
};

function mapStateToProps(state, ownProps) {
    return {
        players: state.gamePlayers
    };
}


export default connect(mapStateToProps)(PlayerPanel);
