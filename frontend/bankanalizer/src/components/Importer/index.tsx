import React, { Component } from 'react';
import {apiConnector} from '../../helpers/BankAnalizer'

interface IState {
  isLoading: boolean
}

class Importer extends Component<{}, IState> {
  state = {
    isLoading: false,
  }

  onChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e?.target?.files && e.target.files[0]) {
      this.fileUpload(e.target.files[0])
    }
  }

  fileUpload = async (file: File) => {
    this.setState({isLoading: true})

    const formData = new FormData();
    formData.append('file', file)

    try {
      await apiConnector.uploadFile("transaction/import", formData)
    }
    finally {
      this.setState({isLoading: false})
    }
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