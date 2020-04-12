import React, { Component } from 'react';
import apiConnector from '../../helpers/api/CqrsApiConnector'
import download from 'downloadjs'

export default class Exporter extends Component {
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