import React, { Component } from 'react';
import {apiConnector} from '../../helpers/BankAnalizer'
import ConnectionCard from './ConnectionCard'

interface IState {
    connections: any[]
}

export default class UsersConnections extends Component<{}, IState> {

    state = {
        connections: []
    }

    componentDidMount = async () => {
        await this.loadConnections()
    }

    loadConnections = async () => {
        const connections = await apiConnector.get('usersconnection', { showAlsoAsRequestedUser: true })
        this.setState({ connections: connections })
    }
   
    render() {
        const connections = this.state.connections

        return (
            <div>
                <div className="ui cards">
                    {connections.map((connection: any) => 
                        <ConnectionCard key={connection.requestedUserId} connection={connection} />
                    )}
                </div>
            </div>
        );
    }
}