export const getUserInfo = () => {
  return fetch(process.env.VUE_APP_USERS_URL, {
    method: "GET",
    credentials: 'include',
    headers: [
      ['Accept', 'application/json'],
      ['Authorization', null],
      ['Content-Type', 'application/json;charset=utf-8'],
    ] as HeadersInit,
  })
}

export const editUserInfo = (name: string, surname: string) => {
  return fetch(process.env.VUE_APP_USERS_URL, {
    method: "PATCH",
    credentials: 'include',
    headers: [
      ['Accept', 'application/json'],
      ['Authorization', null],
      ['Content-Type', 'application/json;charset=utf-8'],
    ] as HeadersInit,
    body: JSON.stringify({
      login: null,
      password_: null,
      name_: name,
      surname: surname,
    })
  })
}
