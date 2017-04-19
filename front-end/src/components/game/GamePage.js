import React from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import * as gameActions from '../../actions/gameActions';
import GameColumn from './GameColumn';

class GamePage extends React.Component {
    constructor(props, context) {
        super(props, context);
        this.handleUnitClick = this.handleUnitClick.bind(this);
    }

    handleUnitClick(columnId, unitId, event) {
        event.preventDefault();
        this.props.actions.updateGameStatus(columnId, unitId);
    }

    render() {
        const layoutData = this.props.layoutData;
        let statusData = this.props.statusData;
        return (
            <div className="game">
                {layoutData.map(columnData =>
                    <GameColumn key={columnData.id}
                            statusData={statusData}
                            columnData={columnData}
                            handleUnitClick={this.handleUnitClick} />
                )}
            </div>
        );
    }
}

GamePage.propTypes = {
    layoutData: PropTypes.array.isRequired,
    statusData: PropTypes.array.isRequired,
    actions: PropTypes.object.isRequired
};

function mapStateToProps(state, ownProps) {
    return {
        layoutData: state.gameLayout,
        statusData: state.gameStatus
    };
}

function mapDispatchToProps(dispatch) {
    return {
        actions: bindActionCreators(gameActions, dispatch)
    };
}

export default connect(mapStateToProps, mapDispatchToProps)(GamePage);
