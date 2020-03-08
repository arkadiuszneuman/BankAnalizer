import HubConnector from './HubConnector'

export default class ApiConnector {
    _apiAddress = "https://localhost:5001/api/"

    _executeMethod =  async (methodName, methodType, body, headers) => {
        let finalHeaders = {
            'Content-Type': 'application/json',
            'connectionId': (await HubConnector).getConnectionId(),
            'userId': (await HubConnector).UserId,
            ...headers
        }

        const user = localStorage.getItem('user')
        if (user != null) {
            const parsedUser = JSON.parse(user)
            finalHeaders = {
                'Authorization': 'Bearer ' + parsedUser.token,
                ...finalHeaders
            }
        }

        return await fetch(this._apiAddress + methodName, { 
            method: methodType, 
            headers: finalHeaders,
            body: JSON.stringify(body)
        })
    }
    
    _executeMethodAndParseResult = async (methodName, methodType, body, headers) => {
        var result = await this._executeMethod(methodName, methodType, body, headers)

        if (!result.ok)
            throw result;

        return result.json()
    }

    _prepareGetQuery = (params) => {
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

        return query
    }

    get = async (methodName, params) => {
        let query = this._prepareGetQuery(params)
        return await this._executeMethodAndParseResult(methodName + query, 'get')
    }

    getFile = async (methodName, params) => {
        let query = this._prepareGetQuery(params)
        let response = await this._executeMethod(methodName + query, 'get')
        return response.blob()
    }

    put = async (methodName) => {
        return await this._executeMethodAndParseResult(methodName, 'put')
    }

    post = async (methodName, body, headers) => {
        return await this._executeMethodAndParseResult(methodName, 'post', body, headers)
    }

    delete = async (methodName, body) => {
        return await this._executeMethodAndParseResult(methodName, 'delete', body)
    }

    uploadFile =  async (methodName, file, headers) => {
        const finalHeaders = {
            'connectionId': HubConnector.getConnectionId(),
            ...headers
        }

        return await fetch(this._apiAddress + methodName, { 
            method: 'post', 
            headers: finalHeaders,
            body: file
        })
    }
}