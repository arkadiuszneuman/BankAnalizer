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

        if (!result.ok)
            throw result;

        return result.json()
    }

    get = async (methodName, params) => {
        let query = '?';
        let isFirstQuery = true;

        for (var param in params) {
            let paramValue = params[param];
            if (params[param] instanceof Date) {
                paramValue = params[param].toISOString()
            }

            if (!paramValue)
                continue;

            if (isFirstQuery) {
                isFirstQuery = false
            } else {
                query += '&'
            }
            query += param + "=" + paramValue
        }

        if (query === '?')
            query = ''

        return await this._executeMethod(methodName + query, 'get')
    }

    put = async (methodName) => {
        return await this._executeMethod(methodName, 'put')
    }

    post = async (methodName, body, headers) => {
        return await this._executeMethod(methodName, 'post', body)
    }

    delete = async (methodName, body) => {
        return await this._executeMethod(methodName, 'delete', body)
    }
}