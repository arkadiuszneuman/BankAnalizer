import React, { Component } from 'react';

class Importer extends Component {
  import = () => {
    alert("asd")
  }

  render() {
    return (
      <div>
        <input type="text" />
        <button onClick={this.import}>Add</button>
      </div>
    )
  }
}

export default Importer