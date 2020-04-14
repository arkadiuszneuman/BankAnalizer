import React, { Component } from "react"
import { Redirect } from 'react-router-dom'
import apiConnector from '../../helpers/api/CqrsApiConnector'
import userManager from '../../helpers/api/UserManager'
import Input from '../Controls/InputControl'
import hubConnector from '../../helpers/api/HubConnector'
import {Location} from 'history'

interface IProps {
  location: Location
}

interface IState {
  username: string,
  password: string,
  [key: string]: any,
  redirectTo?: string
}

export default class LoginPage extends Component<IProps, IState> {
  state = {
    username: '',
    password: '',
  } as IState

  handleChange = (e: any) => {
    const { name, value } = e;
    this.setState({ [name]: value });
  }

  login = async (e: any) => {
    e.preventDefault()

    const user = await apiConnector.post("users/authenticate", 
      { username: this.state.username, password: this.state.password})

    if (user) {
      userManager.saveUserInStorage(user)
      hubConnector.init()
      const state = this.props.location.state as any
      this.setState({ redirectTo: state?.from?.pathname || '' })
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