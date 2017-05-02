import React from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Input, Button, Message } from 'semantic-ui-react';

import * as authActions from '../../../actions/authActions';
import * as gameActions from '../../../actions/gameActions';

class Login extends React.Component {
    constructor() {
        super();
        this.state = {
            user: {
                username: ''
            }
        };
        this.onLogin = this.onLogin.bind(this);
        this.onTextChange = this.onTextChange.bind(this);
    }

    onLogin() {
        this.props.authActions.loginUser(this.state.user);
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
        return (
            <div>
                {
                    this.props.auth.isAuthenticated ?
                    '' :
                    <div className="login-area">
                        <div>
                            <Message>
                                <Message.Header>
                                    Please login to activate the game
                                </Message.Header>
                            </Message>
                        </div>
                        <div className="login-input">
                            <Input focus
                                placeholder="Enter a user Id"
                                onChange={this.onTextChange}
                                value={this.state.user.username} />
                        </div>
                        <div className="login-button">
                            <Button primary onClick={this.onLogin}>Login</Button>
                        </div>
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
