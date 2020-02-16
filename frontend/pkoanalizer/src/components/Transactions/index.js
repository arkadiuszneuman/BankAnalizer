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
                <div className="ui relaxed divided list">
                    {this.state.transactions.map(transaction => 
                        <div className="item" key={transaction.transactionId}>
                            <i className="large github middle aligned icon"></i>
                            <div className="content">
                                <div className="header">{transaction.name}</div>
                                <div className="description">{transaction.type}</div>
                                <div className="description">{transaction.groups.join(", ")}</div>
                                {Object.keys(transaction.extensions).map((key, index) => 
                                    <div className="description" key={index}>{key}: {transaction.extensions[key]}</div>
                                )}
                            </div>
                        </div>
                    )}
                    </div>
            </div>
        )
    }
}

export default TransactionList