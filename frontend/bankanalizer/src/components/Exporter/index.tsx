import React, { Component } from 'react';
import {apiConnector} from '../../helpers/BankAnalizer'
import download from 'downloadjs'

interface IState {
  isLoading: boolean
}

export default class Exporter extends Component<{}, IState> {
  state = {
    isLoading: false,
  }

  export = async () => {
    this.setState({isLoading: true})

    try {
        const result = await apiConnector.getFile("transaction/export");
        download(result, 'bankanalizer_export.json')
    }
    finally {
        this.setState({isLoading: false})
    }
  }

  render() {
    const loading = this.state.isLoading ? 'loading' : ''
    const classes = `ui button ${loading}`

    return (
      <div>
        <button onClick={this.export} className={classes}>Export</button>
      </div>
    )
  }
}