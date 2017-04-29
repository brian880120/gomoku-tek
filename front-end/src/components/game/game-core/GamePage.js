import React from 'react';
import PropTypes from 'prop-types';
import axios from 'axios-es6';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import GameColumn from './GameColumn';
import gameApi from '../../../api/mockGameApi';
import * as gameActions from '../../../actions/gameActions';
import PlayerPanel from '../game-common/gamePlayerPanel';
import { BASE_URL } from '../../../api/apiConfig';
import * as _ from 'lodash';

class GamePage extends React.Component {
    constructor(props, context) {
        super(props, context);
        this.handleUnitClick = this.handleUnitClick.bind(this);
    }

    componentDidMount() {
        if (this.props.auth.isAuthenticated) {
            let config = {
                method: 'get',
                url: BASE_URL + 'gameMoves',
                headers: {
                    Authorization: 'bearer ' + this.props.auth.id_token
                }
            };
            this.props.actions.getGameStatus(config);
        }
    }

    handleUnitClick(columnId, unitId, event) {
        event.preventDefault();
        if (!this.props.auth.isAuthenticated) {
            return;
        }
        let config = {
            method: 'post',
            url: BASE_URL + 'gamemoves',
            data: {
                columnIndex: columnId,
                rowIndex: unitId
            },
            headers: {
                Authorization: 'bearer ' + this.props.auth.id_token
            }
        };

        let occupiedPositions = this.props.statusData;
        axios(config).then(function(response) {
            gameApi.checkWinner(occupiedPositions, {
                columnId: response.data.columnIndex,
                unitId: response.data.rowIndex,
                color: response.data.colorInString
            });
        });
    }

    render() {
        const layoutData = this.props.layoutData;
        let statusData = this.props.statusData;
        let isAuthenticated = this.props.auth.isAuthenticated;
        if (!isAuthenticated) {
            statusData = [];
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
                        {layoutData.map(columnData =>
                            <GameColumn key={columnData.id}
                                    statusData={statusData}
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
    layoutData: PropTypes.array.isRequired,
    statusData: PropTypes.array.isRequired,
    actions: PropTypes.object.isRequired,
    auth: PropTypes.object.isRequired
};

function mapStateToProps(state, ownProps) {
    let gameStatus = state.gameStatus;
    let parsedGameStatus = [];
    _.forEach(gameStatus, function(status) {
        let parsedStatus = {
            color: status.colorInString,
            columnId: status.columnIndex,
            unitId: status.rowIndex
        };
        parsedGameStatus.push(parsedStatus);
    });
    return {
        layoutData: state.gameLayout,
        statusData: parsedGameStatus,
        auth: state.auth
    };
}

function mapDispatchToProps(dispatch) {
    return {
        actions: bindActionCreators(gameActions, dispatch)
    };
}

export default connect(mapStateToProps, mapDispatchToProps)(GamePage);
