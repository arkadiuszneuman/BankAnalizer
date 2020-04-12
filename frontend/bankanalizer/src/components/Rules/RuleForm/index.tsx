import React, { Component } from 'react';
import connector from '../../../helpers/api/CqrsApiConnector'
import TransactionList from '../../TransactionView/TransactionList'
import hubConnector from '../../../helpers/api/HubConnector'
import {
    NavLink
  } from "react-router-dom"

declare const $: any

export interface IProps {
    rule: any,
    onAccept: Function
}

interface IState {
    transactionTypes: Array<any>,
    transactionColumns: Array<any>,
    rule: any,
    fitTransactions: Array<any>
}

class RuleForm extends Component<IProps, IState> {
    state = {
        transactionTypes: [],
        transactionColumns: [],
        rule: {...this.props.rule},
        fitTransactions: []
    } as IState

    async componentDidMount() {
        $('.ui.dropdown.clearable').dropdown({ clearable: true })
        $('.ui.dropdown').dropdown()

        const transactionTypesPromise = connector.get("transaction-type")
        const transactionColumnsPromise = connector.get("transaction/transaction-columns")

        const result = await Promise.all([transactionTypesPromise, transactionColumnsPromise]);
        const transactionTypes = result[0];
        const transactionColumns = result[1];
        
        const rule = this.state.rule;
        rule.columnId = rule.columnId || transactionColumns[0].id;
        rule.type = rule.type || 'Contains';
        this.setState({transactionTypes: transactionTypes, transactionColumns: transactionColumns, rule: rule});

        await this.findTransactions(rule)
    }

    handleChange = async (event: any) => {
        const rule = this.state.rule
        rule[event.target.name] = event.target.value
        this.setState({ rule: rule });

        await this.findTransactions(rule)
    }

    findTransactions = async (rule: any) => {
        if (rule.text) {
            const transactions = await connector.post('transaction/find-transactions-from-rule', rule)
            transactions.forEach((element: any) => {
                element.extensions = JSON.parse(element.extensions) ?? ""
            });
            this.setState({fitTransactions: transactions});
        }
    }

    save = async () => {
        await connector.post("rule", this.state.rule)
        // await hubConnector.handleCommandResult(connector.post("rule", this.state.rule))

        if (this.props.onAccept) {
            this.props.onAccept(this.state.rule)
        }
    }

    render() {
        const rule = this.state.rule
        return (
        <div>
            <form className="ui form">
                <div className="field">
                    <label>Rule name</label>
                    <input type="text" value={rule.ruleName || ''} name="ruleName" onChange={this.handleChange} placeholder="Rule name" />
                </div>
                <div className="three fields">
                    <div className="field">
                        <label>Column</label>
                        <select className="ui fluid dropdown" value={rule.columnId || ''} name="columnId" onChange={this.handleChange}>
                        {this.state.transactionColumns.map((transactionColumn: any) => 
                            <option key={transactionColumn.id} value={transactionColumn.id}>{transactionColumn.name}</option>
                        )}
                        </select>
                    </div>
                    <div className="field">
                        <label>Type</label>
                        <select className="ui fluid dropdown" value={rule.type || ''} name="type" onChange={this.handleChange}>
                            <option value="contains">Contains</option>
                        </select>
                    </div>
                    <div className="field">
                        <label>Text</label>
                        <input type="text" placeholder="Text" value={rule.text || ''} name="text" onChange={this.handleChange} />
                    </div>
                </div>
                <div className="two fields">
                    <div className="field">
                        <label>Group name</label>
                        <input type="text" placeholder="Group name" value={rule.groupName || ''} name="groupName" onChange={this.handleChange} />
                    </div>
                    <div className="field">
                        <label>Transaction type</label>
                        <select className="ui fluid dropdown clearable" value={rule.bankTransactionTypeId || ''} name="bankTransactionTypeId" onChange={this.handleChange}>
                            <option value="">All transactions types</option>
                        {this.state.transactionTypes.map((transactionType: any) => 
                            <option key={transactionType.id} value={transactionType.id}>{transactionType.name}</option>
                        )}
                        </select>
                    </div>
                </div>
                <NavLink className="ui primary button" onClick={this.save} to="/rules">Save</NavLink>
                <NavLink className="ui button" to="/rules">Discard</NavLink>
            </form>
            <TransactionList transactions={this.state.fitTransactions} showGroups={false} />
        </div>
        )
    }
}

export default RuleForm