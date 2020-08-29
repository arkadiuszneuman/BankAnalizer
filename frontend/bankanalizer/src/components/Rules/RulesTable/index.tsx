import React, { Component } from 'react';
import {
    Switch,
    Route,
    Link,
    withRouter,
    RouteComponentProps
  } from "react-router-dom";
import { apiConnector } from '../../../helpers/BankAnalizer'
import RuleForm from '../RuleForm'
import { v4 } from 'uuid';
import _ from 'lodash'

type PathParamsType = {
  path: string,
  url: string
}

type PropsType = RouteComponentProps<PathParamsType> & {
}

interface IState {
  rules: Array<any>,
  currentRule: any
}

class Rules extends Component<PropsType, IState> {
  state = {
    rules: [],
    currentRule: {}
  } as IState

  async componentDidMount() {
    var rules = await apiConnector.get("rule")
    this.setState({rules: rules});
  }

  setCurrentRule = (rule: any) => {
    this.setState({currentRule: rule})
  }

  ruleAccepted = (rule: any) => {
    if (rule.id) {
      Object.assign(this.state.currentRule, rule);
      this.setState({ rules: this.state.rules});
    } else {
      const rules = this.state.rules;
      rule.id = v4()
      rules.unshift(rule)
      this.setState({rules: rules})
    }
  }

  deleteRule(rule: any) {
      apiConnector.delete(`rule/${rule.id}`);
      const rules = this.state.rules;
      _.remove(rules, el => el.id === rule.id);
      this.setState({rules: rules})
  }

  render() {
    let { path, url } = this.props.match;

    return (
      <div>
        <Switch>
          <Route exact path={path}>
            <Link onClick={e => this.setCurrentRule({})} to={`${url}/edit`}>Add</Link>
            <div></div>
            <div className="ui relaxed divided list">
            {this.state.rules.map(rule => 
              <div className="item" key={rule.id}>
                <div className="right floated content">
                  <div className="ui icon buttons">
                    <div onClick={e => this.deleteRule(rule)} className="ui red button"><i className="trash icon"></i></div>
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