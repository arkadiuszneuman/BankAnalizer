import React, { Component } from 'react';
import ApiConnector from '../../../helpers/api/ApiConnector'
import TransactionGroupInput from './TransactionGroupInput'

export default class TransactionGroups extends Component {
    connector = new ApiConnector()

    state = {
        transaction: this.props.transaction
    }

    addGroup = (group) => {
        const currentTransaction = this.state.transaction

        this.connector.post(`transaction/group`, { bankTransactionId: currentTransaction.transactionId, groupName: group });

        currentTransaction.groups.push({ groupName: group, manualGroup: true })
        this.setState({transaction: currentTransaction})
    }

    removeGroup(group) {
        const currentTransaction = this.state.transaction
        const groupToRemove = currentTransaction.groups.filter(g => g.groupName === group.groupName)[0]

        this.connector.delete(`transaction/group`, { bankTransactionId: currentTransaction.transactionId, groupName: groupToRemove.groupName });

        currentTransaction.groups.splice(currentTransaction.groups.indexOf(groupToRemove), 1)
        this.setState({transaction: currentTransaction})
    }

    render() {
        var transaction = this.state.transaction

        return (
            <div>
                {transaction.groups.length > 0 &&
                    <div className="description">Groups: {transaction.groups.filter(g => !g.manualGroup).map(g => g.groupName).join(", ")}</div>
                }
                {transaction.groups.filter(g => g.manualGroup).map((group, index) =>
                    <div className="ui label" key={index}>
                        {group.groupName}
                        <i className="delete icon" onClick={e => this.removeGroup(group)}></i>
                    </div>
                )}
                <TransactionGroupInput onAddGroup={this.addGroup} />
            </div>
        )
    }
}