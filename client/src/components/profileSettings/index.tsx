"use client";
import { useState, useEffect } from "react";
import { Avatar } from "../ui/avatar";
import { IInvitation, IUser } from "@/types";
import { Input } from "../ui/input";
import { Label } from "../ui/label";
import { Button } from "../ui/button";
import {
  HubConnectionBuilder,
  HubConnection,
  LogLevel,
  HubConnectionState,
} from "@microsoft/signalr";
import { DeleteInvite, GetInvitesByUserId, UpdateUserEmail, UpdateUserFirstName, UpdateUserLastName } from "@/requests";

export default function ProfileSettings() {
  const [user, setUser] = useState<IUser>({} as IUser);
  const [lastName, setLastName] = useState<string>("");
  const [firstName, setFirstName] = useState<string>("");
  const [email, setEmail] = useState<string>("");
  const [invites, setInvites] = useState<IInvitation[]>([]);
  const [conn, setConnection] = useState<HubConnection | null>(null);

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

  try {
    if (conn) {
      conn.on(`HelloMsg`, (message) => {
        console.log(message);
      });
    }
  } catch (exception) {
    console.log(exception);
  }

  const handleAcceptInvite = async (invite: IInvitation) => {
    if (!conn) return;
    if (conn.state === HubConnectionState.Connected) {
      conn.invoke(
        "SendAcceptInvite",
        invite.id,
        invite.projectId,
        invite.userId
      );
      setInvites(invites.filter((inv) => inv.id !== invite.id));
    }
  };

  useEffect(() => {
    const value = localStorage.getItem("user");
    if (!value) return;
    const user = JSON.parse(value);
    getInvitations(user.id);
    setUser(user);
    setLastName(user.lastName);
    setFirstName(user.firstName);
    setEmail(user.email);
  }, []);

  useEffect(() => {
    localStorage.removeItem("user");
    localStorage.setItem("user", JSON.stringify(user));
  }, [user]);

  const getInvitations = async (id: string) => {
    const response = await GetInvitesByUserId(id);
    const data = await response.json();
    setInvites(data);
  };

  const handleDeclineInvite = async (invite: IInvitation) => {
    const response = await DeleteInvite(invite.id);
    if(response.ok) setInvites(invites.filter((inv) => inv.id != invite.id));
  };

  const updateUser = async () => {
    if (firstName !== user?.firstName) 
    {
      const response = await UpdateUserFirstName(user.id, firstName);
      if (response.ok ) setUser({ ...user, firstName: firstName });
    }
    if (lastName !== user?.lastName) 
    {
      const response = await UpdateUserLastName(user.id, lastName);
      if (response.ok) setUser({ ...user, lastName: lastName });
    }
    if (email != user?.email) 
      {
      const response = await UpdateUserEmail(user.id, email);
      if (response.ok) setUser({ ...user, email: email });
    }
  };

  return (
    <>
      <div className="flex flex-row w-full justify-between">
        <div className="flex flex-col gap-4">
          <div className="flex flex-row items-end justify-between gap-6 w-full border-b pb-2 pl-4 pr-4">
            <Label className="pb-2">ФИО</Label>
            <div>
              <Label>Фамилия</Label>
              <Input
                value={lastName}
                onChange={(e) => setLastName(e.target.value)}
              />
            </div>
            <div>
              <Label>Имя</Label>
              <Input
                value={firstName}
                onChange={(e) => setFirstName(e.target.value)}
              />
            </div>
          </div>
          <div className="flex flex-row items-center w-full justify-between pr-4 pl-4">
            <Label>E-mail</Label>
            <Input
              className="w-[88%]"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
            />
          </div>
          <Button
            className="w-2/5"
            variant={"save2"}
            onClick={() => updateUser()}
          >
            Сохранить
          </Button>
        </div>
        <div className="w-90 mr-24 pt-6">
          <Label className="pb-4 text-[1.4rem]">Приглашения</Label>
          {invites?.map((invite) => (
            <div className="border rounded-lg p-2 flex flex-col gap-2">
              <Label>
                Пользователь {invite.senderUsername} пригласил вас в проект{" "}
                {invite.projectName}
              </Label>
              <div className="w-90 flex flex-row items-center justify-around">
                <Button
                  variant={"save2"}
                  onClick={() => handleAcceptInvite(invite)}
                >
                  Принять
                </Button>
                <Button
                  variant={"destructive"}
                  onClick={() => handleDeclineInvite(invite)}
                >
                  Отказаться
                </Button>
              </div>
            </div>
          ))}
        </div>
      </div>
    </>
  );
}
