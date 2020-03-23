import React, { Component } from 'react';
import ApiConnector from '../../helpers/api/ApiConnector'
import hubConnector from '../../helpers/api/HubConnector'

class Importer extends Component {
  connector = new ApiConnector()

  state = {
    isLoading: false,
  }

  import = async () => {
    this.setState({isLoading: true})

    const result = await this.connector.get("transaction/import");
    (await hubConnector).waitForEventResult(result.id, () => {
      this.setState({isLoading: false})
    });
  }

  onChange = e => {
    if (e.target.files[0])
      this.fileUpload(e.target.files[0])
  }

  fileUpload = async file => {
    this.setState({isLoading: true})
    const formData = new FormData();
    formData.append('file', file)

    const result = await this.connector.uploadFile("transaction/import", formData);

    if (result == null) {
      this.setState({isLoading: false})
      return
    }

    (await hubConnector).waitForEventResult(result.id, () => {
      this.setState({isLoading: false})
    });
  }

  render() {
    const loading = this.state.isLoading ? 'loading' : ''
    const classes = `ui icon button ${loading}`

    return (
      <div>
          <label htmlFor="file" className={classes}>
              <i className="file icon"></i>
              Import
          </label>
          <input type="file" id="file" style={{display:"none"}} onChange={this.onChange} />
      </div>
    )
  }
}

export default Importer