import React, { Component } from 'react'
import Input, { IChange } from '../Controls/InputControl'

interface IProps {
    name: string,
    text: string,
    onCommitValue?: (value: string) => void
}

interface IState {
    value: string
}

export default class EnterInput extends Component<IProps, IState> {
    state = {
        value: ''
    } as IState

    onKeyDown = (target: HTMLInputElement, event: React.KeyboardEvent) => {
        const keyCode = event.keyCode || event.which;
        if (keyCode === 13) {
            event.preventDefault();
            const value = target.value
            if (value) {
                this.setState({ value: '' })
                if (this.props.onCommitValue) {
                    this.props.onCommitValue(target.value);
                }
            }
            return false;
        }
    }

    onChange = (change: IChange) => { 
        this.setState({ value: change.value })
    }

    render() {
        const value = this.state.value

        return (
            <Input name="userToConnect" text="User to connect" value={value} onKeyDown={this.onKeyDown} onChange={this.onChange} />
        );
    }
}