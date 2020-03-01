import React, { Component } from 'react';
import GroupsChart from './ChartTypes/GroupsChart'
import ApiConnector from '../../helpers/api/ApiConnector'
import TransactionList from '../TransactionView/TransactionList'
import DateTimeRange from '../Controls/DateTimeRange'

export default class ChartsView extends Component {
    state = {
        fitTransactions: [],
        dateFrom: null,
        dateTo: null
    }

    connector = new ApiConnector()

    segmentClicked = async (segment) => {
        const transactions = await this.connector.get('charts/groups/transactions', { groupName: segment.label })
        transactions.forEach(element => {
            element.extensions = JSON.parse(element.extensions) ?? ""
        });
        this.setState({fitTransactions: transactions})
    }

    dateTimeChanged = (dateFrom, dateTo) => {
        this.setState({dateFrom: dateFrom, dateTo: dateTo})
    }
    
    render() {
        return (
            <div>
                <DateTimeRange onChange={this.dateTimeChanged} />
                <GroupsChart onSegmentClick={this.segmentClicked} dateFrom={this.state.dateFrom} dateTo={this.state.dateTo} />
                <TransactionList transactions={this.state.fitTransactions} showGroups={false} />
            </div>
        );
    }
}