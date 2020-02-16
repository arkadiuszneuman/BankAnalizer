import React, { Component } from 'react';
import ApiConnector from '../../helpers/api/ApiConnector'

class TransactionList extends Component {
    connector = new ApiConnector()

    state = {
        transactions: []
    }

    componentDidMount = async () => {
        const transactions = await this.connector.get("transaction")
        transactions.forEach(element => {
            element.extensions = JSON.parse(element.extensions) ?? ""
            element.currentGroup = ""
        });
        this.setState({transactions: transactions})
    }

    addGroup(transaction) {
        const currentTransaction = this.state.transactions.filter(t => t.transactionId === transaction.transactionId)[0]
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
        currentTransaction.groups.splice(currentTransaction.groups.indexOf(groupToRemove), 1)
        this.setState({transactions: this.state.transactions})
    }

    render() {
        return (
            <div>
                <div className="ui relaxed celled list">
                    {this.state.transactions.map(transaction => 
                        <div className="item ui red segment" key={transaction.transactionId}>
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
                    )}
                    </div>
            </div>
        )
    }
}

export default TransactionList