import userManager from '../api/UserManager'

export default class ApiConnector {
    _apiAddress = "https://localhost:5001/api/"

    _executeMethod =  async (methodName, methodType, body, headers) => {
        let finalHeaders = {
            'Content-Type': 'application/json',
            ...headers
        }

        finalHeaders = this._addAuthorizationHeaderToHeaders(finalHeaders);

        const result = await fetch(this._apiAddress + methodName, { 
            method: methodType, 
            headers: finalHeaders,
            body: JSON.stringify(body)
        })

        if (result.status === 401) {
            window.location.href = '/login'
        }

        return result
    }

    _addAuthorizationHeaderToHeaders = (headers) => {
        const user = userManager.getUserFromStorage()
        if (user == null) {
            return headers
        }

        return {
            'Authorization': 'Bearer ' + user.token,
            ...headers
        }
    }
    
    _executeMethodAndParseResult = async (methodName, methodType, body, headers) => {
        const result = await this._executeMethod(methodName, methodType, body, headers)

        if (!result.ok)
            throw result;

        var bodyAsText = await result.text()
        if (bodyAsText === '')
            return null

        return JSON.parse(bodyAsText)
    }

    _prepareGetQuery = (params) => {
        let query = '?';
        let isFirstQuery = true;

        for (var param in params) {
            let paramValue = params[param]

            if (paramValue instanceof Array) {
                if (paramValue.length > 0) {
                    if (query.length !== 0) {
                        query += '&'
                    }

                    query += paramValue.map(p => param + '=' + p).join('&')
                }
                continue
            }

            if (paramValue instanceof Date) {
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
            ...headers
        }

        var result = await fetch(this._apiAddress + methodName, { 
            method: 'post', 
            headers: this._addAuthorizationHeaderToHeaders(finalHeaders),
            body: file
        })

        if (result.status === 401) {
            window.location.href = '/login'
        }

        var bodyAsText = await result.text()
        if (bodyAsText === '')
            return null

        return JSON.parse(bodyAsText)
    }
}