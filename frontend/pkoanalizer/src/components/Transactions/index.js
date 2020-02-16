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
        });
        this.setState({transactions: transactions})
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
                                    <div className="description">Groups: {transaction.groups.join(", ")}</div>
                                }
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