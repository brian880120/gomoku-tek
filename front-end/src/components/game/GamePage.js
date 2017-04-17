import React from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import * as gameActions from '../../actions/gameActions';
import GameColumn from './GameColumn';

class GamePage extends React.Component {
    constructor(props, context) {
        super(props, context);
    }

    render() {
        const layoutData = this.props.layoutData;
        return (
            <div className="game">
                {layoutData.map(columnData =>
                    <GameColumn key={columnData.id}
                            columnData={columnData.data}/>
                )}
            </div>
        );
    }
}

GamePage.propTypes = {
    layoutData: PropTypes.array.isRequired,
    actions: PropTypes.object.isRequired
};

function mapStateToProps(state, ownProps) {
    return {
        layoutData: state.game
    };
}

function mapDispatchToProps(dispatch) {
    return {
        actions: bindActionCreators(gameActions, dispatch)
    };
}

export default connect(mapStateToProps, mapDispatchToProps)(GamePage);
