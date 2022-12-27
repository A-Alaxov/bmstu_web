export const getAllDepartments = () => {
  return fetch(process.env.VUE_APP_DEPARTMENTS_URL, {
    method: "GET",
    credentials: "include",
    headers: [
      ['Accept', 'application/json'],
      ['Authorization', null],
      ['Content-Type', 'application/json;charset=utf-8'],
    ] as HeadersInit,
  })
}

export const addNewDepartment = (title: string, foundationyear: number, activityfield: string) => {
  return fetch(process.env.VUE_APP_DEPARTMENTS_URL, {
    method: "POST",
    credentials: "include",
    headers: [
      ['Accept', 'application/json'],
      ['Authorization', null],
      ['Content-Type', 'application/json;charset=utf-8'],
    ] as HeadersInit,
    body: JSON.stringify({
      title: title,
      company: -1,
      foundationyear: foundationyear,
      activityfield: activityfield,
    })
  })
}

export const putDepartment = (dep_id: number, title: string, foundationyear: number, activityfield: string) => {
  return fetch(process.env.VUE_APP_DEPARTMENTS_URL + `/` + dep_id, {
    method: "PUT",
    credentials: "include",
    headers: [
      ['Accept', 'application/json'],
      ['Authorization', null],
      ['Content-Type', 'application/json;charset=utf-8'],
    ] as HeadersInit,
    body: JSON.stringify({
      title: title,
      company: -1,
      foundationyear: foundationyear,
      activityfield: activityfield,
    })
  })
}

export const removeDepartment = (department_id: number) => {
  return fetch(process.env.VUE_APP_DEPARTMENTS_URL + `/` + department_id, {
    method: "DELETE",
    credentials: "include",
    headers: [
      ['Accept', 'application/json'],
      ['Authorization', null],
      ['Content-Type', 'application/json;charset=utf-8'],
    ] as HeadersInit,
  })
}
