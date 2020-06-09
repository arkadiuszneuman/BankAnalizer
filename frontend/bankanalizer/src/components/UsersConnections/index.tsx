import React, { Component } from 'react';
import {apiConnector} from '../../helpers/BankAnalizer'
import ConnectionCard from './ConnectionCard'
import Input from '../Controls/InputControl'

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

    onKeyDown = (event: React.KeyboardEvent<HTMLInputElement>) => {
        if (event.key === 'Enter') {
            // if (event.target.value)
        }
    }
   
    render() {
        const connections = this.state.connections

        return (
            <div>
                <div className="ui input">
                    <input type="text" placeholder="User to connect" onKeyDown={this.onKeyDown} />
                </div>
                <div className="ui cards">
                    {connections.map((connection: any) => 
                        <ConnectionCard key={connection.requestedUserId} connection={connection} />
                    )}
                </div>
            </div>
        );
    }
}