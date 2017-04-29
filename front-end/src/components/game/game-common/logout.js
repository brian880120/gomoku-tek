import React from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Button } from 'semantic-ui-react';

import * as gameActions from '../../../actions/gameActions';
import * as authActions from '../../../actions/authActions';

import { BASE_URL } from '../../../api/apiConfig';

class Logout extends React.Component {
    constructor() {
        super();
        this.onLogout = this.onLogout.bind(this);
    }


    onLogout() {
        let config = {
            method: 'delete',
            url: BASE_URL + 'gameMoves',
            headers: {
                Authorization: 'bearer ' + this.props.auth.id_token
            }
        };
        this.props.gameActions.deleteGameStatus(config);
        this.props.authActions.logoutUser();
    }

    render() {
        return (
            <div className="container-fluid">
                <div className="row">
                    <div className="col-md-4">
                        <Button basic color="red" onClick={this.onLogout}>Logout</Button>
                    </div>
                </div>
            </div>
        );
    }
}

Logout.propTypes = {
    authActions: PropTypes.object.isRequired,
    gameActions: PropTypes.object.isRequired,
    auth: PropTypes.object.isRequired
};

function mapStateToProps(state, ownProps) {
    return {
        auth: state.auth
    };
}

function mapDispatchToProps(dispatch) {
    return {
        authActions: bindActionCreators(authActions, dispatch),
        gameActions: bindActionCreators(gameActions, dispatch)
    };
}

export default connect(mapStateToProps, mapDispatchToProps)(Logout);
