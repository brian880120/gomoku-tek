import React from 'react';
import { Image } from 'semantic-ui-react';
import imageSrc from '../../../images/gomoku-logo3.jpg';

class GameHeader extends React.Component {
    render() {
        return (
            <div className="header-area">
                <Image src={imageSrc} shape="rounded" wrapped />
                <h1>Gomoku</h1>
            </div>
        );
    }
}

export default GameHeader;
