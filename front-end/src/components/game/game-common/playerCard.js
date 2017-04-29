import React from 'react';
import PropTypes from 'prop-types';
import { Card, Image } from 'semantic-ui-react';

const PlayerCard = ({playerName}) => (
    <Card>
        <Card.Content>
            <Image floated="right" size="mini" src="" />
            <Card.Header>
                {playerName}
            </Card.Header>
            <Card.Meta>
                Friends of Elliot
            </Card.Meta>
        </Card.Content>
    </Card>
);

PlayerCard.propTypes = {
    playerName: PropTypes.string
};

export default PlayerCard;
