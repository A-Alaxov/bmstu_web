export const chooseWorkplace = (workplaceId: number) => {
  return fetch(process.env.VUE_APP_WORKPLACES_URL + `/` + workplaceId, {
    method: "POST",
    credentials: "include",
    headers: [
      ['Accept', 'application/json'],
      ['Authorization', null],
      ['Content-Type', 'application/json;charset=utf-8'],
    ] as HeadersInit,
  })
}

export const getAllWorkplaces = () => {
  return fetch(process.env.VUE_APP_WORKPLACES_URL, {
    method: "GET",
    credentials: "include",
    headers: [
      ['Accept', 'application/json'],
      ['Authorization', null],
      ['Content-Type', 'application/json;charset=utf-8'],
    ] as HeadersInit,
  })
}
