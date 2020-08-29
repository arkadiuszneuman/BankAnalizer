import React, { Component } from 'react';
import {apiConnector} from '../../helpers/BankAnalizer'
import ConnectionCard from './ConnectionCard'
import Input, { IChange } from '../Controls/InputControl'

interface IState {
    connections: any[]
    value: string
}

export default class UsersConnections extends Component<{}, IState> {

    state = {
        connections: [],
        value: ''
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

    onKeyDown = (target: HTMLInputElement, event: React.KeyboardEvent) => {
        const keyCode = event.keyCode || event.which;
        if (keyCode === 13) {
            event.preventDefault();
            if (target.value) {
                this.sendInvitation(target.value);
                this.setState({ value: '' })
            }
            return false;
        }
    }

    onChange = (change: IChange) => {
        this.setState({ value: change.value })
    }
   
    render() {
        const connections = this.state.connections
        const value = this.state.value

        return (
            <div>
                
                <form className="ui form">
                    <Input name="userToConnect" text="User to connect" value={value} onKeyDown={this.onKeyDown} onChange={this.onChange} />
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