import React, { Component } from 'react';
import ApiConnector from "../../helpers/api/ApiConnector";

class Buttons extends Component {
    connector = new ApiConnector()

    state = {
        connection: this.props.connection
    }

    approve = async () => {
        const connection = this.state.connection
        connection.isRequestApproved = true
        this.setState({ connection: connection })
        
        await this.connector.post('usersconnection/accept', { hostUserIdToAcceptConnection: connection.requestingUserId })
    }

    render() {
        const isCurrentUserHost = this.props.isCurrentUserHost
        const connection = this.state.connection

        return (
            <div className="ui two buttons">
                {!isCurrentUserHost && !connection.isRequestApproved &&
                    <div className="ui basic green button" onClick={this.approve}>Approve</div>
                }
                <div className="ui basic red button">Decline</div>
            </div>
        );
    }
}

export default Buttons;