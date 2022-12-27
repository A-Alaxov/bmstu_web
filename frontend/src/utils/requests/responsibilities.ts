export const addNewResponsibility = (employee: number, task: number, timeamount: number) => {
  return fetch(process.env.VUE_APP_RESPONSIBILITIES_URL, {
    method: "POST",
    credentials: "include",
    headers: [
      ['Accept', 'application/json'],
      ['Authorization', null],
      ['Content-Type', 'application/json;charset=utf-8'],
    ] as HeadersInit,
    body: JSON.stringify({
      employee: employee,
      objective: task,
      timeamount: timeamount,
    })
  })
}

export const putResp = (employee: number, task: number, timeamount: number) => {
  return fetch(process.env.VUE_APP_RESPONSIBILITIES_URL, {
    method: "PUT",
    credentials: "include",
    headers: [
      ['Accept', 'application/json'],
      ['Authorization', null],
      ['Content-Type', 'application/json;charset=utf-8'],
    ] as HeadersInit,
    body: JSON.stringify({
      employee: employee,
      objective: task,
      timeamount: timeamount,
    })
  })
}
