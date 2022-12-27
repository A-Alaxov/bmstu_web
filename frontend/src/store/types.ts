export interface Department {
    id: number;
    title: string;
    foundationyear: number;
    activityfield: string;
}

export interface Employee {
    id: number;
    login: string;
    name: string;
    department: string;
    role: string;
}

export interface Objective {
    id: number;
    pid: number;
    name: string;
    company: string;
    department: string;
    startDate: Date;
    endDate: Date;
    estimatedTime: string;
    parent: Objective | null;
    children: Objective[];
}

export interface Responsibility {
    employee: string;
    task: string;
    timespent: string;
}

export interface Workplace {
    employee_id: number;
    company: string;
    department: string;
    role: string;
}
