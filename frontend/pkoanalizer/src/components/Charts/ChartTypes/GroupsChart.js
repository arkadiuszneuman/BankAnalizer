import React, { Component } from 'react';
import PieChart from './Base/PieChart'
import ApiConnector from '../../../helpers/api/ApiConnector'

export default class GroupsChart extends Component {
    connector = new ApiConnector()

    state = {
        data: []
    }

    componentDidMount = async () => {
        const groups = await this.connector.get("charts/groups")
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