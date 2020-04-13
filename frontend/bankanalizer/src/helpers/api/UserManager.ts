class UserManager {
    saveUserInStorage(user: any) {
        if (user) {
            localStorage.setItem('user', JSON.stringify(user))
        }
    }

    getUserFromStorage() {
        var user = localStorage.getItem('user')
        if (user == null)
            return null
        
        return JSON.parse(user)
    }
}

export default new UserManager()