import React, { Component } from 'react';
import ApiConnector from '../../helpers/api/ApiConnector'

class TransactionGroupInput extends Component {
    state = {
        currentGroup: ''
    }

    addGroupByEnter = (event) => {
        if (event.key === 'Enter') {
            this.addGroup()
        }
    }

    addGroup = () => {
        const currentGroup = this.state.currentGroup
        this.setState({currentGroup: '' })
        if (this.props.onAddGroup)
            this.props.onAddGroup(currentGroup)
    }

    updateCurrentGroup = (event) => {
        this.setState({currentGroup: event.target.value })
    }

    render() {
        return (
            <div className="ui mini action input">
                <input type="text" placeholder="Group" 
                        value={this.state.currentGroup} 
                        onChange={this.updateCurrentGroup}
                        onKeyDown={this.addGroupByEnter} />
                <button className="ui mini teal icon button" onClick={this.addGroup}>
                    <i className="add icon"></i>
                </button>
            </div>
        )
    }
}

class TransactionGroups extends Component {
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

class TransactionRow extends Component {
    render() {
        var transaction = this.props.transaction
        var showGroups = this.props.showGroups ?? true

        return (
            <div className="item ui red segment">
                <i className="large money bill alternate middle aligned icon"></i>
                <div className="content">
                    <div className="header">{transaction.name}</div>
                    <div className="description">{transaction.amount} zł</div>
                    <div className="description">{transaction.type}</div>
                    {showGroups && <TransactionGroups transaction={transaction} /> }
                    <div className="list">
                        {Object.keys(transaction.extensions).map((key, index) => 
                            <div className="item" key={index}>{key}: <b>{transaction.extensions[key]}</b></div>
                        )}
                    </div>
                </div>
            </div>
        )
    }
}

function TransactionList(props) {
    var transactions = props.transactions
    var showGroups = props.showGroups ?? true
    return (
        <div className="ui relaxed celled list">
            {transactions.map(transaction => 
                <TransactionRow key={transaction.transactionId} transaction={transaction} showGroups={showGroups} />
            )}
        </div>
    )
}

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