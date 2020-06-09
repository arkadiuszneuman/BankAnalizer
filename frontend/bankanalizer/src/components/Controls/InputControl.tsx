import React, { Component } from "react"

interface IProps {
    name: string,
    text?: string,
    initValue?: string,
    type?: string,
    onChange?: (change: any) => void,
    onKeyDown?: (key: any) => void
}

interface IState {
    value: string,
    type: string
}

export default class InputControl extends Component<IProps, IState> {
    state = {
        value: this.props.initValue ?? '',
        type: this.props.type ?? 'text'
    }

    handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        this.setState({ value: e.target.value });
        if (this.props.onChange) {
            this.props.onChange({ name: this.props.name, value: e.target.value })
        }
    }

    handleKeyDown = (e: React.KeyboardEvent<HTMLInputElement>) => {
        if (this.props.onKeyDown) {
            this.props.onKeyDown(e.target)
        }
    }

    render() {
        const text = this.props.text
        const name = this.props.name
        const value = this.state.value
        const type = this.state.type

        return (
            <div className="field">
                <label>{text}</label>
                <input type={type} value={value} name={name} onChange={this.handleChange} onKeyDown={this.handleKeyDown} placeholder={text} />
            </div>
        )
    }
}