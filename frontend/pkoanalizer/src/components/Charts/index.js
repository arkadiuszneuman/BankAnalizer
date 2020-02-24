import React, { Component } from 'react';
import GroupsChart from './ChartTypes/GroupsChart'
import ApiConnector from '../../helpers/api/ApiConnector'
import TransactionList from '../TransactionView/TransactionList'

export default class ChartsView extends Component {
    state = {
        fitTransactions: []
    }

    connector = new ApiConnector()

    segmentClicked = async (segment) => {
        const transactions = await this.connector.get('charts/groups/transactions', { groupName: segment.label })
        transactions.forEach(element => {
            element.extensions = JSON.parse(element.extensions) ?? ""
        });
        this.setState({fitTransactions: transactions})
    }
    
    render() {
        return (
            <div>
                <GroupsChart onSegmentClick={this.segmentClicked} />
                <TransactionList transactions={this.state.fitTransactions} showGroups={false} />
            </div>
        );
    }
}