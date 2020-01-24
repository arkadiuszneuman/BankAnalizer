import React, { Component } from 'react';
import {
    BrowserRouter as Router,
    Switch,
    Route,
    Link,
    useParams,
    withRouter
  } from "react-router-dom";
import { HubConnectionBuilder } from '@aspnet/signalr';
import ApiConnector from '../../../helpers/api/ApiConnector'
import RuleForm from '../RuleForm'

class Rules extends Component {
  connector = new ApiConnector()

  state = {
    rules: [],
    currentRule: {}
  }

  async componentDidMount() {
    var rules = await this.connector.get("rule")
    this.setState({rules: rules});
  }

  import = async () => {
    this.setState({currentRule: {}})
  }

  render() {
    let { path, url } = this.props.match;

    return (
      <div>
        <Switch>
        <Route exact path={path}>
          <Link onClick={this.import} to={`${url}/edit`}>Add</Link>
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
                <a className="header">{rule.ruleName}</a>
                <div className="description">{rule.groupName}</div>
              </div>
            </div>
          )}
          </div>
        </Route>
          <Route path={`${path}/edit`}>
            <RuleForm rule={this.state.currentRule} />
          </Route>
        </Switch>
      </div>
    )
  }
}

export default withRouter(Rules)