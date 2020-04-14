import React, { Component } from "react"
import { Redirect } from 'react-router-dom'
import {apiConnector} from '../../helpers/BankAnalizer'
import userManager from '../../helpers/api/UserManager'
import Input from '../Controls/InputControl'

interface IUser {
  firstName: '',
  lastName: '',
  username: '',
  password: '',
}

interface IState {
  user: IUser,
  redirectTo?: string
}

export default class RegisterPage extends Component<{}, IState> {

  state = {
    user: {
      firstName: '',
      lastName: '',
      password: '',
      username: ''
    }
  } as IState

  handleChange = (e: any) => {
    const { name, value } = e.target
    const user = this.state.user as any
    user[name] = value
    this.setState({ user: user })
  }

  onFieldChange = (f: any) => {
    const { name, value } = f
    const user = this.state.user as any
    user[name] = value
    this.setState({ user: user })
  }

  register = async (e: any) => {
    e.preventDefault()

    const stateUser = this.state.user

    await apiConnector.post("users/register", stateUser)
    const user = await apiConnector.post("users/authenticate", 
      { username: stateUser.username, password: stateUser.password})

    if (user) {
      userManager.saveUserInStorage(user)
      this.setState({ redirectTo: '' })
    }
  }

  render() {
    if (this.state.redirectTo != null) {
      return <Redirect to={this.state.redirectTo} />
    }

    return (
      <form className="ui form">
        <div className="two fields">
            <Input name="firstName" text="First Name" onChange={this.onFieldChange} />
            <Input name="lastName" text="Last Name" onChange={this.onFieldChange} />
        </div>
        <Input name="username" text="Username" onChange={this.onFieldChange} />
        <Input type="password" name="password" text="Password" onChange={this.onFieldChange} />
        <button className="ui primary button" onClick={this.register}>Register</button>
    </form>
    )
  }
}