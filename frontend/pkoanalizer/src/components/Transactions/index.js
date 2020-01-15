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
                <table className="ui very basic celled table">
                    <thead>
                        <tr>
                            <th>Name</th>
                            <th>Type</th>
                        </tr>
                    </thead>
                    <tbody>
                    {this.state.transactions.map(transaction => 
                        <tr key={transaction.transactionId}>
                            <td>{transaction.name}</td>
                            <td>{transaction.type}</td>
                        </tr>
                    )}
                    </tbody>
                </table>
            </div>
        )
    }
}

export default TransactionList