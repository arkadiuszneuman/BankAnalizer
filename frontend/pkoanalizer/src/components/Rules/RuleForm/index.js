import React, { Component } from 'react';
import ApiConnector from '../../../helpers/api/ApiConnector'
import {
    BrowserRouter as Router,
    Switch,
    Route,
    Link,
    useParams,
    withRouter,
    NavLink
  } from "react-router-dom";

class RuleForm extends Component {
    connector = new ApiConnector()

    state = {
        transactionTypes: [],
        transactionColumns: []
    }

    async componentDidMount() {
        window.$('.ui.dropdown.clearable').dropdown({ clearable: true })
        window.$('.ui.dropdown').dropdown()

        const transactionTypesPromise = this.connector.get("transaction-type")
        const transactionColumnsPromise = this.connector.get("transaction/transaction-columns")

        const result = await Promise.all([transactionTypesPromise, transactionColumnsPromise]);

        this.setState({transactionTypes: result[0], transactionColumns: result[1]});
    }

    handleChange(event) {
        this.setState({value: event.target.value});
    }

    render() {
        const rule = this.props.rule
        return (
        <div>
            {rule.ruleName}
            <form className="ui form">
                <div className="field">
                    <label>Rule name</label>
                    <input type="text" value={rule.ruleName} name="rule-name" placeholder="Rule name" />
                </div>
                <div className="three fields">
                    <div className="field">
                        <label>Column</label>
                        <select className="ui fluid dropdown" value={rule.columnId}>
                        {this.state.transactionColumns.map(transactionColumn => 
                            <option key={transactionColumn.id} value={transactionColumn.id}>{transactionColumn.name}</option>
                        )}
                        </select>
                    </div>
                    <div className="field">
                        <label>Type</label>
                        <select className="ui fluid dropdown" value={rule.type}>
                            <option value="contains">Contains</option>
                        </select>
                    </div>
                    <div className="field">
                        <label>Text</label>
                        <input type="text" name="text" placeholder="Text" value={rule.text} />
                    </div>
                </div>
                <div className="two fields">
                    <div className="field">
                        <label>Group name</label>
                        <input type="text" name="group-name" placeholder="Group name" value={rule.groupName} />
                    </div>
                    <div className="field">
                        <label>Transaction type</label>
                        <select className="ui fluid dropdown clearable" value={rule.bankTransactionTypeId}>
                            <option value="">All transactions types</option>
                        {this.state.transactionTypes.map(transactionType => 
                            <option key={transactionType.id} value={transactionType.id}>{transactionType.name}</option>
                        )}
                        </select>
                    </div>
                </div>
                <div className="ui primary button">Save</div>
                <NavLink className="ui button" to="/rules">Discard</NavLink>
            </form>
        </div>
        )
    }
}

export default RuleForm