import React, { Component } from 'react';
import TransactionList from './TransactionList'
import ApiConnector from '../../helpers/api/ApiConnector'
import DateTimeRange from '../Controls/DateTimeRange'

export default class TransactionView extends Component {
    connector = new ApiConnector()

    state = {
        transactions: [],
        dateFrom: null,
        dateTo: null,
        onlyWithoutGroup: false
    }
    
    loadTransactions = async () => {
        const transactions = await this.connector.get("transaction", { 
            onlyWithoutGroup: this.state.onlyWithoutGroup,
            dateFrom: this.state.dateFrom,
            dateTo: this.state.dateTo
        })
        transactions.forEach(element => {
            element.extensions = JSON.parse(element.extensions) ?? ""
        });
        this.setState({transactions: transactions})
    }

    toggleOnlyWithoutGroup = async (event) => {
        this.setState({onlyWithoutGroup: event.target.checked}, async () => await this.loadTransactions())
    }

    dateTimeChanged = async (dateFrom, dateTo) => {
        this.setState({dateFrom: dateFrom, dateTo: dateTo}, async () => await this.loadTransactions())
    }

    render() {
        return (
            <div>
                <DateTimeRange onChange={this.dateTimeChanged} />
                <div className="ui checkbox">
                    <input type="checkbox" onChange={this.toggleOnlyWithoutGroup} />
                    <label>Only without group</label>
                </div>
                <TransactionList transactions={this.state.transactions} />
            </div>
        )
    }
}