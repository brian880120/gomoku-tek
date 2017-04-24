import React from 'react';
import PropTypes from 'prop-types';
import axios from 'axios-es6';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import * as authActions from '../../actions/authActions';
import GameColumn from './GameColumn';

const BASE_URL = 'http://172.27.148.51:5000/api/';

class GamePage extends React.Component {
    constructor(props, context) {
        super(props, context);
        this.state = {
            user: {
                name: '',
                password: ''
            }
        };
        this.handleUnitClick = this.handleUnitClick.bind(this);
        this.onLogin = this.onLogin.bind(this);
        this.clearStorage = this.clearStorage.bind(this);
    }

    componentWillMount() {
        this.setState({
            user: {
                username: 'hong.xu',
                password: 'P@ssw0rd!'
            }
        });
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
                playerName: this.state.user.username
            },
            headers: {
                Authorization: 'bearer ' + this.props.auth.id_token
            }
        };

        axios(config).then(function(response) {
        }).catch(function(error) {
        });
    }

    onLogin() {
        this.props.actions.loginUser(this.state.user);
    }

    clearStorage() {
        localStorage.removeItem('id_token');
    }

    render() {
        const layoutData = this.props.layoutData;
        let statusData = this.props.statusData;
        return (
            <div>
                <button className="btn btn-primary" onClick={this.onLogin}>Login</button>
                <button className="btn btn-danger" onClick={this.clearStorage}>Clear Storage</button>
                <div className="game">
                    {layoutData.map(columnData =>
                        <GameColumn key={columnData.id}
                                statusData={statusData}
                                columnData={columnData}
                                handleUnitClick={this.handleUnitClick}
                                isAuthenticated={this.props.auth.isAuthenticated} />
                    )}
                </div>
            </div>
        );
    }
}

GamePage.propTypes = {
    layoutData: PropTypes.array.isRequired,
    statusData: PropTypes.array.isRequired,
    auth: PropTypes.object.isRequired,
    actions: PropTypes.object.isRequired
};

function mapStateToProps(state, ownProps) {
    return {
        layoutData: state.gameLayout,
        statusData: state.gameStatus,
        auth: state.auth
    };
}

function mapDispatchToProps(dispatch) {
    return {
        actions: bindActionCreators(authActions, dispatch)
    };
}

export default connect(mapStateToProps, mapDispatchToProps)(GamePage);
