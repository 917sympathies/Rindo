"use client";
import { useState, useEffect } from "react";
import { Avatar } from "../ui/avatar";
import { Invitation, UserDto } from "@/types";
import { Input } from "../ui/input";
import { Label } from "../ui/label";
import { Button } from "../ui/button";
import { HubConnectionBuilder, HubConnection, HubConnectionState } from "@microsoft/signalr";
import { deleteInvite, getInvitesByUserId, updateUser as updateUserRequest, getUser as getUserRequest } from "@/requests";

export interface InvitationDto {
    invitationId: string;
    projectId: string;
    senderId: string;
    projectName: string;
    senderUsername: string;
}

export default function ProfileSettings() {
    const [user, setUser] = useState<UserDto>({} as UserDto);
    const [lastName, setLastName] = useState<string>("");
    const [firstName, setFirstName] = useState<string>("");
    const [email, setEmail] = useState<string>("");
    const [invites, setInvites] = useState<InvitationDto[]>([]);
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

    const handleAcceptInvite = async (invite: InvitationDto) => {
        if (!conn) return;
        if (conn.state === HubConnectionState.Connected) {
            conn.invoke("SendAcceptInvite", invite.invitationId, invite.projectId, user.id);
            setInvites(invites.filter((inv) => inv.invitationId !== invite.invitationId));
        }
    };

    useEffect(() => {
        const value = localStorage.getItem("user");
        if (!value) return;
        const user = JSON.parse(value);
        getInvitations(user.id);
        getUser(user.id);
    }, []);

    useEffect(() => {
        localStorage.removeItem("user");
        localStorage.setItem("user", JSON.stringify(user));
    }, [user]);

    const getUser = async (id: string) => {
        getUserRequest(id)
            .then(response => {
                setUser(response.data);
                setLastName(response.data.lastName);
                setFirstName(response.data.firstName);
                setEmail(response.data.email);
            });
    }

    const getInvitations = async (id: string) => {
        const response = await getInvitesByUserId(id);
        const data = response.data;
        setInvites(data);
    };

    const handleDeclineInvite = async (invite: InvitationDto) => {
        const response = await deleteInvite(invite.invitationId);
        if (response.status === 200) setInvites(invites.filter((inv) => inv.invitationId != invite.invitationId));
    };

    const updateUser = async () => {
        updateUserRequest({
            email: email,
            firstName: firstName,
            lastName: lastName,
            id: user.id,
            username: user.username,
        });
    };

    return (
        <div className="flex flex-col w-full h-full justify-between">
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
                </div>
                <div className="w-90 mr-24 pt-6">
                    <Label className="pb-4 text-[1.4rem]">Приглашения</Label>
                    {invites?.map(invite => (
                        <div className="border rounded-lg p-2 flex flex-col gap-2" key={invite.invitationId}>
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
            <Button
                className="w-1/5"
                variant={"save2"}
                onClick={() => updateUser()}
            >
                Сохранить
            </Button>
        </div>
    );
}
