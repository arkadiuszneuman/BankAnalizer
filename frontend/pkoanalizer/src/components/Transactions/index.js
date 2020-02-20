import React, { Component } from 'react';
import ApiConnector from '../../helpers/api/ApiConnector'

function TransactionRow(props) {
    var transaction = props.transaction
    return (
        <div className="item ui red segment">
            <i className="large money bill alternate middle aligned icon"></i>
            <div className="content">
                <div className="header">{transaction.name}</div>
                <div className="description">{transaction.amount} zł</div>
                <div className="description">{transaction.type}</div>
                {transaction.groups.length > 0 &&
                    <div className="description">Groups: {transaction.groups.filter(g => !g.manualGroup).map(g => g.groupName).join(", ")}</div>
                }
                {transaction.groups.filter(g => g.manualGroup).map((group, index) =>
                    <div className="ui label" key={index}>
                        {group.groupName}
                        <i className="delete icon" onClick={e => this.removeGroup(group, transaction)}></i>
                    </div>
                )}
                <div className="ui mini action input">
                    <input type="text" placeholder="Group" 
                            value={transaction.currentGroup} 
                            onChange={e => this.updateCurrentGroup(e, transaction)}
                            onKeyDown={e => this.addGroupByEnter(e, transaction)} />
                    <button className="ui mini teal icon button" onClick={e => this.addGroup(transaction)}>
                        <i className="add icon"></i>
                    </button>
                </div>
                <div className="list">
                    {Object.keys(transaction.extensions).map((key, index) => 
                        <div className="item" key={index}>{key}: <b>{transaction.extensions[key]}</b></div>
                    )}
                </div>
            </div>
        </div>
    );
}

function TransactionList(props) {
    var transactions = props.transactions
    return (
        <div className="ui relaxed celled list">
            {transactions.map(transaction => 
                <TransactionRow key={transaction.transactionId} transaction={transaction} />
            )}
        </div>
    );
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

    addGroup(transaction) {
        const currentTransaction = this.state.transactions.filter(t => t.transactionId === transaction.transactionId)[0]

        this.connector.post(`transaction/group`, { bankTransactionId: transaction.transactionId, groupName: currentTransaction.currentGroup });

        currentTransaction.groups.push({ groupName: currentTransaction.currentGroup, manualGroup: true })
        currentTransaction.currentGroup = ""
        this.setState({transactions: this.state.transactions})
    }

    addGroupByEnter(event, transaction) {
        if (event.key === 'Enter') {
            this.addGroup(transaction)
        }
    }

    updateCurrentGroup(event, transaction) {
        const currentTransaction = this.state.transactions.filter(t => t.transactionId === transaction.transactionId)[0]
        currentTransaction.currentGroup = event.target.value
        this.setState({transactions: this.state.transactions})
    }

    removeGroup(group, transaction) {
        const currentTransaction = this.state.transactions.filter(t => t.transactionId === transaction.transactionId)[0]
        const groupToRemove = currentTransaction.groups.filter(g => g.groupName === group.groupName)[0]

        this.connector.delete(`transaction/group`, { bankTransactionId: transaction.transactionId, groupName: groupToRemove.groupName });

        currentTransaction.groups.splice(currentTransaction.groups.indexOf(groupToRemove), 1)
        this.setState({transactions: this.state.transactions})
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