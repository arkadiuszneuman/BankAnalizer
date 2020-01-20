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
    connId: ''
  }

  componentDidMount() {
    console.log("mounted")
  }

  import = async () => {
    
  }

  render() {
    let { path, url } = this.props.match;

    return (
      <div>
        <Switch>
        <Route exact path={path}>
          <Link to={`${url}/add`}>Add</Link>
        </Route>
          <Route path={`${path}/:topicId`}>
            <RuleForm />
          </Route>
        </Switch>
      </div>
    )
  }
}

export default withRouter(Rules)