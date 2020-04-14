import React, { Component } from 'react';
import {apiConnector} from '../../../helpers/BankAnalizer'
import TransactionGroupInput from './TransactionGroupInput'

interface IProps {
    transaction: any
}

interface IState {
    transaction: any
}

export default class TransactionGroups extends Component<IProps, IState> {
    state = {
        transaction: this.props.transaction
    }

    addGroup = (group: any) => {
        const currentTransaction = this.state.transaction

        apiConnector.post(`transaction/group`, { bankTransactionId: currentTransaction.transactionId, groupName: group });

        currentTransaction.groups.push({ groupName: group, manualGroup: true })
        this.setState({transaction: currentTransaction})
    }

    removeGroup(group: any) {
        const currentTransaction = this.state.transaction
        const groupToRemove = currentTransaction.groups.filter((g: any) => g.groupName === group.groupName)[0]

        apiConnector.delete(`transaction/group`, { bankTransactionId: currentTransaction.transactionId, groupName: groupToRemove.groupName });

        currentTransaction.groups.splice(currentTransaction.groups.indexOf(groupToRemove), 1)
        this.setState({transaction: currentTransaction})
    }

    render() {
        var transaction = this.state.transaction

        return (
            <div>
                {transaction.groups.length > 0 &&
                    <div className="description">Groups: {transaction.groups.filter((g: any) => !g.manualGroup).map((g: any) => g.groupName).join(", ")}</div>
                }
                {transaction.groups.filter((g: any) => g.manualGroup).map((group: any, index: number) =>
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