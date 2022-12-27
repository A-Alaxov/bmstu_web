export const getAllEmployees = () => {
  return fetch(process.env.VUE_APP_EMPLOYEES_URL, {
    method: "GET",
    credentials: "include",
    headers: [
      ['Accept', 'application/json'],
      ['Authorization', null],
      ['Content-Type', 'application/json;charset=utf-8'],
    ] as HeadersInit,
  })
}

export const addNewEmployee = (login: string, role: number) => {
  return fetch(process.env.VUE_APP_EMPLOYEES_URL, {
    method: "POST",
    credentials: "include",
    headers: [
      ['Accept', 'application/json'],
      ['Authorization', null],
      ['Content-Type', 'application/json;charset=utf-8'],
    ] as HeadersInit,
    body: JSON.stringify({
      employeeID: 0,
      user_: login,
      company: -1,
      department: -1,
      permission_: role,
    })
  })
}

export const removeEmployee = (employee_id: number) => {
  return fetch(process.env.VUE_APP_EMPLOYEES_URL + `/` + employee_id, {
    method: "DELETE",
    credentials: "include",
    headers: [
      ['Accept', 'application/json'],
      ['Authorization', null],
      ['Content-Type', 'application/json;charset=utf-8'],
    ] as HeadersInit,
  })
}

export const getEmplResponsibilities = (employee_id: number) => {
  return fetch(process.env.VUE_APP_EMPLOYEES_URL + '/' + employee_id, {
    method: "GET",
    credentials: "include",
    headers: [
      ['Accept', 'application/json'],
      ['Authorization', null],
      ['Content-Type', 'application/json;charset=utf-8'],
    ] as HeadersInit,
  })
}
