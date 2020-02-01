import HubConnector from './HubConnector'

export default class ApiConnector {
    _apiAddress = "https://localhost:5001/api/"
    
    _executeMethod = async (methodName, methodType, headers, body) => {
        var result = await fetch(this._apiAddress + methodName, { 
            method: methodType, 
            headers: {
                'Content-Type': 'application/json',
                'connectionId': HubConnector.getConnectionId()
            },
            body: JSON.stringify(body)
        })

        return result.json();
    }

    get = async (methodName, headers) => {
        return await this._executeMethod(methodName, 'get', headers)
    }

    put = async (methodName, headers) => {
        return await this._executeMethod(methodName, 'put', headers)
    }

    post = async (methodName, body, headers) => {
        return await this._executeMethod(methodName, 'post', headers, body)
    }

    delete = async (methodName, headers) => {
        return await this._executeMethod(methodName, 'delete', headers)
    }
}