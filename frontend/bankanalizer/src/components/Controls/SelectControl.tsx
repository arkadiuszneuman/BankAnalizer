import React, { Component } from 'react';

declare const $: any

export interface Option{
    id: string | number,
    name: string
}

interface IProps {
    options: Option[],
    onChange?: (value: string) => void
}

interface IState {
    value: string
}

export default class SelectControl extends Component<IProps, IState> {
    state = {
        value: ''
    }

    componentDidMount = () => {
        $('.bank-transactions-select').dropdown()
    }

    handleChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
        const value = event.target.value
        if (this.props.onChange) {
            this.props.onChange(value)
        }

        this.setState({value: value});
    }

    render() {
        const options = this.props.options ?? []
        return (
            <select className="ui dropdown bank-transactions-select" value={this.state.value} onChange={this.handleChange}>
                {options.map(option => 
                    <option key={option.id} value={option.id}>{option.name}</option>
                )}
            </select>

        );
    }
}