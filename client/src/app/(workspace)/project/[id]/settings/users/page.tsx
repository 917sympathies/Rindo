"use client";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { Label } from "@/components/ui/label";
import { Avatar } from "@/components/ui/avatar";
import { Dialog, DialogContent } from "@/components/ui/dialog";
import { useState, useEffect } from "react";
import { useParams, useRouter } from "next/navigation";
import { IProject, IUser } from "@/types";
import { CirclePlus, Crown, X } from "lucide-react";
import AddUserModal from "@/components/addUserModal";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardFooter,
  CardTitle,
} from "@/components/ui/card";
import { GetSettingsInfo, RemoveUserFromProject } from "@/requests";

export default function Page() {
  const { id } = useParams<{ id: string }>();
  const router = useRouter();
  const [projectSettings, setProjectSettings] = useState<IProject>(
    {} as IProject
  );
  const [users, setUsers] = useState<IUser[]>([]);
  const [isUserModalOpen, setIsUserModalOpen] = useState<boolean>(false);
  const [isUserDeleteModal, setIsUserDeleteModal] = useState<boolean>(false);
  const [userOnDelete, setUserOnDelete] = useState<IUser>({} as IUser);
  const [fetchRolesInfo, setFetch] = useState<boolean>(true);
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
      const response = await GetSettingsInfo(id);
      if (response.ok) {
        const data = await response.json();
        setProjectSettings(data);
        setProjectSettings((prev) => ({
          ...prev,
          users: [...data.users],
        }));
      }
    }
    fetchInfo();
    if (fetchRolesInfo) {
      setFetch(false);
    }
  }, [fetchRolesInfo]);

  const handleRemoveUser = async () => {
    const response = await RemoveUserFromProject(id, userOnDelete.username);
    setFetch(true);
    setIsUserDeleteModal(false);
    setUserOnDelete({} as IUser);
  };

  return (
    <>
      <div style={{ color: "black", width: "80%" }}>
        <div className="flex flex-row items-center gap-4 mb-4 ">
          <Input
            className="w-[40%]"
            placeholder="Введите имя пользователя"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
          ></Input>
          <Button
            className="text-white bg-blue-500 hover:bg-blue-800 rounded-lg flex flex-row gap-2"
            onClick={() => setIsUserModalOpen(true)}
          >
            <CirclePlus size={18} />
            <div>Пригласить пользователя</div>
          </Button>
        </div>
        <div className="flex flex-row flex-wrap gap-2">
          {users ? (
            users.map((user) => (
              <Card
                className="dark:bg-[#111] dark:border-black/20"
                key={user.id}
              >
                <CardHeader>
                  <CardTitle className="flex flex-row gap-2 justify-between">
                    <div className="flex flex-row items-center gap-2">
                      {user.id === projectSettings.ownerId ? (
                        <Crown size={18} color="rgb(255, 255, 0)" />
                      ) : (
                        <></>
                      )}
                      {user.username}
                    </div>
                    {user.id !== projectSettings.ownerId ? (
                      <X
                        size={24}
                        className="p-[0.2rem] rounded-full hover:bg-[rgba(1,1,1,0.1)] ease-in-out duration-200"
                        onClick={() => {
                          setIsUserDeleteModal(true);
                          setUserOnDelete(user);
                        }}
                      ></X>
                    ) : (
                      <></>
                    )}
                  </CardTitle>
                  <CardDescription className="flex flex-col dark:text-gray-400 text-[0.9rem]">
                    <span>Имя: {user.firstName}</span>
                    <span>Фамилия: {user.lastName}</span>
                    <span>E-Mail: {user.email}</span>
                  </CardDescription>
                </CardHeader>
                <CardContent>
                  <Avatar
                    style={{
                      backgroundColor: "#4198FF",
                      color: "white",
                      width: "3.5vh",
                      height: "3.5vh",
                      fontSize: "1.2rem",
                      margin: "0.1rem",
                      marginLeft: "0.4rem",
                      display: "flex",
                      justifyContent: "center",
                      alignItems: "center",
                    }}
                  >
                    {user?.firstName?.slice(0, 1).toUpperCase()}
                    {user?.lastName?.slice(0, 1).toUpperCase()}
                  </Avatar>
                  <div
                    style={{
                      display: "flex",
                      flexDirection: "column",
                      alignItems: "center",
                    }}
                  ></div>
                </CardContent>
              </Card>
            ))
          ) : (
            <div></div>
          )}
        </div>
      </div>
      <Dialog open={isUserModalOpen}>
        <DialogContent>
          <AddUserModal onClose={() => setIsUserModalOpen(false)} />
        </DialogContent>
      </Dialog>
      <Dialog open={isUserDeleteModal}>
        <DialogContent>
          <div
            style={{
              display: "flex",
              flexDirection: "column",
              justifyContent: "center",
              backgroundColor: "white",
              width: "100%",
              borderRadius: "8px",
              padding: "10px",
              gap: 8,
            }}
          >
            <Label style={{ color: "black", alignSelf: "center" }}>
              Вы действительно хотите удалить пользователя{" "}
              {userOnDelete.username} из проекта?
            </Label>
            <div style={{ display: "flex", alignSelf: "center" }}>
              <Button
                onClick={() => handleRemoveUser()}
                style={{
                  color: "white",
                  backgroundColor: "green",
                  marginRight: "0.4rem",
                }}
              >
                Да
              </Button>
              <Button
                onClick={() => setIsUserDeleteModal(false)}
                style={{ color: "white", backgroundColor: "red" }}
              >
                Нет
              </Button>
            </div>
          </div>
        </DialogContent>
      </Dialog>
    </>
  );
}
