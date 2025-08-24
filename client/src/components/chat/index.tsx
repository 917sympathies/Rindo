"use client";
import { useState, useEffect, Dispatch, SetStateAction } from "react";
import { X } from "lucide-react";
import { Label } from "../ui/label";
import { Input } from "../ui/input";
import { Send } from "lucide-react";
import { IUserInfo, CookieInfo } from "@/types";
import { ScrollArea } from "@/components/ui/scroll-area";
import { HubConnectionBuilder, HubConnection, HubConnectionState} from "@microsoft/signalr";
import dayjs from "dayjs";
import { GetChatInfo, GetUserInfo } from "@/requests";
import { Message } from "./models/message.model";
import { Chat as ChatModel } from "./models/chat.model";

interface ChatProps {
    chatId: string | undefined;
    projectName: string | undefined;
    isActive: boolean;
    setIsChatActive: Dispatch<SetStateAction<boolean>>
}

export default function Chat({
    isActive,
    setIsChatActive,
    chatId,
    projectName,
}: ChatProps) {
    const [message, setMessage] = useState<string>("");
    const [chatMessages, setChatMessages] = useState<Message[]>([])
    const [chat, setChat] = useState<ChatModel | null>(null);
    const [user, setUser] = useState<IUserInfo | null>(null);
    const [conn, setConnection] = useState<HubConnection | null>(null);

    useEffect(() => {
        if(!isActive) return;
        const getChatInfo = async () => {
            const response = await GetChatInfo(chatId!);
            const data = await response.json();
            setChat(data);
            setChatMessages(data.messages)
        };
        const getUserInfo = async () => {
            const userId = localStorage.getItem("userId");
            const response = await GetUserInfo(userId!);
            const data = await response.json();
            setUser(data);
        };
        getChatInfo();
        getUserInfo();
    }, [isActive]);

    useEffect(() => {
        async function start() {
            let connection = new HubConnectionBuilder()
                .withUrl(`http://localhost:5000/chat`)
                .build();
            setConnection(connection);
            if (connection.state === HubConnectionState.Disconnected) {
                await connection.start();
            }
            else {
                console.log("Already connected");
            }
        }
        if(isActive) start();
    }, [isActive, chatId]);

    try {
        if(conn){
            conn.on(`ReceiveProjectChat${chatId}`, (id, username, message, chatId, time) => {
                const str: Message = {id: id, username: username, content: message, chatId: chatId, time: time };
                setChatMessages([...chatMessages, str])
            });
        }
    } catch (exception) {
        console.log(exception);
    }

    const sendMessage = async () => {
        if(!conn) return;
        if (conn.state === HubConnectionState.Connected) {
            conn.invoke("SendProjectChat", user?.id, message, chatId).then(() => setMessage(""));
        } else {
            console.log("sendMsg: " + conn.state);
        }
    };

    return (
        <div className="flex flex-col justify-between w-full h-full mb-[2rem]">
        <div className="flex flex-row justify-between p-4">
            <Label className="text-[1.2rem] pl-[2rem]">
            {`Чат проекта: ${projectName}`}
            </Label>
            <X onClick={() => setIsChatActive(false)} className=""/>
        </div>
            <ScrollArea className="grow h-full">
            <div className="flex flex-col px-[1rem]">
            {chatMessages.length == 0 ? 
            <div className="flex self-center text-gray-400">
                Тут пока нет сообщений...
            </div> : <></>}
            {chatMessages?.map((message, index) =>
                user && message.username === user?.username ? (
                <div key={index} className="flex flex-col">
                    <div className="self-end">
                    <Label className="text-center text-[0.8rem] text-[#87888C]">
                        {message ? dayjs(message.time).format("DD.MM.YY HH:mm") : ""}
                    </Label>
                    </div>
                    <div className="flex justify-end my-[.15rem]">
                    <div className="bg-[#3288F0] px-[0.8rem] py-[0.2rem] rounded-[0.6rem]">
                        <div className="flex justify-between items-baseline">
                        <Label className="text-white">
                            {message.content}
                        </Label>
                        </div>
                    </div>
                    </div>
                </div>
                ) : (
                <div key={index}>
                    <div>
                    <Label className="text-center text-[0.8rem] text-[#87888C]">
                        {message ? dayjs(message.time).format("DD.MM.YY HH:mm") : ""}
                    </Label>
                    </div>
                    <div className="flex justify-start my-[0.15rem]">
                    <div className="bg-[#ECF0F3] px-[0.8rem] py-[0.2rem] rounded-[0.6rem]">
                        <Label className="text-[#87888C] text-[0.7rem]">
                        {message.username}
                        </Label>
                        <div className="flex justify-between items-baseline">
                        <Label className="font-medium text-[0.9rem] text-black">
                            {message.content}
                        </Label>
                        </div>
                    </div>
                    </div>
                </div>
                )
            )}
            </div>
            </ScrollArea>
        <div className="flex justify-center">
            <div className="py-[.6rem] px-[.8rem] flex justify-between items-center mt-[.5rem] mr-0 mb-[1vh] ml-0 gap-7 w-[85%]">
            <Input
                className="flex-1 dark:text-white dark:bg-[#111] dark:border-black/20"
                placeholder="Написать сообщение..."
                value={message}
                onChange={(e) => setMessage(e.target.value)}
                onKeyDown={(e) => {
                    if(e.key === 'Enter') sendMessage();
                }}
            />
            <Send className="text-[.9rem] text-sky-600 hover:text-sky-800 ease-in-out duration-300"onClick={() => sendMessage()}/>
            </div>
        </div>
        </div>
    );
}
