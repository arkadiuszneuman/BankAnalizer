import React from 'react'
import TransactionRow from './TransactionRow'

interface IProps {
    transactions: any[],
    showGroups?: boolean
}

export default (props: IProps) => {
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