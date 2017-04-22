import React from 'react';
import TextInput from '../common/input/TextInput';

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

export default LoginPage;
