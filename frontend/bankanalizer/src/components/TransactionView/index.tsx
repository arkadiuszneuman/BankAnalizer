import React, { Component } from 'react';
import TransactionList from './TransactionList'
import apiConnector from '../../helpers/api/CqrsApiConnector'
import DateTimeRange from '../Controls/DateTimeRangeControl'
import HubConnector from '../../helpers/api/HubConnector'
import Importer from '../Importer'
import Exporter from '../Exporter'
import UsersSelector, { IUser } from '../Controls/UsersSelectorControl'
import userManager from '../../helpers/api/UserManager'

interface IState {
    isInit: boolean,
    transactions: any[],
    loadedTransactions: any[],
    dateFrom?: Date,
    dateTo?: Date,
    onlyWithoutGroup: boolean,
    users: IUser[],
    selectedUsers: IUser[]
}

export default class TransactionView extends Component<{}, IState> {

    state = {
        isInit: true,
        transactions: [],
        loadedTransactions: [],
        onlyWithoutGroup: false,
        users: [],
        selectedUsers: []
    } as IState

    componentDidMount = async () => {
        (await HubConnector).hubConnection.on('group-to-transaction-added', event => {
            if (this.state.onlyWithoutGroup) {
                const transactions = this.state.transactions.filter((t: any) => t.transactionId !== event.bankTransactionId)
                this.setState({transactions: transactions})
            }
        });

        await this.loadUsers()
        this.setState({isInit: false})
        await this.loadTransactions()
    }

    loadUsers = async () => {
        const connections = await apiConnector.get('usersconnection', { onlyApproved: true })
        const users = connections.map((c: any) => {
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
            const transactions = await apiConnector.get("transaction", { 
                onlyWithoutGroup: this.state.onlyWithoutGroup,
                dateFrom: this.state.dateFrom,
                dateTo: this.state.dateTo,
                users: this.state.selectedUsers.map(u => u.id)
            })
            transactions.forEach((element: any) => {
                element.extensions = JSON.parse(element.extensions) ?? ""
            });
            this.setState({transactions: transactions, loadedTransactions: transactions})
        }
    }

    toggleOnlyWithoutGroup = (event: React.ChangeEvent<HTMLInputElement>) => {
        this.setState({onlyWithoutGroup: event.target.checked}, async () => await this.loadTransactions())
    }

    dateTimeChanged = (dateFrom: Date, dateTo: Date) => {
        this.setState({dateFrom: dateFrom, dateTo: dateTo}, async () => await this.loadTransactions())
    }

    usersChanged = (selectedUsers: any) => {
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