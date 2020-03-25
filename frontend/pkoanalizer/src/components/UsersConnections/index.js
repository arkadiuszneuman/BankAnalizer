import React, { Component } from 'react';
import ApiConnector from '../../helpers/api/ApiConnector'
import ConnectionCard from './ConnectionCard'

export default class UsersConnections extends Component {
    connector = new ApiConnector()

    state = {
        connections: []
    }

    componentDidMount = async () => {
        await this.loadConnections()
    }

    loadConnections = async () => {
        const connections = await this.connector.get("usersconnection")
        this.setState({ connections: connections })
    }
   
    render() {
        const connections = this.state.connections

        return (
            <div>
                <div className="ui cards">
                    {connections.map(connection => 
                        <ConnectionCard key={connection.requestedUserId} connection={connection} />
                    )}
                </div>
            </div>
        );
    }
}