import React from 'react';
import PropTypes from 'prop-types';
import axios from 'axios-es6';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import GameColumn from './GameColumn';
import * as gameActions from '../../../actions/gameActions';
import PlayerPanel from '../game-common/gamePlayerPanel';
import { BASE_URL, getInitialGameStatus } from '../../../api/apiConfig';
import * as _ from 'lodash';

class GamePage extends React.Component {
    constructor(props, context) {
        super(props, context);
        this.handleUnitClick = this.handleUnitClick.bind(this);
    }

    componentWillMount() {
        let config = {
            method: 'get',
            url: BASE_URL + 'gameMoves',
            headers: {
                Authorization: 'bearer ' + this.props.auth.id_token
            }
        };
        this.props.actions.getGameStatus(config);
    }

    handleUnitClick(columnIndex, rowIndex, event) {
        event.preventDefault();
        if (!this.props.auth.isAuthenticated) {
            return;
        }
        let config = {
            method: 'post',
            url: BASE_URL + 'gamemoves',
            data: { columnIndex, rowIndex },
            headers: {
                Authorization: 'bearer ' + this.props.auth.id_token
            }
        };
        axios(config);
    }

    render() {
        let statusData = this.props.statusData;
        let isAuthenticated = this.props.auth.isAuthenticated;
        if (!isAuthenticated || _.isEmpty(statusData)) {
            statusData = getInitialGameStatus();
        }
        return (
            <div className="container-fluid">
                {
                    isAuthenticated ?
                    <PlayerPanel /> :
                    ''
                }
                <div className="row">
                    <div className="game col-md-8">
                        {statusData.map(columnData =>
                            <GameColumn key={statusData.indexOf(columnData)}
                                    columnIndex={statusData.indexOf(columnData)}
                                    columnData={columnData}
                                    handleUnitClick={this.handleUnitClick}
                                    isAuthenticated={this.props.auth.isAuthenticated} />
                        )}
                    </div>
                </div>
            </div>
        );
    }
}

GamePage.propTypes = {
    statusData: PropTypes.array.isRequired,
    actions: PropTypes.object.isRequired,
    auth: PropTypes.object.isRequired
};

function mapStateToProps(state, ownProps) {
    return {
        statusData: state.gameStatus,
        auth: state.auth
    };
}

function mapDispatchToProps(dispatch) {
    return {
        actions: bindActionCreators(gameActions, dispatch)
    };
}

export default connect(mapStateToProps, mapDispatchToProps)(GamePage);
