import React from 'react'
import TransactionGroups from './TransactionGroups'

export default function TransactionRow(props) {
    var transaction = props.transaction
    var showGroups = props.showGroups ?? true

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