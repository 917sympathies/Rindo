export interface IInvitation{
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

export interface IUserInfo {
    id: string,
    username: string,
    firstName: string,
    lastName: string,
    email: string,
}

export interface ITask {
    id: string,
    name: string,
    description: string,
    progress: number,
    projectId: string,
    stageId: string,
    index: number,
    responsibleUserId: string | null,
    comments: ITaskComment[] | null,
    startDate: string | null,
    finishDate: string | null
}

export interface ITaskDto {
    name: string,
    description: string,
    projectId: string,
    stageId: string,
    responsibleUserId: string | null,
    comments: ITaskComment[] | null,
    startDate: string | null,
    finishDate: string | null
}

export interface ITaskComment{
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
    users: IUserInfo[]
}

export interface IRoleDto {
    name: string;
    projectId: string;
    color: string;
}

export interface IUserRights{
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

export interface IStage {
    id: string,
    name: string,
    projectId: string,
    tasks: ITask[]
}

export interface IProject{
    id: string,
    name: string,
    description: string,
    chat: IChat,
    chatId: string,
    inviteLink: string,
    startDate: string,
    finishDate: string,
    ownerId: string,
    users: IUser[],
    stages: IStage[],
    roles: IRole[]
}

export interface IProjectDto {
    name: string;
    description: string;
    ownerId: string;
    startDate: string;
    finishDate: string;
    tags: IProjectTag[];
}
  
export interface IProjectTag {
    name: string;
}

export interface CookieInfo {
    userId : string,
    exp: number
}

export interface IStageDto{
    name: string,
    projectId: string
}

export interface IProjectSettings{
    name: string,
    description: string,
    inviteLink: string,
    users: IUserInfo[],
    roles: IRole[] | null,
    startDate: string,
    finishDate: string
}