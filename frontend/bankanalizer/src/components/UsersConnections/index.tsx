import React, { Component } from 'react';
import {apiConnector} from '../../helpers/BankAnalizer'
import ConnectionCard from './ConnectionCard'
import EnterInput from './EnterInput';

interface IState {
    connections: any[]
}

export default class UsersConnections extends Component<{}, IState> {

    state = {
        connections: []
    } as IState

    componentDidMount = async () => {
        await this.loadConnections()
    }

    loadConnections = async () => {
        const connections = await apiConnector.get('usersconnection', { showAlsoAsRequestedUser: true })
        this.setState({ connections: connections })
    }

    sendInvitation = (userToSendInvitation: string) => {
        console.log(userToSendInvitation)
    }

    render() {
        const connections = this.state.connections

        return (
            <div>
                
                <form className="ui form">
                    <EnterInput name="userToConnect" text="User to connect" onCommitValue={this.sendInvitation} />
                </form>
                <div className="ui cards">
                    {connections.map((connection: any) => 
                        <ConnectionCard key={connection.requestedUserId} connection={connection} />
                    )}
                </div>
            </div>
        );
    }
}