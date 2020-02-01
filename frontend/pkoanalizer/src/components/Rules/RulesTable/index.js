import React, { Component } from 'react';
import {
    BrowserRouter as Router,
    Switch,
    Route,
    Link,
    useParams,
    withRouter
  } from "react-router-dom";
import ApiConnector from '../../../helpers/api/ApiConnector'
import RuleForm from '../RuleForm'
import uuid from 'uuid/v4'

class Rules extends Component {
  connector = new ApiConnector()

  state = {
    rules: [],
    currentRule: {}
  }

  constructor() {
    super()
    this.ruleAccepted = this.ruleAccepted.bind(this)
  }

  async componentDidMount() {
    var rules = await this.connector.get("rule")
    this.setState({rules: rules});
  }

  setCurrentRule = (rule) => {
    this.setState({currentRule: rule})
  }

  ruleAccepted(rule) {
    if (rule.id) {
      const originalRule = Object.assign(this.state.currentRule, rule);
      this.setState({ rules: this.state.rules});
    } else {
      const rules = this.state.rules;
      rule.id = uuid()
      rules.push(rule)
      this.setState({rules: rules})
    }
  }

  render() {
    let { path, url } = this.props.match;

    return (
      <div>
        <Switch>
        <Route exact path={path}>
          <Link onClick={e => this.setCurrentRule({})} to={`${url}/edit`}>Add</Link>
          <div className="ui relaxed divided list">
          {this.state.rules.map(rule => 
            <div className="item" key={rule.id}>
              <div className="right floated content">
                <div className="ui icon buttons">
                  <div className="ui red button"><i className="trash icon"></i></div>
                </div>
              </div>
              <i className="large handshake middle aligned icon"></i>
              <div className="content">
                <Link onClick={e => this.setCurrentRule(rule)} className="header" to={`${url}/edit`}>{rule.ruleName}</Link>
                <div className="description">{rule.groupName}</div>
              </div>
            </div>
          )}
          </div>
        </Route>
          <Route path={`${path}/edit`}>
            <RuleForm rule={this.state.currentRule} onAccept={this.ruleAccepted} />
          </Route>
        </Switch>
      </div>
    )
  }
}

export default withRouter(Rules)