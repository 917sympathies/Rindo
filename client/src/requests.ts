import {
    IProjectDto,
    IRoleDto,
    IStage,
    IStageDto,
    ITaskDto,
    IUserRights,
} from "./types";

const baseUrl = "http://localhost:5000";

export const GetProjectInfoHeader = async (id: string) => {
  return await fetch(`${baseUrl}/api/project/${id}/header`, {
    method: "GET",
    headers: { "Content-Type": "application/json" },
    credentials: "include",
  });
};

export const GetProjectsByUserId = async (userId: string) => {
  return await fetch(
    `${baseUrl}/api/project?userId=${userId}`,
    {
      method: "GET",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
    }
  );
};

export const GetProjectUsers = async (projectId: string) => {
  return await fetch(
    `${baseUrl}/api/user?projectId=${projectId}`,
    {
      method: "GET",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
    }
  );
};

export const GetTasksByUserId = async (userId: string) => {
  return await fetch(
    `${baseUrl}/api/project/${userId}/usertasks`,
    {
      method: "GET",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
    }
  );
};

export const AuthUser = async (userName: string, password: string) => {
  const authInfo = { username: userName, password: password };
  return await fetch(`${baseUrl}/api/authorization/auth`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    credentials: "include",
    body: JSON.stringify(authInfo),
  });
};

export const SignUpUser = async (
  username: string,
  password: string,
  email: string,
  firstName: string,
  lastName: string
) => {
  return await fetch(`${baseUrl}/api/authorization/signup`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    credentials: "include",
    body: JSON.stringify({
      username: username,
      password: password,
      email: email,
      firstName: firstName,
      lastName: lastName,
    }),
  });
};

export const UpdateUserFirstName = async (
  userId: string,
  firstName: string
) => {
  return await fetch(
    `${baseUrl}/api/user/${userId}/firstname?firstName=${firstName}`,
    {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
    }
  );
};

export const UpdateUserLastName = async (userId: string, lastName: string) => {
  return await fetch(
    `${baseUrl}/api/user/${userId}/lastname?lastName=${lastName}`,
    {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
    }
  );
};

export const UpdateUserEmail = async (userId: string, email: string) => {
  return await fetch(
    `${baseUrl}/api/user/${userId}/email?email=${email}`,
    {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
    }
  );
};

export const GetRights = async (roleId: string, userId: string) => {
  //const userId = localStorage.getItem("userId");// непонятно, что за id?
  // проверить на правильность выполнения, должно быть api/role/projectId=id?userId=..
  return await fetch(`${baseUrl}/api/role/${roleId}/${userId}`, {
    method: "GET",
    headers: { "Content-Type": "application/json" },
    credentials: "include",
  });
};

export const ChangeProjectDescription = async (
  id: string,
  description: string
) => {
  return await fetch(
    `${baseUrl}/api/project/${id}/desc?description=${description}`,
    {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
    }
  );
};

export const ChangeProjectName = async (id: string, name: string) => {
  return await fetch(`${baseUrl}/api/project/${id}/name?name=${name}`, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    credentials: "include",
  });
};

export const DeleteProject = async (id: string) => {
  return await fetch(`${baseUrl}/api/project/${id}`, {
    method: "DELETE",
    headers: { "Content-Type": "application/json" },
    credentials: "include",
  });
};

export const GetSettingsInfo = async (id: string) => {
  return await fetch(
    `${baseUrl}/api/project/${id}/settings`,
    {
      method: "GET",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
    }
  );
};

export const GetRolesByProjectId = async (id: string) => {
  return await fetch(
    `${baseUrl}/api/role?projectId=${id}`,
    {
      method: "GET",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
    }
  );
};

export const AddUserToRole = async (roleId: string, userId: string) => {
  return await fetch(
    `${baseUrl}/api/role/${roleId}/adduser?userId=${userId}`,
    {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
    }
  );
};

export const RemoveUserFromRole = async (roleId: string, userId: string) => {
  return await fetch(
    `${baseUrl}/api/role/${roleId}/removeuser?userId=${userId}`,
    {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
    }
  );
};

export const SaveRoleName = async (roleId: string, name: string) => {
  return await fetch(`${baseUrl}/api/role/${roleId}/name?name=${name}`, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    credentials: "include",
  });
};

export const SaveRoleRights = async (roleId: string, rights: IUserRights) => {
  return await fetch(`${baseUrl}/api/role/${roleId}`, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    credentials: "include",
    body: JSON.stringify(rights),
  });
};

export const CreateRole = async (role: IRoleDto) => {
  return await fetch(`${baseUrl}/api/role`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    credentials: "include",
    body: JSON.stringify(role),
  });
};

export const DeleteRole = async (id: string) => {
  return await fetch(`${baseUrl}/api/role/${id}`, {
    method: "DELETE",
    headers: { "Content-Type": "application/json" },
    credentials: "include",
  });
};

export const RemoveUserFromProject = async (
  projectId: string,
  username: string
) => {
  return await fetch(
    `${baseUrl}/api/project/${projectId}/remove?username=${username}`,
    {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
    }
  );
};

