import React from 'react';
import { Message } from 'semantic-ui-react';

class LoginRequire extends React.Component {
    render() {
        return (
            <div className="require-login">
                <Message>
                    <Message.Header>
                        You are not currently logged in. Please login to activate the game
                    </Message.Header>
                </Message>
            </div>
        );
    }
}

export default LoginRequire;
