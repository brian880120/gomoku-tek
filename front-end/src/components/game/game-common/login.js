import React from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Input, Button, Message } from 'semantic-ui-react';
import SelectInput from '../../common/input/selectInput';

import * as authActions from '../../../actions/authActions';
import * as gameActions from '../../../actions/gameActions';

const options = [{
    key: 'manual',
    text: 'Manual',
    value: 'manual'
}];

const defaultOption = {
    key: 'auto',
    text: 'Auto',
    value: 'auto'
};

class Login extends React.Component {
    constructor() {
        super();
        this.state = {
            user: {
                username: '',
                gamemode: ''
            }
        };
        this.onLogin = this.onLogin.bind(this);
        this.updateUserState = this.updateUserState.bind(this);
    }

    onLogin() {
        this.props.authActions.loginUser(this.state.user);
    }

    updateUserState(event) {
        event.preventDefault();
        let user = this.state.user;
        user[event.target.name] = event.target.value;
        return this.setState({
            user: user
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
                                name="username"
                                placeholder="Enter a user Id"
                                onChange={this.updateUserState}
                                value={this.state.user.username} />
                        </div>
                        <div className="login-button">
                            <Button primary onClick={this.onLogin}>Login</Button>
                        </div>
                        <div>
                            <SelectInput name="gamemode"
                                defaultOption={defaultOption}
                                options={options}
                                value={this.state.user.gamemode}
                                onChange={this.updateUserState} />
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
