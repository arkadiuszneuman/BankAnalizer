import React, { Component } from "react"

export interface IChange {
    name: string,
    value: string,
    event: React.ChangeEvent
}

interface IProps {
    name: string,
    text?: string,
    value?: string,
    type?: string,
    setValue?: (value: string) => void,
    onChange?: (change: IChange) => void,
    onKeyDown?: (key: HTMLInputElement, event: React.KeyboardEvent) => void
    onKeyPress?: (key: any) => void
}

interface IState {
    value: string,
    type: string
}

export default class InputControl extends Component<IProps, IState> {
    state = {
        value: this.props.value ?? '',
        type: this.props.type ?? 'text'
    }

    componentDidUpdate = (prevProps: Readonly<IProps>, prevState: Readonly<IState>) => {
        if (prevProps.value && prevProps.value !== this.props.value && this.props.value !== this.state.value) {
            this.setState({ value: this.props.value as string })
        }
    }

    handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        this.setState({ value: e.target.value });
        if (this.props.onChange) {
            this.props.onChange({ name: this.props.name, value: e.target.value, event: e })
        }
    }

    handleKeyDown = (e: React.KeyboardEvent<HTMLInputElement>) => {
        if (this.props.onKeyDown) {
            this.props.onKeyDown(e.target as HTMLInputElement, e)
        }
    }

    onKeyPress = (e: React.KeyboardEvent<HTMLInputElement>) => {
        if (this.props.onKeyPress) {
            this.props.onKeyPress(e)
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
                <input  type={type} 
                        value={value}
                        name={name}
                        placeholder={text}
                        onChange={this.handleChange}
                        onKeyPress={this.onKeyPress}
                        onKeyDown={this.handleKeyDown} />
            </div>
        )
    }
}