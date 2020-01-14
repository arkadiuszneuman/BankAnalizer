import React, { Component } from 'react';
import ApiConnector from '../../helpers/api/ApiConnector'

class TransactionList extends Component {
    connector = new ApiConnector()

    state = {
        transactions: []
    }

    componentDidMount = async () => {
        const transactions = await this.connector.get("transaction")
        this.setState({transactions: transactions});
    }

    render() {
        return (
            <div>
                {this.state.transactions.map(transaction => 
                    <div>{transaction.name} {transaction.type}</div>
                )}
            </div>
        )
    }
}

export default TransactionList