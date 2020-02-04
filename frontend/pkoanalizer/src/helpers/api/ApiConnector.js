import HubConnector from './HubConnector'

export default class ApiConnector {
    _apiAddress = "https://localhost:5001/api/"
    
    _executeMethod = async (methodName, methodType, body) => {
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

    get = async (methodName) => {
        return await this._executeMethod(methodName, 'get')
    }

    put = async (methodName) => {
        return await this._executeMethod(methodName, 'put')
    }

    post = async (methodName, body, headers) => {
        return await this._executeMethod(methodName, 'post', body)
    }

    delete = async (methodName, body) => {
        return await this._executeMethod(methodName, 'delete')
    }
}