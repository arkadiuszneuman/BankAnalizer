import React, { Component } from 'react';
import TransactionList from './TransactionList'
import ApiConnector from '../../helpers/api/ApiConnector'

export default class TransactionView extends Component {
    connector = new ApiConnector()

    state = {
        transactions: [],
    }

    componentDidMount = async () => {
        await this.loadTransactions(false)
    }

    loadTransactions = async (onlyWithoutGroup) => {
        const transactions = await this.connector.get("transaction", { onlyWithoutGroup: onlyWithoutGroup })
        transactions.forEach(element => {
            element.extensions = JSON.parse(element.extensions) ?? ""
            element.currentGroup = ""
        });
        this.setState({transactions: transactions})
    }

    toggleOnlyWithoutGroup = async (event) => {
        await this.loadTransactions(event.target.checked);
    }

    render() {
        return (
            <div>
                <div className="ui checkbox">
                    <input type="checkbox" onChange={this.toggleOnlyWithoutGroup} />
                    <label>Only without group</label>
                </div>
                <TransactionList transactions={this.state.transactions} />
            </div>
        )
    }
}