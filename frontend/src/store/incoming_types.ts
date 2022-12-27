export interface ReceivedDepartment {
    departmentid: number;
    title: string;
    foundationyear: number;
    activityfield: string;
}

export interface ReceivedEmployee {
    employeeid: number;
    login: string;
    name_: string;
    surname: string;
    department: string;
    permission_: string;
}

export interface ReceivedObjective {
    objectiveid: number;
    parentobjective: number;
    title: string;
    company: string;
    department: string;
    termbegin: Date;
    termend: Date;
    estimatedtime: string;
}

export interface ReceivedResponsibility {
    employee: string;
    objective: string;
    timespent: string;
}

export interface ReceivedWorkplace {
    employeeID: number;
    company: ReceivedCompany;
    department: ReceivedDepartment;
    permission_: string;
}

export interface ReceivedCompany {
    companyid: number;
    title: string;
    foundationyear: number;
}
