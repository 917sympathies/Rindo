import axios from 'axios';
import {
    AddStageDto,
    AddTaskDto,
    CreateProjectDto,
    IRoleDto,
    Stage,
    IUserRights,
    ProjectInfo,
    TaskDto,
    TokenDto,
    UserDto, UpdateTaskDto,
} from "./types";
import { Chat } from './components/chat/models/chat.model';
import {ProjectTasks, UpdateProjectDto} from "@/types/project.types";

axios.interceptors.request.use(config => {
    const token = localStorage.getItem("token");
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    else {
        // make refreshToken request
    }
    config.headers["Content-Type"] = "application/json";
    config.baseURL = "http://localhost:5000";
    return config;
});

// axios.interceptors.response.use(
//     (response) => {
//         return response.data;
//     },
//     (error) => {
//         if (error.response && error.response.data) {
//             return Promise.reject(error.response.data);
//         }
//         return Promise.reject(error.message);
//     }
// );

// axios.interceptors.response.use(
//     function onFulfilled(response) {
//         // Any status code that lie within the range of 2xx cause this function to trigger
//         // Do something with response data
//         return response;
//     }, function onRejected(error) {
//         // Any status codes that falls outside the range of 2xx cause this function to trigger
//         // Do something with response error
//         return Promise.reject(error);
//     }
// );

axios.interceptors.response.use(
    (response) => response,
    async (error) => {
        const originalRequest = error.config;

        // Prevent infinite loops for the refresh endpoint itself
        if (error.response.status === 401 && originalRequest.url !== 'auth/refresh') {
            try {
                const refreshToken = localStorage.getItem("refreshToken");
                // Call the refresh API
                const response = await axios.post('api/authorization/refresh', {
                    refreshToken: refreshToken,
                });

                const { accessToken, refreshToken: newRefreshToken } = response.data;

                // Update tokens in storage/store
                localStorage.setItem("token", newRefreshToken);

                // Retry the original request with the new token
                originalRequest.headers.Authorization = `Bearer ${accessToken}`;
                return axios(originalRequest);
            } catch (refreshError) {
                // If refresh fails, log out the user
                // store.dispatch(logout());
                // Redirect to login page (handle this in your UI logic)
                return Promise.reject(refreshError);
            }
        }

        return Promise.reject(error);
    }
);

export const getProjectInfoHeader = async (id: string) => {
    return await axios.get(`/api/projects/${id}/header`);
};

export const getProjectsByUserId = async (userId: string) => {
    return await axios.get(`/api/projects?userId=${userId}`);
};

export const getProjectUsers = async (projectId: string) => {
    return await axios.get(`/api/users?projectId=${projectId}`);
};

export const getTasksByUserId = async (userId: string) => {
    return await axios.get<ProjectTasks[]>(`/api/projects/${userId}/user-tasks`);
};

export const authUser = async (userName: string, password: string) => {
    const authInfo = { username: userName, password: password };
    return await axios.post<TokenDto>(`/api/authorization/auth`, authInfo);
};

export const signUpUser = async (
    username: string,
    password: string,
    email: string,
    firstName: string,
    lastName: string
) => {
    return await axios.post(`/api/authorization/signup`, {
        username: username,
        password: password,
        email: email,
        firstName: firstName,
        lastName: lastName,
    });
};

export const getRights = async (roleId: string, userId: string) => {
    //const userId = localStorage.getItem("userId");// непонятно, что за id?
    // проверить на правильность выполнения, должно быть api/roles/projectId=id?userId=..
    return await axios.get(`/api/roles/${roleId}/${userId}`);
};

export const deleteProject = async (id: string) => {
    return await axios.delete(`/api/projects/${id}`);
};

export const getSettingsInfo = async (id: string) => {
    return await axios.get<ProjectInfo>(`/api/projects/${id}/settings`);
};

export const getRolesByProjectId = async (id: string) => {
    return await axios.get(`/api/roles?projectId=${id}`);
};

export const addUserToRole = async (roleId: string, userId: string) => {
    return await axios.put(`/api/roles/${roleId}/add-user?userId=${userId}`);// FIXME
};

export const removeUserFromRole = async (roleId: string, userId: string) => {
    return await axios.put(`/api/roles/${roleId}/remove-user?userId=${userId}`);// FIXME
};

export const saveRoleName = async (roleId: string, name: string) => {
    return await axios.put(`/api/roles/${roleId}/name?name=${name}`);// FIXME
};

export const saveRoleRights = async (roleId: string, rights: IUserRights) => {
    return await axios.put(`/api/roles/${roleId}`);
};

export const createRole = async (role: IRoleDto) => {
    return await axios.post(`/api/roles`, role);
};

export const deleteRole = async (id: string) => {
    return await axios.delete(`/api/roles/${id}`);
};

export const removeUserFromProject = async (projectId: string, username: string) => {
    return await axios.post(`/api/projects/${projectId}/remove?username=${username}`);// FIXME
};

export const createProject = async (project: CreateProjectDto) => {
    return await axios.post<string>(`/api/projects`, project);
};

export const getUsersByProjectId = async (projectId: string) => {
    return await axios.get<UserDto[]>(`/api/users?projectId=${projectId}`);
};

export const addTask = async (task: AddTaskDto) => {
    return await axios.post(`/api/tasks`, task);
};

export const deleteTask = async (id: string) => {
    return await axios.delete(`/api/tasks/${id}`);
};

export const getTask = async (id: string) => {
    return await axios.get<TaskDto>(`/api/tasks/${id}`);
};

export const updateTask = async (taskDto: UpdateTaskDto) => {
    return await axios.put(`/api/tasks`, taskDto);
};

export const inviteUserToProject = async (projectId: string, username: string) => {
    return await axios.post(`/api/projects/${projectId}/invite?username=${username}`); // FIXME
};

export const getChatInfo = async (chatId: string) => {
    return await axios.get<Chat>(`/api/chat/${chatId}`);
};

export const getUser = async (id: string) => {
    return await axios.get<UserDto>(`/api/users/${id}`);
};

export const getTasksByProjectId = async (projectId: string) => {
    return await axios.get(`/api/tasks/?projectId=${projectId}`);
};

export const getInvitesByProjectId = async (projectId: string) => {
    return await axios.get(`/api/invitations/project?projectId=${projectId}`);
};

export const getInvitesByUserId = async (userId: string) => {
    return await axios.get(`/api/invitations/user?userId=${userId}`
    );
};

export const deleteInvite = async (id: string) => {
    return await axios.delete(`/api/invitations/${id}`);
};

export const addStage = async (stage: AddStageDto) => {
    return await axios.post(`/api/stages`, stage);
};

export const deleteStage = async (id: string) => {
    return await axios.delete(`/api/stages/${id}`);
};

export const updateTaskStage = async (taskId: string, stageId: string) => {
    return await axios.patch(`/api/tasks/${taskId}?stageId=${stageId}`);
}

export const getStagesByProjectId = async (projectId: string) => {
    return await axios.get<Stage[]>(`/api/stages?projectId=${projectId}`);
};

export const getTasksCommentsAmount = async (taskId: string) => {
    return await axios.get(`/api/comments?taskId=${taskId}`);
};

export const updateUser = async (userDto: UserDto) => {
    return await axios.put(`/api/users`, userDto);
}

export const updateProject = async (updateProjectDto: UpdateProjectDto) => {
    return await axios.put(`/api/projects`, updateProjectDto);
}
