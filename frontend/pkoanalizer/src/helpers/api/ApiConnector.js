class ApiConnector {
    _apiAddress = "https://localhost:5001/api/"
    
    _executeMethod = async (methodName, methodType, headers) => {
        var result = await fetch(this._apiAddress + methodName, { 
            method: methodType, 
            headers: new Headers(headers)
        })

        return result.json();
    }

    get = async (methodName, headers) => {
        return await this._executeMethod(methodName, 'get', headers)
    }

    put = async (methodName, headers) => {
        return await this._executeMethod(methodName, 'put', headers)
    }

    post = async (methodName, headers) => {
        return await this._executeMethod(methodName, 'post', headers)
    }

    delete = async (methodName, headers) => {
        return await this._executeMethod(methodName, 'delete', headers)
    }
}

export default ApiConnector
