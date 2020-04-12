import React, { Component } from "react"
import { Redirect } from 'react-router-dom'
import apiConnector from '../../helpers/api/CqrsApiConnector'
import userManager from '../../helpers/api/UserManager'
import Input from '../Controls/Input'

export default class LoginPage extends Component {
  state = {
    username: '',
    password: '',
    redirectTo: null
  }

  handleChange = e => {
    const { name, value } = e;
    this.setState({ [name]: value });
  }

  login = async (e) => {
    e.preventDefault()

    const user = await apiConnector.post("users/authenticate", 
      { username: this.state.username, password: this.state.password})

    if (user) {
      userManager.saveUserInStorage(user)
      this.setState({ redirectTo: this.props.location.state?.from?.pathname || '' })
    }
  }

  render() {
    if (this.state.redirectTo != null) {
      return <Redirect to={this.state.redirectTo} />
    }

    return (
      <form className="ui form">
        <Input name="username" text="Username" onChange={this.handleChange} />
        <Input type="password" name="password" text="Password" onChange={this.handleChange} />
        <button className="ui primary button" onClick={this.login}>Login</button>
    </form>
    )
  }
}