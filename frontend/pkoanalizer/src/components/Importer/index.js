import React, { Component } from 'react';
import ApiConnector from '../../helpers/api/ApiConnector'
import HubConnector from '../../helpers/api/HubConnector'

class Importer extends Component {
  connector = new ApiConnector()

  state = {
    isLoading: false,
  }

  import = async () => {
    this.setState({isLoading: true})

    const result = await this.connector.get("transaction/import", { 'connectionId': HubConnector.getConnectionId() });
    HubConnector.waitForEventResult(result.id, () => {
      this.setState({isLoading: false})
    });
  }

  render() {
    const loading = this.state.isLoading ? 'loading' : ''
    const classes = `ui button ${loading}`

    return (
      <div>
        <button onClick={this.import} className={classes}>Import</button>
      </div>
    )
  }
}

export default Importer