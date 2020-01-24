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
  state = {
    isLoading: false,
    currentRule: {}
  }

  componentDidMount() {
    console.log("mounted")
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