export const registerUser = (username: string, password: string, first_name: string, last_name: string) => {
  return fetch(process.env.VUE_APP_AUTH_URL + '/Register', {
    method: "POST",
    credentials: "include",
    headers: [
      ['Accept', 'application/json'],
      ['Authorization', null],
      ['Content-Type', 'application/json;charset=utf-8'],
    ] as HeadersInit,
    body: JSON.stringify({
      login: username,
      password_: password,
      name_: first_name,
      surname: last_name
    })
  })
}

export const loginUser = (email: string, password: string) => {
  return fetch(process.env.VUE_APP_AUTH_URL + '/Login', {
    method: "POST",
    credentials: "include",
    headers: [
      ['Accept', 'application/json'],
      ['Authorization', null],
      ['Content-Type', 'application/json;charset=utf-8'],
    ] as HeadersInit,
    body: JSON.stringify({
      username: email,
      password: password
    })
  })
}

export const logoutUser = () => {
  return fetch(process.env.VUE_APP_AUTH_URL + '/Logout', {
    method: "GET",
    credentials: "include",
    headers: [
      ['Accept', 'application/json'],
      ['Authorization', null],
      ['Content-Type', 'application/json;charset=utf-8'],
    ] as HeadersInit,
  })
}
