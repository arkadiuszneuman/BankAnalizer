import React, { Component } from 'react';
import PieChart from './Base/PieChart'
import ApiConnector from '../../../helpers/api/ApiConnector'

export default class GroupsChart extends Component {
    connector = new ApiConnector()

    state = {
        data: []
    }
    
    componentDidUpdate = async (prevProps) => {
        if (prevProps.dateFrom !== this.props.dateFrom ||
            prevProps.dateTo !== this.props.dateTo) {
                await this.reloadGroups(this.props.dateFrom, this.props.dateTo)
            }
    }

    reloadGroups = async (dateFrom, dateTo) => {
        let body = { dateFrom: dateFrom, dateTo: dateTo }
        if (!dateFrom || !dateTo)
            body = {}
            
        const groups = await this.connector.get("charts/groups", body)
        const mappedGroups = groups.map(g => ({
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