export const CreateProject = async (project: IProjectDto) => {
  return await fetch(`${baseUrl}/api/project`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    credentials: "include",
    body: JSON.stringify(project),
  });
};

export const GetUsersByProjectId = async (projectId: string) => {
  return await fetch(
    `${baseUrl}/api/user?projectId=${projectId}`,
    {
      method: "GET",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
    }
  );
};

export const AddTask = async (task: ITaskDto) => {
  return await fetch(`${baseUrl}/api/task`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    credentials: "include",
    body: JSON.stringify(task),
  });
};

export const DeleteTask = async (id: string) => {
  return await fetch(`${baseUrl}/api/task/${id}`, {
    method: "DELETE",
    headers: { "Content-Type": "application/json" },
    credentials: "include",
  });
};

export const GetTask = async (id: string) => {
  return await fetch(`${baseUrl}/api/task/${id}`, {
    method: "GET",
    headers: { "Content-Type": "application/json" },
    credentials: "include",
  });
};

export const UpdateTaskStage = async (taskId: string, stageId: string) => {
  return await fetch(
    `${baseUrl}/api/stage/${stageId}?taskId=${taskId}`,
    {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
    }
  );
};

export const UpdateTaskProgress = async (taskId: string, value: string) => {
  return await fetch(
    `${baseUrl}/api/task/${taskId}/progress?number=${value}`,
    {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
    }
  );
};

export const UpdateTaskStartDate = async (taskId: string, date: string) => {
  return await fetch(
    `${baseUrl}/api/task/${taskId}/start`,
    {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
      body: JSON.stringify(date),
    }
  );
};

export const UpdateTaskFinishDate = async (taskId: string, date: string) => {
  return await fetch(
    `${baseUrl}/api/task/${taskId}/finish`,
    {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
      body: JSON.stringify(date),
    }
  );
};

export const UpdateTaskResponsibleUser = async (taskId: string,value: string) => {
  return await fetch(
    `${baseUrl}/api/task/${taskId}/responsible?userId=${value}`,
    {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
    }
  );
};

export const UpdateTaskName = async (taskId: string, name: string) => {
  return await fetch(
    `${baseUrl}/api/task/${taskId}/name?name=${name}`,
    {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
    }
  );
};

export const UpdateTaskDescription = async (
  taskId: string,
  description: string
) => {
  return await fetch(
    `${baseUrl}/api/task/${taskId}/description?description=${description}`,
    {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
    }
  );
};

export const InviteUserToProject = async (
  projectId: string,
  username: string
) => {
  return await fetch(
    `${baseUrl}/api/project/${projectId}/invite?username=${username}`,
    {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
    }
  );
};

export const GetChatInfo = async (chatId: string) => {
  return await fetch(`${baseUrl}/api/chat/${chatId}`, {
    method: "GET",
    headers: { "Content-Type": "application/json" },
    credentials: "include",
  });
};

export const GetUserInfo = async (id: string) => {
    return await fetch(`${baseUrl}/api/user/${id}`, {
        method: "GET",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
    });
};

export const GetTasksByProjectId = async (projectId: string) => {
    return await fetch(
        `${baseUrl}/api/task/?projectId=${projectId}`, {
        method: "GET",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
    });
};

export const GetInvitesByProjectId = async (projectId: string) => {
    return await fetch(
      `${baseUrl}/api/invitation/project?projectId=${projectId}`, {
          method: "GET",
          headers: { "Content-Type": "application/json" },
          credentials: "include",
    });
};

export const GetInvitesByUserId = async (userId: string) => {
  return await fetch(
    `${baseUrl}/api/invitation/user?userId=${userId}`,
    {
      method: "GET",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
    }
  );
};

export const DeleteInvite = async (id: string) => {
  return await fetch(`${baseUrl}/api/invitation/${id}`, {
    method: "DELETE",
    headers: { "Content-Type": "application/json" },
    credentials: "include",
  });
};

export const AddStage = async (stage: IStageDto) => {
  return await fetch(`${baseUrl}/api/stage`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    credentials: "include",
    body: JSON.stringify(stage),
  });
};

export const DeleteStage = async (id: string) => {
  return await fetch(`${baseUrl}/api/stage/${id}`, {
    method: "DELETE",
    headers: { "Content-Type": "application/json" },
    credentials: "include",
  });
};

export const UpdateProjectStages = async (
  projectId: string,
  stages: IStage[]
) => {
  return await fetch(
    `${baseUrl}/api/project/${projectId}/stages`,
    {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
      body: JSON.stringify(stages),
    }
  );
};

export const GetStagesByProjectId = async (projectId: string) => {
  return await fetch(
    `${baseUrl}/api/stage?projectId=${projectId}`,
    {
      method: "GET",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
    }
  );
};

export const GetTasksCommentsAmount = async (taskId: string) => {
  return await fetch(
    `${baseUrl}/api/comment?taskId=${taskId}`,
    {
      method: "GET",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
    }
  );
};
