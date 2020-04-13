import React, { Component } from 'react';
import GroupsChart from './ChartTypes/GroupsChart'
import {apiConnector} from '../../helpers/BankAnalizer'
import TransactionList from '../TransactionView/TransactionList'
import DateTimeRange from '../Controls/DateTimeRange'

interface IState {
    fitTransactions: string[],
    dateFrom?: Date,
    dateTo?: Date
}

export default class ChartsView extends Component<{}, IState> {
    state = {
        fitTransactions: []
    } as IState

    segmentClicked = async (segment: any) => {
        const transactions = await apiConnector.get('charts/groups/transactions', { 
            groupName: segment.label, 
            dateFrom: this.state.dateFrom, 
            dateTo: this.state.dateTo
        })
        transactions.forEach((element: any) => {
            element.extensions = JSON.parse(element.extensions) ?? ""
        });
        this.setState({fitTransactions: transactions})
    }

    dateTimeChanged = (dateFrom: Date, dateTo: Date) => {
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