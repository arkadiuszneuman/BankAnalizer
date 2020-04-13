import React, { Component } from 'react';
import PieChart from './Base/PieChart'
import {apiConnector} from '../../../helpers/BankAnalizer'

interface IProps {
    dateFrom?: Date,
    dateTo?: Date,
    onSegmentClick?: Function
}

interface IState {
    data: any[]
}

export default class GroupsChart extends Component<IProps, IState> {
    state = {
        data: []
    }
    
    componentDidUpdate = async (prevProps: IProps) => {
        if (prevProps.dateFrom !== this.props.dateFrom ||
            prevProps.dateTo !== this.props.dateTo) {
                await this.reloadGroups(this.props.dateFrom, this.props.dateTo)
            }
    }

    reloadGroups = async (dateFrom?: Date, dateTo?: Date) => {
        let body: any = { dateFrom: dateFrom, dateTo: dateTo }
        if (!dateFrom || !dateTo)
            body = {}
            
        const groups = await apiConnector.get("charts/groups", body)
        const mappedGroups = groups.map((g: any) => ({
                id: g.groupName,
                label: g.groupName,
                value: g.amount
            }));

        this.setState({data: mappedGroups})
    }

    render() {
        const height = '300px'
        return (
            <div style={{height: height}}>
                <PieChart data={this.state.data} onClick={this.props.onSegmentClick} />
            </div>
        );
    }
}