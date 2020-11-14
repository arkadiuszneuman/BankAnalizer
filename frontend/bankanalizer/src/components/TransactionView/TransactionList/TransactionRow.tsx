import React from 'react'
import TransactionGroups from './TransactionGroups'

interface IProps {
    transaction: any,
    showGroups: boolean
}

export default (props: IProps) => {
    var transaction = props.transaction
    var showGroups = props.showGroups ?? true

    const parseDate = (unparsedDate: Date) => {
        const date = new Date(unparsedDate)
        return new Date(date.getFullYear(), date.getMonth() , date.getDate()).toLocaleDateString()
    }

    return (
        <div className="item ui red segment">
            <i className="large money bill alternate middle aligned icon"></i>
            <div className="content">
                <div className="header">{transaction.name}</div>
                <div className="description">{transaction.amount} zł</div>
                <div className="description">{transaction.type}</div>
                <div className="description">{transaction.bankName}</div>
                <div className="description">{parseDate(transaction.transactionDate)}</div>
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