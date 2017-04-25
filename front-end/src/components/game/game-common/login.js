import React from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Input, Button } from 'semantic-ui-react';

import * as authActions from '../../../actions/authActions';

class Login extends React.Component {
    constructor() {
        super();
        this.state = {
            user: {
                username: '',
                password: ''
            }
        };
        this.onLogin = this.onLogin.bind(this);
        this.clearStorage = this.clearStorage.bind(this);
        this.onTextChange = this.onTextChange.bind(this);
    }

    onLogin() {
        this.props.actions.loginUser(this.state.user);
    }

    clearStorage() {
        localStorage.removeItem('id_token');
    }

    onTextChange(event) {
        event.preventDefault();
        return this.setState({
            user: {
                username: event.target.value,
                password: 'P@ssw0rd!'
            }
        });
    }

    render() {
        return (
            <div className="login-area">
                <div>
                    <Input focus
                        placeholder="Enter a user Id"
                        onChange={this.onTextChange}
                        value={this.state.user.username} />
                </div>
                <div>
                    <Button primary onClick={this.onLogin}>Login</Button>
                    <Button secondary onClick={this.clearStorage}>Clear Storage</Button>
                </div>
            </div>
        );
    }
}

Login.propTypes = {
    actions: PropTypes.object.isRequired
};

function mapStateToProps(state, ownProps) {
    return {
        auth: state.auth
    };
}

function mapDispatchToProps(dispatch) {
    return {
        actions: bindActionCreators(authActions, dispatch)
    };
}

export default connect(mapStateToProps, mapDispatchToProps)(Login);
