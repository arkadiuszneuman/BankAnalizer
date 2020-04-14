import React, { Component } from 'react'
import {apiConnector} from "../../helpers/BankAnalizer"

interface IProps {
    connection: any,
    isCurrentUserHost: boolean
}

interface IState {
    connection: any
}

class Buttons extends Component<IProps, IState> {
    state = {
        connection: this.props.connection
    }

    approve = async () => {
        const connection = this.state.connection
        connection.isRequestApproved = true
        this.setState({ connection: connection })
        
        await apiConnector.post('usersconnection/accept', { hostUserIdToAcceptConnection: connection.requestingUserId })
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