import React from 'react';
import PropTypes from 'prop-types';
import { Card, Image } from 'semantic-ui-react';
import Logout from './logout';
import logo from '../../../images/steve.jpg';

const PlayerCard = ({playerName, playerSide, isCurrentPlayer}) => {
    return (
        <Card>
            <Card.Content>
                <Image floated="right" size="mini" src={logo} />
                <Card.Header>
                    {playerName}
                </Card.Header>
                <Card.Meta>
                    {playerSide}
                </Card.Meta>
            </Card.Content>
            {
                isCurrentPlayer ?
                <Card.Content extra>
                    <Logout />
                </Card.Content> :
                ''
            }
        </Card>
    );
};

PlayerCard.propTypes = {
    playerName: PropTypes.string,
    playerSide: PropTypes.string,
    isCurrentPlayer: PropTypes.bool
};

export default PlayerCard;
