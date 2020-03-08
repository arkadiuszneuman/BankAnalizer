import React, { Component } from "react";
import { Redirect } from 'react-router-dom'
import ApiConnector from '../../helpers/api/ApiConnector'

export default class LoginPage extends Component {
  connector = new ApiConnector()

  state = {
    username: '',
    password: '',
    redirectTo: null
  }

  handleChange = e => {
    const { name, value } = e.target;
    this.setState({ [name]: value });
  }

  login = async (e) => {
    e.preventDefault()

    const user = await this.connector.post("users/authenticate", 
      { username: this.state.username, password: this.state.password})

    if (user) {
      localStorage.setItem('user', JSON.stringify(user))
      this.setState({ redirectTo: this.props.location.state?.from?.pathname || '' })
    }
  }

  render() {
    const username = this.state.username
    const password = this.state.password

    if (this.state.redirectTo != null) {
      return <Redirect to={this.state.redirectTo} />
    }

    return (
      <form className="ui form">
        <div className="field">
            <label>Username</label>
            <input type="text" value={username} name="username" onChange={this.handleChange} placeholder="Username" />
        </div>
        <div className="field">
            <label>Password</label>
            <input type="password" value={password} name="password" onChange={this.handleChange} placeholder="Password" />
        </div>
        <button className="ui primary button" onClick={this.login}>Login</button>
    </form>
    )
  }
}