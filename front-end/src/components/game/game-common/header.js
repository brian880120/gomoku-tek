import React from 'react';
import { Image } from 'semantic-ui-react';

class GameHeader extends React.Component {
    render() {
        return (
            <div className="header-area">
                <div className="header-icon"></div>
                <h1>Gomoku</h1>
            </div>
        );
    }
}

export default GameHeader;
