export interface Invitation {
    id: string,
    userId: string,
    projectId: string,
    projectName: string,
    senderUsername: string
}

export interface IUser {
    id: string,
    username: string,
    firstName: string,
    lastName: string,
    email: string,
    password: string,
    projects: IProject[]
}

export interface CreateProjectDto {
    name: string;
    description: string;
    deadlineDate: Date;
}

export interface UserDto {
    id: string,
    username: string,
    firstName: string,
    lastName: string,
    email: string,
}


export interface TokenDto {
    user: UserDto;
    token: string;
}


export interface TaskDto {
    taskId: string;
    name: string;
    taskNumber: string;
    description: string;
    projectId: string;
    stageId: string;
    index: number;
    assignee?: UserShortInfoDto;
    reporter: UserShortInfoDto;
    comments?: ITaskComment[] | null;
    priority: TaskPriority;
    deadlineDate: Date | undefined;
    created: string;
}

export interface UpdateTaskDto {
    taskId: string;
    name: string;
    description: string;
    stageId: string;
    index: number;
    assigneeId?: string | undefined;
    priority: TaskPriority;
    deadlineDate: Date | undefined;
}

export interface UserShortInfoDto {
    id: string;
    firstName: string;
    lastName: string;
}

export interface ITaskDto {
    name: string;
    description: string;
    projectId: string;
    stageId: string;
    assigneeId: string | null;
    comments: ITaskComment[] | null;
    startDate: string | null;
    finishDate: string | null;
}

export interface AddTaskDto {
    id: string;
    name: string;
    description: string | undefined;
    priority: TaskPriority;
    projectId: string;
    stageId: string;
    assigneeId: string | undefined;
    reporterId: string | undefined;
    deadlineDate: Date | undefined;
}

export enum TaskPriority {
    Low,
    Medium,
    High
}

export interface ITaskComment {
    id: string,
    content: string,
    username: string,
    taskId: string,
    userId: string,
    time: string
}

export interface IRole {
    id: string,
    name: string,
    // canAddRoles: boolean,
    // canAddStage: boolean,
    // canAddTask: boolean,
    // canCompleteTask: boolean,
    // canDeleteStage: boolean,
    // canDeleteTask: boolean,
    // canExcludeUser: boolean,
    // canInviteUser: boolean,
    // canModifyRoles: boolean,
    // canModifyStage: boolean,
    // canModifyTask: boolean,
    // canUseChat: boolean,
    users: UserDto[]
}

export interface IRoleDto {
    name: string;
    projectId: string;
    color: string;
}

export interface IUserRights {
    canAddRoles: boolean,
    canAddStage: boolean,
    canAddTask: boolean,
    canCompleteTask: boolean,
    canDeleteStage: boolean,
    canDeleteTask: boolean,
    canExcludeUser: boolean,
    canInviteUser: boolean,
    canModifyRoles: boolean,
    canModifyStage: boolean,
    canModifyTask: boolean,
    canUseChat: boolean,
}

export enum UserRights {
    CanAddTask = 1 << 1,
    CanModifyTask = 1 << 2,
    CanCompleteTask = 1 << 3,
    CanDeleteTask = 1 << 4,
    CanAddStage = 1 << 5,
    CanModifyStage = 1 << 6,
    CanDeleteStage = 1 << 7,
    CanAddRoles = 1 << 8,
    CanModifyRoles = 1 << 9,
    CanInviteUser = 1 << 10,
    CanExcludeUser = 1 << 11,
    CanUseChat = 1 << 12,
}

export interface Stage {
    id: string,
    customName: string,
    projectId: string,
    index: number;
    type: StageType;
    tasks: TaskDto[]
}

export interface IProject {
    id: string,
    name: string,
    description: string,
    // chat: IChat,
    chatId: string,
    inviteLink: string,
    startDate: string,
    finishDate: string,
    ownerId: string,
    users: IUser[],
    stages: Stage[],
    roles: IRole[]
}

export interface ProjectInfo {
    id: string;
    name: string;
    description: string;
    ownerId: string;
    users: UserDto[];
    roles: any[]; // check type on server
    created: string;
    deadlineDate: string;
}

// deprecated
export interface CookieInfo {
    userId: string,
    exp: number
}

export interface StageDto {
    id: string;
    customName: string;
    projectId: string;
    type: StageType;
    tasks?: TaskDto[];
}

export interface StageShortInfo {
    stageId: string;
    customName: string;
}

export interface AddStageDto {
    customName: string;
    projectId: string;
}

export enum StageType {
    ToDo = 1,
    InProgress = 2,
    ReadyToTest = 3,
    Testing = 4,
    Closed = 5,
    Custom = 6,
}

export interface IProjectSettings {
    name: string,
    description: string,
    inviteLink: string,
    users: UserDto[],
    roles: IRole[] | null,
    startDate: string,
    finishDate: string
}