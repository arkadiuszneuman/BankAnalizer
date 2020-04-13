import React, { Component } from 'react';

class UsersSelector extends Component {
    state = {
        users: this.props.users,
        selectedUsers: [],
        isInit: true
    }

    componentDidMount = async () => {
        window.$('.bank-transactions-users-selector').dropdown({
            onAdd: (id) => {
                if (!this.state.isInit) {
                    const user = this.state.users.filter(u => u.id === id)[0]
                    const selectedUsers = this.state.selectedUsers
                    selectedUsers.push(user)
                    this.handleChange(selectedUsers)
                }
            },
            onRemove: (id) => {
                if (!this.state.isInit) {
                    this.handleChange(this.state.selectedUsers.filter(u => u.id !== id))
                }
            }
        })
    }

    componentDidUpdate = prevProps => {
        if (JSON.stringify(prevProps.users) !== JSON.stringify(this.props.users)) {
            this.setState({ users: this.props.users })
            
            
            setTimeout(() => {
                this.props.initSelectedUsers.forEach(user => {
                    window.$('.dropdown').dropdown('set selected', user.id)
                })

                this.setState({ isInit: false, selectedUsers: this.props.initSelectedUsers })
            })
        }
    }

    handleChange = selectedUsers => {
        this.setState({ selectedUsers: Array.from(selectedUsers) })

        if (this.props.onChange) {
            this.props.onChange(selectedUsers)
        }
    }

    getLink = user => {
        return `https://eu.ui-avatars.com/api/?name=${user.firstName}+${user.lastName}&rounded=true&size=22`
    }

    render() {
        const users = this.state.users
        
        return (
            <div>
                <div className="ui sub header">Users selection</div>
                <div className="ui fluid multiple search normal selection dropdown bank-transactions-users-selector">
                    <input type="hidden" name="users" />
                    <i className="dropdown icon"></i>
                    <div className="default text">Select users</div>
                    <div className="menu">
                        {users.map(user => 
                            <div className="item" data-value={user.id} key={user.id}><img className="ui mini avatar image" src={this.getLink(user)} alt="" />{user.firstName} {user.lastName}</div>
                        )}
                    </div>
                </div>
            </div>
        );
    }
}

export default UsersSelector;