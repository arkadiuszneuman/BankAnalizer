import React, { Component } from 'react';
import ApiConnector from '../../helpers/api/ApiConnector'
import download from 'downloadjs'

export default class Exporter extends Component {
  connector = new ApiConnector()

  state = {
    isLoading: false,
  }

  export = async () => {
    this.setState({isLoading: true})

    try {
        const result = await this.connector.getFile("transaction/export");
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