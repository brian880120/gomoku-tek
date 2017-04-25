import React from 'react';
import PropTypes from 'prop-types';
import GameHeader from './game/game-common/header';

class App extends React.Component {
    render() {
        return (
            <div>
                <GameHeader />
                <div className="game-body">
                    {this.props.children}
                </div>
            </div>
        );
    }
}

App.propTypes = {
    children: PropTypes.object.isRequired
};

export default App;
