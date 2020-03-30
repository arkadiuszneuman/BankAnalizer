import React, { Component } from 'react';
import TransactionList from './TransactionList'
import ApiConnector from '../../helpers/api/ApiConnector'
import DateTimeRange from '../Controls/DateTimeRange'
import HubConnector from '../../helpers/api/HubConnector'
import Importer from '../Importer'
import Exporter from '../Exporter'
import UsersSelector from '../Controls/UsersSelector'
import userManager from '../../helpers/api/UserManager'

export default class TransactionView extends Component {
    connector = new ApiConnector()

    state = {
        isInit: true,
        transactions: [],
        loadedTransactions: [],
        dateFrom: null,
        dateTo: null,
        onlyWithoutGroup: false,
        users: [],
        selectedUsers: null
    }

    componentDidMount = async () => {
        (await HubConnector).HubConnection.on('group-to-transaction-added', event => {
            if (this.state.onlyWithoutGroup) {
                const transactions = this.state.transactions.filter(t => t.transactionId !== event.bankTransactionId)
                this.setState({transactions: transactions})
            }
        });

        await this.loadUsers()
        this.setState({isInit: false})
        await this.loadTransactions()
    }

    loadUsers = async () => {
        const connections = await this.connector.get('usersconnection', { onlyApproved: true })
        const users = connections.map(function(c) {
            return {
                id: c.requestedUserId,
                firstName: c.requestedUserFirstName,
                lastName: c.requestedUserLastName,
            }
        });

        users.unshift(userManager.getUserFromStorage())

        this.setState({ users: users, selectedUsers: users });
    }
    
    loadTransactions = async () => {
        if (!this.state.isInit) {
            const transactions = await this.connector.get("transaction", { 
                onlyWithoutGroup: this.state.onlyWithoutGroup,
                dateFrom: this.state.dateFrom,
                dateTo: this.state.dateTo,
                users: this.state.selectedUsers.map(u => u.id)
            })
            transactions.forEach(element => {
                element.extensions = JSON.parse(element.extensions) ?? ""
            });
            this.setState({transactions: transactions, loadedTransactions: transactions})
        }
    }

    toggleOnlyWithoutGroup = event => {
        this.setState({onlyWithoutGroup: event.target.checked}, async () => await this.loadTransactions())
    }

    dateTimeChanged = (dateFrom, dateTo) => {
        this.setState({dateFrom: dateFrom, dateTo: dateTo}, async () => await this.loadTransactions())
    }

    usersChanged = selectedUsers => {
        this.setState({selectedUsers: selectedUsers}, async () => await this.loadTransactions())
    }

    render() {
        const users = this.state.users

        return (
            <div>
                <Importer />
                <Exporter />
                <DateTimeRange onChange={this.dateTimeChanged} />
                <UsersSelector users={users} initSelectedUsers={users} onChange={this.usersChanged} />
                <div className="ui checkbox">
                    <input type="checkbox" onChange={this.toggleOnlyWithoutGroup} />
                    <label>Only without group</label>
                </div>
                <TransactionList transactions={this.state.transactions} />
            </div>
        )
    }
}