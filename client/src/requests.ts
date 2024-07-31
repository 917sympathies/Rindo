import {
  IProjectDto,
  IRoleDto,
  IStage,
  IStageDto,
  ITask,
  IUserRights,
} from "./types";

const baseUrl = "";

export const GetProjectInfoHeader = async (id: string) => {
  const response = await fetch(`${baseUrl}/api/project/${id}/header`, {
    method: "GET",
    headers: { "Content-Type": "application/json" },
    credentials: "include",
  });
  return response;
};

export const GetProjectsByUserId = async (userId: string) => {
  const response = await fetch(
    `${baseUrl}/api/project?userId=${userId}`,
    {
      method: "GET",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
    }
  );
  return response;
};

export const GetProjectUsers = async (projectId: string) => {
  const response = await fetch(
    `${baseUrl}/api/user?projectId=${projectId}`,
    {
      method: "GET",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
    }
  );
  return response;
};

export const GetTasksByUserId = async (userId: string) => {
  const response = await fetch(
    `${baseUrl}/api/project/${userId}/usertasks`,
    {
      method: "GET",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
    }
  );
  return response;
};

export const AuthUser = async (userName: string, password: string) => {
  const authInfo = { username: userName, password: password };
  const response = await fetch(`${baseUrl}/api/user/auth`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    credentials: "include",
    body: JSON.stringify(authInfo),
  });
  return response;
};

export const SignUpUser = async (
  username: string,
  password: string,
  email: string,
  firstName: string,
  lastName: string
) => {
  const response = await fetch(`${baseUrl}/api/user/signup`, {
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
  return response;
};

export const UpdateUserFirstName = async (
  userId: string,
  firstName: string
) => {
  const response = await fetch(
    `${baseUrl}/api/user/${userId}/firstname?firstName=${firstName}`,
    {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
    }
  );
  return response;
};

export const UpdateUserLastName = async (userId: string, lastName: string) => {
  const response = await fetch(
    `${baseUrl}/api/user/${userId}/lastname?lastName=${lastName}`,
    {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
    }
  );
  return response;
};

export const UpdateUserEmail = async (userId: string, email: string) => {
  const response = await fetch(
    `${baseUrl}/api/user/${userId}/email?email=${email}`,
    {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
    }
  );
  return response;
};

export const GetRights = async (roleId: string, userId: string) => {
  //const userId = localStorage.getItem("userId");
  // непонятно, что за id?
  // проверить на правильность выполнения, должно быть api/role/projectId=id?userId=..
  const response = await fetch(`${baseUrl}/api/role/${roleId}/${userId}`, {
    method: "GET",
    headers: { "Content-Type": "application/json" },
    credentials: "include",
  });
  return response;
};

export const ChangeProjectDescription = async (
  id: string,
  description: string
) => {
  const response = await fetch(
    `${baseUrl}/api/project/${id}/desc?description=${description}`,
    {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
    }
  );
  return response.status;
};

export const ChangeProjectName = async (id: string, name: string) => {
  const response = await fetch(`${baseUrl}/api/project/${id}/name?name=${name}`, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    credentials: "include",
  });
  return response.status;
};

export const DeleteProject = async (id: string) => {
  const response = await fetch(`${baseUrl}/api/project/${id}`, {
    method: "DELETE",
    headers: { "Content-Type": "application/json" },
    credentials: "include",
  });
  return response;
};

export const GetSettingsInfo = async (id: string) => {
  const response = await fetch(
    `${baseUrl}/api/project/${id}/settings`,
    {
      method: "GET",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
    }
  );
  return response;
};

export const GetRolesByProjectId = async (id: string) => {
  const response = await fetch(
    `${baseUrl}/api/role?projectId=${id}`,
    {
      method: "GET",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
    }
  );
  return response;
};

export const AddUserToRole = async (roleId: string, userId: string) => {
  const response = await fetch(
    `${baseUrl}/api/role/${roleId}/adduser?userId=${userId}`,
    {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
    }
  );
  return response;
};

export const RemoveUserFromRole = async (roleId: string, userId: string) => {
  const response = await fetch(
    `${baseUrl}/api/role/${roleId}/removeuser?userId=${userId}`,
    {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
    }
  );
  return response;
};

export const SaveRoleName = async (roleId: string, name: string) => {
  const response = await fetch(`${baseUrl}/api/role/${roleId}/name?name=${name}`, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    credentials: "include",
  });
  return response;
};

export const SaveRoleRights = async (roleId: string, rights: IUserRights) => {
  const response = await fetch(`${baseUrl}/api/role/${roleId}`, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    credentials: "include",
    body: JSON.stringify(rights),
  });
  return response;
};

export const CreateRole = async (role: IRoleDto) => {
  const response = await fetch(`${baseUrl}/api/role`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    credentials: "include",
    body: JSON.stringify(role),
  });
  return response;
};

export const DeleteRole = async (id: string) => {
  const response = await fetch(`${baseUrl}/api/role/${id}`, {
    method: "DELETE",
    headers: { "Content-Type": "application/json" },
    credentials: "include",
  });
  return response;
};

export const RemoveUserFromProject = async (
  projectId: string,
  username: string
) => {
  const response = await fetch(
    `${baseUrl}/api/project/${projectId}/remove?username=${username}`,
    {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
    }
  );
  return response;
};

