export const addNewCompany = (title: string, foundationyear: number) => {
  return fetch(process.env.VUE_APP_COMPANIES_URL, {
    method: "POST",
    credentials: "include",
    headers: [
      ['Accept', 'application/json'],
      ['Authorization', null],
      ['Content-Type', 'application/json;charset=utf-8'],
    ] as HeadersInit,
    body: JSON.stringify({
      title: title,
      foundationyear: foundationyear,
    })
  })
}

export const putCompany = (title: string, foundationyear: number) => {
  return fetch(process.env.VUE_APP_COMPANIES_URL, {
    method: "PUT",
    credentials: "include",
    headers: [
      ['Accept', 'application/json'],
      ['Authorization', null],
      ['Content-Type', 'application/json;charset=utf-8'],
    ] as HeadersInit,
    body: JSON.stringify({
      title: title,
      foundationyear: foundationyear,
    })
  })
}

export const removeCompany = () => {
  return fetch(process.env.VUE_APP_COMPANIES_URL, {
    method: "DELETE",
    credentials: "include",
    headers: [
      ['Accept', 'application/json'],
      ['Authorization', null],
      ['Content-Type', 'application/json;charset=utf-8'],
    ] as HeadersInit,
  })
}
