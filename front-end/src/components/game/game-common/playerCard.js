import React from 'react';
import PropTypes from 'prop-types';
import { Card, Image } from 'semantic-ui-react';
import Logout from './logout';

const PlayerCard = ({playerName, playerSide, isCurrentPlayer, isActive, isWinner}) => {
    let cardStyle = 'name-card';
    if (isActive) {
        cardStyle = 'name-card name-card-active';
    }
    if (isWinner) {
        cardStyle = 'name-card name-card-win';
    }
    return (
        <div className={cardStyle}>
            <div className="card-header">
                <div className="name">{playerName}</div>
                <div className="card-img">
                </div>
            </div>
            <div className="card-content">
                {
                    isWinner ?
                    <div className="star-area">
                        <div className="star"></div>
                        <div className="star"></div>
                        <div className="star"></div>
                        <div className="star"></div>
                        <div className="star"></div>
                    </div> :
                    <div className="player-side">{playerSide} side player</div>
                }
            </div>
            {
                isCurrentPlayer ?
                <div className="card-info-extra">
                    <Logout />
                </div> :
                ''
            }
        </div>
    );
};

PlayerCard.propTypes = {
    playerName: PropTypes.string,
    playerSide: PropTypes.string,
    isActive: PropTypes.bool.isRequired,
    isWinner: PropTypes.bool.isRequired,
    isCurrentPlayer: PropTypes.bool
};

export default PlayerCard;