export const CreateProject = async (project: IProjectDto) => {
  const response = await fetch(`${baseUrl}/api/project`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    credentials: "include",
    body: JSON.stringify(project),
  });
  return response;
};

export const GetUsersByProjectId = async (projectId: string) => {
  const response = await fetch(
    `${baseUrl}/api/user?projectId=${projectId}`,
    {
      method: "GET",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
    }
  );
  return response;
};

export const AddTask = async (task: ITask) => {
  const response = await fetch(`${baseUrl}/api/task`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    credentials: "include",
    body: JSON.stringify(task),
  });
  return response;
};

export const DeleteTask = async (id: string) => {
  const response = await fetch(`${baseUrl}/api/task/${id}`, {
    method: "DELETE",
    headers: { "Content-Type": "application/json" },
    credentials: "include",
  });
};

export const GetTask = async (id: string) => {
  const response = await fetch(`${baseUrl}/api/task/${id}`, {
    method: "GET",
    headers: { "Content-Type": "application/json" },
    credentials: "include",
  });
  return response;
};

export const UpdateTaskStage = async (taskId: string, stageId: string) => {
  const response = await fetch(
    `${baseUrl}/api/stage/${stageId}?taskId=${taskId}`,
    {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
    }
  );
  return response;
};

export const UpdateTaskProgress = async (taskId: string, value: string) => {
  const response = await fetch(
    `${baseUrl}/api/task/${taskId}/progress?number=${value}`,
    {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
    }
  );
  return response;
};

export const UpdateTaskStartDate = async (taskId: string, date: string) => {
  const response = await fetch(
    `${baseUrl}/api/task/${taskId}/start`,
    {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
      body: JSON.stringify(date),
    }
  );
  return response;
};

export const UpdateTaskFinishDate = async (taskId: string, date: string) => {
  const response = await fetch(
    `${baseUrl}/api/task/${taskId}/finish`,
    {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
      body: JSON.stringify(date),
    }
  );
  return response;
};

export const UpdateTaskResponsibleUser = async (taskId: string,value: string) => {
  const response = await fetch(
    `${baseUrl}/api/task/${taskId}/responsible?userId=${value}`,
    {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
    }
  );
  return response;
};

export const UpdateTaskName = async (taskId: string, name: string) => {
  const response = await fetch(
    `${baseUrl}/api/task/${taskId}/name?name=${name}`,
    {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
    }
  );
  return response;
};

export const UpdateTaskDescription = async (
  taskId: string,
  description: string
) => {
  const response = await fetch(
    `${baseUrl}/api/task/${taskId}/description?description=${description}`,
    {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
    }
  );
  return response;
};

export const InviteUserToProject = async (
  projectId: string,
  username: string
) => {
  const response = await fetch(
    `${baseUrl}/api/project/${projectId}/invite?username=${username}`,
    {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
    }
  );
  return response;
};

export const GetChatInfo = async (chatId: string) => {
  const response = await fetch(`${baseUrl}/api/chat/${chatId}`, {
    method: "GET",
    headers: { "Content-Type": "application/json" },
    credentials: "include",
  });
  return response;
};

export const GetUserInfo = async (id: string) => {
  // const userId = localStorage.getItem("userId");
  const response = await fetch(`${baseUrl}/api/user/${id}`, {
    method: "GET",
    headers: { "Content-Type": "application/json" },
    credentials: "include",
  });
  return response;
};

export const GetTasksByProjectId = async (projectId: string) => {
  const response = await fetch(
    `${baseUrl}/api/task/?projectId=${projectId}`,
    {
      method: "GET",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
    }
  );
  return response;
};

export const GetInvitesByProjectId = async (projectId: string) => {
  const response = await fetch(
    `${baseUrl}/api/invitation/project?projectId=${projectId}`,
    {
      method: "GET",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
    }
  );
  return response;
};

export const GetInvitesByUserId = async (userId: string) => {
  const response = await fetch(
    `${baseUrl}/api/invitation/user?userId=${userId}`,
    {
      method: "GET",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
    }
  );
  return response;
};

export const DeleteInvite = async (id: string) => {
  const response = await fetch(`${baseUrl}/api/invitation/${id}`, {
    method: "DELETE",
    headers: { "Content-Type": "application/json" },
    credentials: "include",
  });
  return response;
};

export const AddStage = async (stage: IStageDto) => {
  const response = await fetch(`${baseUrl}/api/stage`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    credentials: "include",
    body: JSON.stringify(stage),
  });
};

export const DeleteStage = async (id: string) => {
  const response = await fetch(`${baseUrl}/api/stage/${id}`, {
    method: "DELETE",
    headers: { "Content-Type": "application/json" },
    credentials: "include",
  });
  return response;
};

export const UpdateProjectStages = async (
  projectId: string,
  stages: IStage[]
) => {
  const response = await fetch(
    `${baseUrl}/api/project/${projectId}/stages`,
    {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
      body: JSON.stringify(stages),
    }
  );
  return response;
};

export const GetStagesByProjectId = async (projectId: string) => {
  const response = await fetch(
    `${baseUrl}/api/stage?projectId=${projectId}`,
    {
      method: "GET",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
    }
  );
  return response;
};

export const GetTasksCommentsAmount = async (taskId: string) => {
  const response = await fetch(
    `${baseUrl}/api/comment?taskId=${taskId}`,
    {
      method: "GET",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
    }
  );
  return response;
};
