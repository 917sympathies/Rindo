"use client";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { Label } from "@/components/ui/label";
import { Avatar } from "@/components/ui/avatar";
import { useState, useEffect } from "react";
import { useParams, useRouter } from "next/navigation";
import { IProject, IRole, IUser, IUserRights } from "@/types";
import { X } from "lucide-react";
import {
  Select,
  SelectItem,
  SelectContent,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import RoleEditor from "@/components/roleEditor";
import {
  HubConnectionBuilder,
  HubConnection,
  LogLevel,
  HubConnectionState,
} from "@microsoft/signalr";

interface IRoleDto {
  name: string;
  projectId: string;
  color: string;
}

export default function Page() {
  const { id } = useParams<{ id: string }>();
  const router = useRouter();
  const [projectSettings, setProjectSettings] = useState<IProject>(
    {} as IProject
  );
  const [conn, setConnection] = useState<HubConnection | null>(null);
  const [users, setUsers] = useState<IUser[]>([]);
  const [roles, setRoles] = useState<IRole[]>([]);
  const [selectedRole, setSelectedRole] = useState<IRole>({} as IRole);
  const [newRole, setNewRole] = useState<string>("");
  const [fetchRolesInfo, setFetch] = useState<boolean>(true);
  const [userToRole, setUserToRole] = useState<string>("");
  const [username, setUsername] = useState<string>("");

  useEffect(() => {
    if (username === "") setUsers(projectSettings.users);
    else
      setUsers(
        projectSettings.users.filter((user) => user.username.includes(username))
      );
  }, [username]);

  useEffect(() => {
    setUsers(projectSettings.users);
  }, [projectSettings]);

  useEffect(() => {
    async function fetchInfo() {
      const response = await fetch(
        `http://localhost:5000/api/project/${id}/settings`,
        {
          method: "GET",
          headers: { "Content-Type": "application/json" },
          credentials: "include",
        }
      );
      if (response.ok) {
        const data = await response.json();
        setProjectSettings(data);
        setProjectSettings((prev) => ({
          ...prev,
          users: [...data.users, data.owner],
        }));
      }
    }
    async function fetchRoles() {
      const response = await fetch(
        `http://localhost:5000/api/role?projectId=${id}`,
        {
          method: "GET",
          headers: { "Content-Type": "application/json" },
          credentials: "include",
        }
      );
      if (response.ok) {
        const data = await response.json();
        setRoles(data);
        setSelectedRole(data[0]);
      }
    }
    fetchInfo();
    if (fetchRolesInfo) {
      fetchRoles();
      setFetch(false);
    }
  }, [fetchRolesInfo]);

  useEffect(() => {
    async function start() {
      let connection = new HubConnectionBuilder()
        .withUrl(`http://localhost:5000/chat`)
        .build();
      setConnection(connection);
      if (connection.state === HubConnectionState.Disconnected) {
        await connection.start();
      } else {
        console.log("Already connected");
      }
    }
    start();
  }, []);

  const handleAddUserToRole = async (userId: string) => {
    setUserToRole("");
    await fetch(
      `http://localhost:5000/api/role/${selectedRole.id}/adduser?userId=${userId}`,
      {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
      }
    );
    setFetch(true);
  };

  const handleRemoveUserFromRole = async (userId: string) => {
    await fetch(
      `http://localhost:5000/api/role/${selectedRole.id}/removeuser?userId=${userId}`,
      {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
      }
    );
    setFetch(true);
  };

  const handleSaveRole = async () => {
    await fetch(
      `http://localhost:5000/api/role/${selectedRole.id}/name?name=${selectedRole.name}`,
      {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
      }
    );
    const rights = {
      canAddRoles: selectedRole.canAddRoles,
      canAddStage: selectedRole.canAddStage,
      canAddTask: selectedRole.canAddTask,
      canCompleteTask: selectedRole.canCompleteTask,
      canDeleteStage: selectedRole.canDeleteStage,
      canDeleteTask: selectedRole.canDeleteTask,
      canExcludeUser: selectedRole.canExcludeUser,
      canInviteUser: selectedRole.canInviteUser,
      canModifyRoles: selectedRole.canModifyRoles,
      canModifyStage: selectedRole.canModifyStage,
      canModifyTask: selectedRole.canModifyTask,
      canUseChat: selectedRole.canUseChat,
    } as IUserRights;
    const response = await fetch(
      `http://localhost:5000/api/role/${selectedRole.id}`,
      {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
        body: JSON.stringify(rights),
      }
    );
    setFetch(true);
  };

  const handleCreateRole = async () => {
    const response = await fetch(`http://localhost:5000/api/role`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
      body: JSON.stringify({
        name: newRole === "" ? "Новая роль" : newRole,
        projectId: id,
        color: "#FFFF",
      } as IRoleDto),
    });
    setFetch(true);
    setNewRole("");
  };
  return (
    <div className="flex flex-row items-start">
      <div className="flex flex-col items-center gap-1">
        <Input
          className="text-center rounded-t-full focus-visible:outline-none focus-visible:ring-0  dark:border-black/20"
          placeholder="Название роли"
          value={newRole}
          onChange={(e) => setNewRole(e.target.value)}
        ></Input>
        <Button
          className="text-white font-semibold w-full rounded-b-full bg-[#00b4d8] hover:bg-[#0095b3] ease-in-out duration-300"
          onClick={() => handleCreateRole()}
        >
          Создать роль
        </Button>
      </div>
      {roles ? (
        <ul className="list-none ml-8 rounded-l-full dark:bg-transparent">
          {roles.map((role) => (
            <li
              className={
                selectedRole.id === role.id
                  ? "py-[0.3rem] px-[1rem] rounded-l-full bg-[#90e0ef] dark:bg-black/30"
                  : "py-[0.3rem] px-[1rem] bg-white dark:bg-transparent"
              }
              key={role.id}
              onClick={() => setSelectedRole(role)}
            >
              <div
                className={
                  selectedRole.id === role.id
                    ? ""
                    : "hover:bg-gray-50 rounded-l-full ease-in-out duration-300 dark:hover:bg-black/20"
                }
              >
                {role.name}
              </div>
            </li>
          ))}
        </ul>
      ) : (
        <></>
      )}
      {selectedRole ? (
        <RoleEditor
          selectedRole={selectedRole}
          setSelectedRole={setSelectedRole}
          handleSaveRole={handleSaveRole}
          doFetch={() => setFetch(true)}
        />
      ) : (
        <></>
      )}
      {selectedRole ? (
        <div className="flex flex-col gap-2  ml-4">
          {selectedRole.users && selectedRole.users.length !== 0 ? (
            <Label className="w-full text-center">Обладатели роли</Label>
          ) : (
            <></>
          )}
          {selectedRole ? (
            selectedRole.users?.map((user) => (
              <div
                key={user.id}
                className="bg-gray-50 flex flex-row items-center p-2 rounded-full dark:bg-[#111]"
              >
                <div className="flex flex-row items-center">
                  <Avatar
                    style={{
                      backgroundColor: "#4198FF",
                      color: "white",
                      width: "2.5vh",
                      height: "2.5vh",
                      fontSize: "0.6rem",
                      margin: "0.1rem",
                      marginLeft: "0.4rem",
                      display: "flex",
                      justifyContent: "center",
                      alignItems: "center",
                    }}
                  >
                    {user?.firstName?.slice(0, 1)}
                    {user?.lastName?.slice(0, 1)}
                  </Avatar>
                  <div
                    style={{
                      display: "flex",
                      flexDirection: "column",
                      alignItems: "center",
                    }}
                  >
                    <Label>{user.username}</Label>
                    <Label>{`${user.firstName} ${user.lastName}`}</Label>
                  </div>
                </div>
                <X
                  onClick={() => handleRemoveUserFromRole(user.id)}
                  size={18}
                  className="rounded-full hover:bg-gray-200 p-1 ease-in-out duration-150 ml-2"
                ></X>
              </div>
            ))
          ) : (
            <div></div>
          )}
          <Label className="w-full text-center">Назначить роль</Label>
          <Select
            value={userToRole}
            onValueChange={(value) => handleAddUserToRole(value)}
          >
            <SelectTrigger
              className="SelectTrigger dark:bg-[#111] dark:border-black/20"
              aria-label="Food"
            >
              <SelectValue placeholder="Пользователь" />
            </SelectTrigger>
            <SelectContent>
              {projectSettings &&
                projectSettings.users?.map((user) => (
                  <SelectItem key={user.username} value={user.id}>
                    <div
                      style={{
                        display: "flex",
                        flexDirection: "row",
                        alignItems: "center",
                        gap: 8,
                      }}
                    >
                      <Avatar
                        style={{
                          backgroundColor: "#4198FF",
                          color: "white",
                          width: "2.5vh",
                          height: "2.5vh",
                          fontSize: "0.6rem",
                          margin: "0.1rem",
                          marginLeft: "0.4rem",
                          display: "flex",
                          justifyContent: "center",
                          alignItems: "center",
                        }}
                        // src="/static/images/avatar/1.jpg"
                      >
                        {user?.firstName?.slice(0, 1)}
                        {user?.lastName?.slice(0, 1)}
                      </Avatar>
                      <div>
                        {user.firstName +
                          " " +
                          user.lastName +
                          " (" +
                          user.username +
                          ")"}
                      </div>
                    </div>
                  </SelectItem>
                ))}
            </SelectContent>
          </Select>
        </div>
      ) : (
        <></>
      )}
    </div>
  );
}
