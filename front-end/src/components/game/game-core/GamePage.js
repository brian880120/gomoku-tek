import React from 'react';
import PropTypes from 'prop-types';
import axios from 'axios-es6';
import { connect } from 'react-redux';
import GameColumn from './GameColumn';
import Login from '../game-common/login';
import RequireLogin from '../game-common/loginRequire';

const BASE_URL = 'http://172.27.148.51:5000/api/';

class GamePage extends React.Component {
    constructor(props, context) {
        super(props, context);
        this.handleUnitClick = this.handleUnitClick.bind(this);
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
                rowIndex: unitId,
                playerName: this.props.auth.creds.username
            },
            headers: {
                Authorization: 'bearer ' + this.props.auth.id_token
            }
        };

        axios(config).then(function(response) {
        }).catch(function(error) {
        });
    }

    render() {
        const layoutData = this.props.layoutData;
        let statusData = this.props.statusData;
        let isAuthenticated = this.props.auth.isAuthenticated;
        return (
            <div>
                {
                    isAuthenticated ? '' :
                        <RequireLogin />
                }
                <div>
                    <div className="game">
                        {layoutData.map(columnData =>
                            <GameColumn key={columnData.id}
                                    statusData={statusData}
                                    columnData={columnData}
                                    handleUnitClick={this.handleUnitClick}
                                    isAuthenticated={this.props.auth.isAuthenticated} />
                        )}
                    </div>
                    {
                        isAuthenticated ? '' :
                            <Login />
                    }
                </div>
            </div>
        );
    }
}

GamePage.propTypes = {
    layoutData: PropTypes.array.isRequired,
    statusData: PropTypes.array.isRequired,
    auth: PropTypes.object.isRequired
};

function mapStateToProps(state, ownProps) {
    return {
        layoutData: state.gameLayout,
        statusData: state.gameStatus,
        auth: state.auth
    };
}

export default connect(mapStateToProps)(GamePage);
