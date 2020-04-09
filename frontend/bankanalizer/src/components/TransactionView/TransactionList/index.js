import React from 'react'
import TransactionRow from './TransactionRow'

export default function TransactionList(props) {
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