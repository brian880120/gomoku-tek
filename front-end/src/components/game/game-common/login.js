import React from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Input, Button } from 'semantic-ui-react';

import * as authActions from '../../../actions/authActions';
import * as gameActions from '../../../actions/gameActions';

const BASE_URL = 'http://localhost:5000/api/';

class Login extends React.Component {
    constructor() {
        super();
        this.state = {
            user: {
                username: ''
            }
        };
        this.onLogin = this.onLogin.bind(this);
        this.onLogout = this.onLogout.bind(this);
        this.onTextChange = this.onTextChange.bind(this);
    }

    onLogin() {
        this.props.authActions.loginUser(this.state.user);
    }

    onLogout() {
        let config = {
            method: 'delete',
            url: BASE_URL + 'gameMoves',
            headers: {
                Authorization: 'bearer ' + this.props.auth.id_token
            }
        };
        this.props.gameActions.deleteGameStatus(config);
        this.props.authActions.logoutUser();
    }

    onTextChange(event) {
        event.preventDefault();
        return this.setState({
            user: {
                username: event.target.value
            }
        });
    }

    render() {
        let isAuthenticated = this.props.auth.isAuthenticated;
        return (
            <div className="login-area">
                {
                    isAuthenticated ?
                    <Button secondary onClick={this.onLogout}>Logout</Button> :
                    <div>
                        <div>
                            <Input focus
                                placeholder="Enter a user Id"
                                onChange={this.onTextChange}
                                value={this.state.user.username} />
                        </div>
                        <Button primary onClick={this.onLogin}>Login</Button>
                    </div>
                }
            </div>
        );
    }
}

Login.propTypes = {
    authActions: PropTypes.object.isRequired,
    gameActions: PropTypes.object.isRequired,
    auth: PropTypes.object.isRequired
};

function mapStateToProps(state, ownProps) {
    return {
        auth: state.auth
    };
}

function mapDispatchToProps(dispatch) {
    return {
        authActions: bindActionCreators(authActions, dispatch),
        gameActions: bindActionCreators(gameActions, dispatch)
    };
}

export default connect(mapStateToProps, mapDispatchToProps)(Login);
