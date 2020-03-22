class UserManager {
    saveUserInStorage(user) {
        if (user) {
            localStorage.setItem('user', JSON.stringify(user))
        }
    }

    getUserFromStorage() {
        return localStorage.getItem('user')
    }
}

export default new UserManager()