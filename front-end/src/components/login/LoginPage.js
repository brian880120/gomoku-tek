import React from 'react';
import PropTypes from 'prop-types';
import TextInput from '../common/input/TextInput';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import * as authActions from '../../actions/authActions';

class LoginPage extends React.Component {
    constructor(props, context) {
        super(props, context);
        this.state = {
            userId: ''
        };
        this.onTextChange = this.onTextChange.bind(this);
        this.checkinGame = this.checkinGame.bind(this);
    }

    onTextChange(event) {
        event.preventDefault();
        return this.setState({
            userId: event.target.value
        });
    }

    checkinGame() {
        this.props.history.pushState(null, '/game');
    }

    render() {
        return (
            <div>
                <form>
                    <h1>Game Checkin</h1>
                    <TextInput name="userId"
                        label="User Id"
                        value={this.state.userId}
                        placeholder="enter a user id"
                        onChange={this.onTextChange} />
                </form>
                <button className="btn btn-primary"
                    onClick={this.checkinGame}>Check In
                </button>
            </div>
        );
    }
}

LoginPage.propTypes = {
    isAuthenticated: PropTypes.bool.isRequired,
    actions: PropTypes.object.isRequired
};

function mapStateToProps(state, ownProps) {
    return {
        isAuthenticated: state.auth.isAuthenticated
    };
}

function mapDispatchToProps(dispatch) {
    return {
        actions: bindActionCreators(authActions, dispatch)
    };
}

export default connect(mapStateToProps, mapDispatchToProps)(LoginPage);
