export const addNewTask = (title: string, begin: string, end: string, estTime: number) => {
  return fetch(process.env.VUE_APP_TASKS_URL, {
    method: "POST",
    credentials: "include",
    headers: [
      ['Accept', 'application/json'],
      ['Authorization', null],
      ['Content-Type', 'application/json;charset=utf-8'],
    ] as HeadersInit,
    body: JSON.stringify({
      parentobjective: 0,
      title: title,
      department: -1,
      termbegin: begin,
      termend: end,
      estimatedtime: estTime,
    })
  })
}

export const addNewSubTask = (parent_id: number, title: string, begin: string, end: string, estTime: number) => {
  return fetch(process.env.VUE_APP_TASKS_URL + `/` + parent_id, {
    method: "POST",
    credentials: "include",
    headers: [
      ['Accept', 'application/json'],
      ['Authorization', null],
      ['Content-Type', 'application/json;charset=utf-8'],
    ] as HeadersInit,
    body: JSON.stringify({
      parentobjective: 0,
      title: title,
      department: -1,
      termbegin: begin,
      termend: end,
      estimatedtime: estTime,
    })
  })
}

export const getAllTasks = () => {
  return fetch(process.env.VUE_APP_TASKS_URL, {
    method: "GET",
    credentials: "include",
    headers: [
      ['Accept', 'application/json'],
      ['Authorization', null],
      ['Content-Type', 'application/json;charset=utf-8'],
    ] as HeadersInit,
  })
}

export const getTasks = (curId: number) => {
  return fetch(process.env.VUE_APP_TASKS_URL + `/` + curId, {
    method: "GET",
    credentials: "include",
    headers: [
      ['Accept', 'application/json'],
      ['Authorization', null],
      ['Content-Type', 'application/json;charset=utf-8'],
    ] as HeadersInit,
  })
}

export const putTask = (task_id: number, title: string, begin: string, end: string, estTime: number) => {
  return fetch(process.env.VUE_APP_TASKS_URL + `/` + task_id, {
    method: "PUT",
    credentials: "include",
    headers: [
      ['Accept', 'application/json'],
      ['Authorization', null],
      ['Content-Type', 'application/json;charset=utf-8'],
    ] as HeadersInit,
    body: JSON.stringify({
      parentobjective: 0,
      title: title,
      department: -1,
      termbegin: begin,
      termend: end,
      estimatedtime: estTime,
    })
  })
}

export const removeTask = (task_id: number) => {
  return fetch(process.env.VUE_APP_TASKS_URL + `/` + task_id, {
    method: "DELETE",
    credentials: "include",
    headers: [
      ['Accept', 'application/json'],
      ['Authorization', null],
      ['Content-Type', 'application/json;charset=utf-8'],
    ] as HeadersInit,
  })
}

export const getResponsibleEmployees = (curId: number) => {
  return fetch(process.env.VUE_APP_TASKS_URL + `/` + curId + '/Responsibles', {
    method: "GET",
    credentials: "include",
    headers: [
      ['Accept', 'application/json'],
      ['Authorization', null],
      ['Content-Type', 'application/json;charset=utf-8'],
    ] as HeadersInit,
  })
}
