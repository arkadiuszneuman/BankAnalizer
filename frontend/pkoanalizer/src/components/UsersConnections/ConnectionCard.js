import React, { Component } from 'react';
import userManager from '../../helpers/api/UserManager'

export default class ConnectionCard extends Component {
    state = {
        user: null
    }

    componentDidMount = () => {
        const user = userManager.getUserFromStorage()

        const connection = this.props.connection
        const isCurrentUserHost = connection.requestingUserId === user.id
        const requestedUser = {
            firstName: isCurrentUserHost ? connection.requestedUserFirstName : connection.requestingUserFirstName,
            lastName: isCurrentUserHost ? connection.requestedUserLastName : connection.requestingUserLastName,
        }

        this.setState({ requestedUser: requestedUser, isCurrentUserHost: isCurrentUserHost })
    }

    render() {
        const connection = this.props.connection
        const requestedUser = this.state.requestedUser
        const isCurrentUserHost = this.state.isCurrentUserHost
        const link = `https://eu.ui-avatars.com/api/?name=${requestedUser?.firstName}+${requestedUser?.lastName}&rounded=true`

        return (
            <div className="card">
                <div className="content">
                    <img className="right floated mini ui image" 
                        src={link}
                        />
                    <div className="header">
                        {requestedUser?.firstName} {requestedUser?.lastName}
                    </div>
                    <div className="description">
                    {isCurrentUserHost &&
                        <div className="description">
                            You requested permission to view {connection?.requestedUserFirstName}'s transactions
                        </div>
                    }
                    {!isCurrentUserHost &&
                        <div className="description">
                            {connection?.requestingUserFirstName} requested permission to view your transactions
                        </div>
                    }
                    </div>
                </div>
                <div className="extra content">
                    <div className="ui two buttons">
                        <div className="ui basic green button">Approve</div>
                        <div className="ui basic red button">Decline</div>
                    </div>
                </div>
            </div>
        );
    }
}