import React, { Component } from 'react';

class Importer extends Component {
  import = () => {
    fetch("https://localhost:5001/api/import")
      .then(response => response.json())
      .then(json => console.log(json));
  }

  render() {
    return (
      <div>
        <button onClick={this.import}>Import</button>
      </div>
    )
  }
}

export default Importer